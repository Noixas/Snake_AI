using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Snake : MonoBehaviour
{
    //Manage the visual/colliders version of the snake 
    private Vector2 direction = Vector2.up * 0.32f;
    // Start is called before the first frame update
    public List<Transform> tail = new List<Transform>();
    bool ate = false;
    private Field_Manager manager;
    void Start()
    {
        //InvokeRepeating("Move", 0.3f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            direction = Vector2.right*0.32f;
        else if (Input.GetKey(KeyCode.DownArrow))
            direction = -Vector2.up*0.32f;    // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow))
            direction = -Vector2.right * 0.32f; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow))
            direction = Vector2.up * 0.32f;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("Food"))
        {
            // Get longer in next Move call
            ate = true;
            //manager.RelocateFood();
        }
        // Collided with Tail or Border
        else
        {
         //   CancelInvoke("Move");
                // ToDo 'You lose' screen
        }
    }
    
    private void Move()
    {

        Vector2 preMovePos = transform.position;
        bool canMove = manager.Move(direction.normalized);
        if (canMove == false) {
            CancelInvoke("Move");
            canMove = true;
            return;
        }     
        // Do we have a Tail?
        if (ate)
        {
            //manager.increase_size(preMovePos);
            ate = false;
        }
        else if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = preMovePos;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
        transform.Translate(direction);
    }
    public void set_manager(Field_Manager mang)
    {
        manager = mang;
    }
}
