using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        field = new int[width, height];
        snake = snake_head.GetComponent<Snake>();
        snake.set_manager(this);
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
        Transform snake_inst = Instantiate(snake_head, new Vector3(0, 0, 1),
            Quaternion.identity, snake_dad);
        Snake snake_script = snake_inst.GetComponent<Snake>();
        snake_script.tail = new List<Transform>();
        Transform t1 = Instantiate(tail, new Vector3(0, -1 * block_scale, 1), 
            Quaternion.identity, snake_dad);
        Transform t2 = Instantiate(tail, new Vector3(0, -2 * block_scale, 1), 
            Quaternion.identity, snake_dad);
        snake_script.tail.Add(t1);
        snake_script.tail.Add(t2);
        
        field[width/2, -1 + height/2] = 2; //tail
        field[width/2, -2 + height/2] = 2; //tail
        field[width/2,  0 + height/2] = 3; //head
        print_array(field);


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
