﻿using MLAgents;
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

    const float BLOCKSCALE = .32f;
    //Manage the visual/colliders version of the snake 
    private Vector2 direction = Vector2.up * 0.32f;
    Vector2 last_dir = Vector2.zero;
    private bool lost = false;
    private bool ate = false;

    void Start() {
        InvokeRepeating("Move", 0.3f, 0.3f);
    }
    private void Move() {
        //Check if going backwards
        if (-last_dir.normalized == direction.normalized)
            direction = last_dir;

        Vector2 preMovePos = head.position;
        if (manager.Move(direction.normalized) == false) {
            CancelInvoke("Move");
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
    }

    public override void AgentReset() {
        if (lost) {
            last_dir = Vector2.zero;
            lost = false;
            ate = false;
            tail.Clear();
            CreateSnake();

        }
    }
    public override void CollectObservations() {
        // Target and Agent positions
        Observations obs = manager.getObservations();
        AddVectorObs(obs.position_head);
        AddVectorObs(obs.position_food);
        foreach (Vector2 v in obs.position_tail) {
            AddVectorObs(v);
        }
        foreach (Vector2 v in obs.position_walls) {
            AddVectorObs(v);
        }
        foreach (Vector2 v in obs.position_empty) {
            AddVectorObs(v);
        }
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

        // Rewards
        // Reached target
        if (ate) {
            SetReward(1.0f);
            Done();
        }

        // Fell off platform
        if (lost) {
            Done();
        }

    }
}

