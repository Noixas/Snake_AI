using MLAgents;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SnakeAgent : Agent
{

    //prefabs
    public Transform tail_part;
    public Transform head_prefab;
    //instances
    public Transform parent;
    public List<Transform> tail = new List<Transform>();
    private Transform head;
    public Field_Manager manager = null;
    private float prevDist = 100000;
    const float BLOCKSCALE = .32f;
    //Manage the visual/colliders version of the snake 
    private Vector2 direction = Vector2.up * 0.32f;
    Vector2 last_dir = Vector2.zero;
    private bool lost = false;
    private bool ate = false;
    private bool close = false;
    void Start() {
        InvokeRepeating("Move", 0.3f, 0.3f);
    }
    private void Update() {
        Debug.Log(direction);
    }
    private void Move() {
        //Check if going backwards
        if (-last_dir.normalized == direction.normalized)
            direction = last_dir;

        Vector2 preMovePos = head.position;
        if (manager.Move(direction.normalized) == false) {
            lost = true;
            return;
        }


        if (ate) {
            ate = false;
        } else if (tail.Count > 0) { // Do we have a Tail?            
            tail.Last().position = preMovePos;// Move last Tail part to where the Head was.
            tail.Insert(0, tail.Last()); // Add to front of list, remove from the back
            tail.RemoveAt(tail.Count - 1);
        }
        head.Translate(direction);
        last_dir = direction;
    }
    /// <summary>
    /// Create stuff
    /// </summary>
    public void CreateSnake() {
        Transform snake_dad = this.transform;
        head = Instantiate(head_prefab, new Vector3(0, 0, 1),
            Quaternion.identity, snake_dad);
        //snake = snake_head.GetComponent<Snake>();
        tail = new List<Transform>();
        Transform t1 = Instantiate(tail_part, new Vector3(0, -1 * BLOCKSCALE, 1),
            Quaternion.identity, snake_dad);
        Transform t2 = Instantiate(tail_part, new Vector3(0, -2 * BLOCKSCALE, 1),
            Quaternion.identity, snake_dad);
        tail.Add(t1);
        tail.Add(t2);

    }
    public void IncreaseSize() {
        Vector3 pos = new Vector3(head.position.x,
            head.position.y, head.position.z);
        Transform t1 = Instantiate(tail_part, pos, head.rotation, this.transform);
        Debug.Log(t1.position);
        tail.Insert(0, t1);
        ate = true;
    }

    public override void AgentReset() {
        if (lost) {
            last_dir = Vector2.up * 0.32f;
            lost = false;
            ate = false;
            foreach(Transform t in tail) {
                Destroy(t.gameObject);
            }
            Destroy(head.gameObject);
            tail.Clear();

            manager.RestartField();
        }
    }
    public override void CollectObservations() {
        // Target and Agent positions
        Observations obs = manager.getObservations();

        //AddVectorObs(obs.position_head);
        //AddVectorObs(obs.position_food);
        AddVectorObs(direction);        
        AddVectorObs(Vector2.Distance(obs.position_food, obs.position_head));
        //foreach (Vector2 v in obs.position_tail) {
        //    AddVectorObs(v);
        //}
        //foreach (Vector2 v in obs.position_walls) {
        //    AddVectorObs(v);
        //}
        //foreach (Vector2 v in obs.position_empty) {
        //    AddVectorObs(v);
        //}
    }


    //public float speed = 10;
    public override void AgentAction(float[] vectorAction, string textAction) {
        // Actions, size = 2

        if (vectorAction[0] > 0)
            direction = Vector2.right * 0.32f;
        else if (vectorAction[1] < 0)
            direction = -Vector2.up * 0.32f;    // '-up' means 'down'
        else if (vectorAction[0] < 0)
            direction = -Vector2.right * 0.32f; // '-right' means 'left'
        else if (vectorAction[1] > 0)
            direction = Vector2.up * 0.32f;
        Observations obs = manager.getObservations();
        //AddReward(100- Vector2.Distance(obs.position_food, obs.position_head)*0.0001f);
        //AddReward();
        // Rewards
        // Reached target
        if (ate) {
            SetReward(40 + GetReward());
            Done();
        }
        //tood add time reward
        float dist = Vector2.Distance(obs.position_food, obs.position_head);
        if(dist < 15) {
            close = true;
        }
        if (close && dist > prevDist) {
            SetReward(-1 + 0.90f / dist + GetReward());
            Debug.Log("Dist " + dist);
            Done();
        } else if (dist < prevDist) {
            prevDist = dist;
            AddReward(.001f + 0.1f /dist);
        } else { 
            prevDist = dist;
        }
        // Fell off platform
        if (lost) {
            if (dist < .5f) {
                AddReward(.7f);
            } else if (dist < 1) {
                AddReward(.5f);
            } else if (dist < 2) {
                AddReward(.3f);
            } else if (dist < 5) {
                AddReward(.1f);
            } else if (dist < 15) {
                AddReward(.0015f);
            } else if (dist < 30) {
                AddReward(.0005f);
            } else {
                AddReward(-10);
            }
            SetReward(GetReward());
            //SetReward(0);
            Done();
            //AddReward(90 - Vector2.Distance(obs.position_food, obs.position_head));
        }

    }
}

