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
    Vector2 last_dir = Vector2.zero;
    void Start(){
        InvokeRepeating("Move", 0.3f, 0.3f);
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
    void OnTriggerEnter2D(Collider2D coll){
        // Food?
        if (coll.name.StartsWith("Food")){
            // Get longer in next Move call
            ate = true;
        }
    }
    
    private void Move()
    {
        //Check if going backwards
        if (-last_dir.normalized == direction.normalized)
            direction = last_dir;

        Vector2 preMovePos = transform.position;
        if (manager.Move(direction.normalized) == false) {
            CancelInvoke("Move");
            return;
        }     

        
        if (ate){            
            ate = false;
        }
        else if (tail.Count > 0) { // Do we have a Tail?            
            tail.Last().position = preMovePos;// Move last Tail part to where the Head was.
            tail.Insert(0, tail.Last()); // Add to front of list, remove from the back
            tail.RemoveAt(tail.Count - 1);
        }
        transform.Translate(direction);
        last_dir = direction;
    }
    public void set_manager(Field_Manager mang)
    {
        manager = mang;
    }
}
