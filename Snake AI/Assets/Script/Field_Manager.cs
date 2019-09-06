﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field_Manager : MonoBehaviour {
    public Transform wall_parent;
    public Transform food;
    public Transform snake_dad;
    public Transform snake_head;
    public Transform wall;
    public Transform tail;
    private SnakeAgent snake;
    public TMPro.TextMeshProUGUI score;
    public TMPro.TextMeshProUGUI time;
    Queue<Vector2> last_tail = new Queue<Vector2>();
    Vector2 last_dir = Vector2.zero;
    Vector2 head;//height, width 
    float block_scale = .32f;
    int width = 64;
    int height = 32;
    Boolean fed = false;
    private int[,] field;
    private System.Random rnd;

    private Vector2 food_pos;


    private float start_time;
    // Start is called before the first frame update
    void Start() {
        rnd = new System.Random();
        field = new int[height, width];
        CreateWalls();
        CreateSnake();
        print_array(field);
        food = Instantiate(food, this.transform);
        RelocateFood();
        start_time = Time.time;
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            print_array(field);
        }
        time.SetText(Time.time.ToString("F2"));

    }
   public void RestartField() {
        field = new int[height, width];
        for (int i = 0; i < field.GetLength(0); i++) {
            for (int j = 0; j < field.GetLength(1); j++) {
                if (i == 0 || i == field.GetLength(0) - 1 || j == 0
                    || j == field.GetLength(1) - 1) {
                    field[i, j] = 9; //wall
                }
            }
        }
        start_time = Time.time;
        last_tail = new Queue<Vector2>();
        RelocateFood();
        CreateSnake();
    }
    public void RelocateFood() {
        int w = rnd.Next(1, width - 1 );
        int h = rnd.Next(1, height - 1);
        //Check if is in body of snake
        while (field[height - h - 1, w] != 0) {
            w = rnd.Next(1, width - 1);
            h = rnd.Next(1, height - 1);
        }
        food.position = new Vector3((w - width * 0.5f) * block_scale,
            (h - height * 0.5f) * block_scale, food.position.z);
        field[height-h -1, w] = 5;
        food_pos = new Vector2(height - h - 1, w);
    }

    public Observations getObservations() {
        return new Observations(field,head, food_pos, Time.time-start_time);
    }

    public void Increase_size()
    {
        snake_dad.GetComponent<SnakeAgent>().IncreaseSize();
        fed = true;
    }
    public bool Move(Vector2 pDir) {
        if (CheckForWallsAndTail(pDir)) {            
            return false;
        } else if (CheckForFood(pDir)) {
            //Manage food and growth before moving! Order matters.
            Increase_size();
            RelocateFood();            
            IncreaseScore();
        }

        MoveHead(pDir);

        if (!fed) { //If we get food then avoid erasing last tail
            Vector2 tail_pos = last_tail.Dequeue();
            field[(int)tail_pos.x, (int)tail_pos.y] = 0;
        }            
        
        fed = false;
        print_array(field);
        return true;
    }
    private void MoveHead(Vector2 pDir) {
        last_tail.Enqueue(new Vector2(head.x, head.y));
        field[(int)head.x, (int)head.y] = 2; //Add tail at prev pos of head

        head.x -= pDir.y; //If is going up in real world then have to substract to go up in array
        head.y += pDir.x;
        field[(int)head.x, (int)head.y] = 3; //Add head
    }
    /// <summary>
    /// Create stuff
    /// </summary>
    private void CreateSnake() {
        //snake_head = Instantiate(snake_head, new Vector3(0, 0, 1),
        //    Quaternion.identity, snake_dad);
        snake = snake_dad.GetComponent<SnakeAgent>();
        snake.CreateSnake();
        //snake.tail = new List<Transform>();
        //snake.set_manager(this);
        //Transform t1 = Instantiate(tail, new Vector3(0, -1 * block_scale, 1),
        //    Quaternion.identity, snake_dad);
        //Transform t2 = Instantiate(tail, new Vector3(0, -2 * block_scale, 1),
        //    Quaternion.identity, snake_dad);
        //snake.tail.Add(t1);
        //snake.tail.Add(t2);

        //Add snake elemtns to array
        field[height / 2 + 0, width / 2] = 2; //tail
        field[height / 2 + 1, width / 2] = 2; //tail
        field[height / 2 - 1, width / 2] = 3; //head

        head = new Vector2(height / 2 - 1, width / 2);
        last_tail.Enqueue(new Vector2(height / 2 + 1, width / 2));
        last_tail.Enqueue(new Vector2(height / 2 + 0, width / 2));
    }
    /// <summary>
    /// 9 is wall
    /// </summary>
    private void CreateWalls() {
        //Creates wall in the array
        for (int i = 0; i < field.GetLength(0); i++) {
            for (int j = 0; j < field.GetLength(1); j++) {
                if (i == 0 || i == field.GetLength(0) - 1 || j == 0
                    || j == field.GetLength(1) - 1) {
                    field[i, j] = 9; //wall
                    Instantiate(wall, new Vector3((j - width * 0.5f) * block_scale,
                        (i - height * 0.5f) * block_scale, 1), Quaternion.identity, wall_parent);
                }
            }
        }
    }

    /// <summary>
    /// Checks
    /// </summary>
    /// <param name="pDir"></param>
    /// <returns></returns>
    private bool CheckForWallsAndTail(Vector2 pDir) {
        Vector2 nextMove = new Vector2(head.x - pDir.y, head.y + pDir.x);
        if (field[(int)nextMove.x, (int)nextMove.y] == 9 || field[(int)nextMove.x, (int)nextMove.y] == 2) {
            Debug.Log("There is a wall, cant move");
            return true; //there is a wall or tail, game over.
        } else return false;
    }
    private bool CheckForFood(Vector2 pDir) {
        Vector2 nextMove = new Vector2(head.x - pDir.y, head.y + pDir.x);
        if (field[(int)nextMove.x, (int)nextMove.y] == 5) {
            return true; //there is a wall
        } else return false;
    }

    
    /// <summary>
    /// Text stuff
    /// </summary>
    /// <param name="typef"></param>
    private void IncreaseScore(int typef = 0) {
        if(typef == 0) {
            int scoreInt = Int32.Parse(score.GetParsedText());
            scoreInt += 100;
            score.SetText(""+scoreInt);
        }
    }
    
    void print_array(int[,] arra)
    {
        string linea = "";
        for (int i = 0; i < arra.GetLength(0); i++)
        {
            for (int j = 0; j < arra.GetLength(1); j++)
            {
                linea += arra[i, j].ToString();
            }
            linea += "\n";
        }
        Debug.Log(linea);
    }
}
//TODO: Reset manager when reseting snake