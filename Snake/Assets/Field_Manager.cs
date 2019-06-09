using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field_Manager : MonoBehaviour
{
    public Transform food;
    public Transform snake_dad;
    public Transform snake_head;
    public Transform wall;
    public Transform tail;
    private Snake snake;
    Vector2 last_tail;
    Vector2 head;
    float block_scale = .32f;
    int width = 64;
    int height = 32;
    // public Transform snake_tail;
    private int[,] field;
    private System.Random rnd;
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        field = new int[width, height];
        //Creates wall in the array
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (i == 0 || i == field.GetLength(0) - 1 || j == 0
                    || j == field.GetLength(1) - 1)
                {
                    field[i, j] = 9; //wall
                    Instantiate(wall,new Vector3((i- width*0.5f) * block_scale ,
                        (j- height * 0.5f )* block_scale, 1), Quaternion.identity);
                }
            }
        }
        last_tail = new Vector2(16, 16);
        head = new Vector2(13, 16);
        snake_head = Instantiate(snake_head, new Vector3(0, 0, 1),
            Quaternion.identity, snake_dad);
        snake = snake_head.GetComponent<Snake>();
        snake.tail = new List<Transform>();
        snake.set_manager(this);
        Transform t1 = Instantiate(tail, new Vector3(0, -1 * block_scale, 1), 
            Quaternion.identity, snake_dad);
        Transform t2 = Instantiate(tail, new Vector3(0, -2 * block_scale, 1), 
            Quaternion.identity, snake_dad);
        snake.tail.Add(t1);
        snake.tail.Add(t2);
        
        field[width/2, -1 + height/2] = 2; //tail
        field[width/2, -2 + height/2] = 2; //tail
        field[width/2,  0 + height/2] = 3; //head
        print_array(field);


    }
    public void RelocateFood()
    {
        int w = rnd.Next(1,width);
        int h = rnd.Next(1,height);

        food.position = new Vector3((w - width * 0.5f) * block_scale,
            ( h - height * 0.5f )*block_scale, food.position.z);
    }
    public void increase_size(Vector3 pos)
    {
        Transform t1 = Instantiate(tail, pos, snake_head.rotation);
        field[(int) (pos.x/ block_scale + width * 0.5f), (int)(pos.y / block_scale + height * 0.5f)] = 2; //tail
        snake.tail.Insert(0,t1);
    }
    public void Move(Vector2 pDir)
    {

      //  field[(int)head.x, (int)head.y] = 2;
      //  head += pDir;
       // field[(int)head.x, (int)head.y] = 3;
       // field[(int)last_tail.x, -(int)last_tail.y] = 0;
       // last_tail += pDir;
       // print_array(field);
    }

    // Update is called once per frame
    void Update()
    {
        
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
