using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public struct Observations {

    public float time;
    public Vector2 position_head;
    public Vector2 position_food;
    public List<Vector2> position_tail;
    public List<Vector2> position_empty;
    public List<Vector2> position_walls;
    public Observations(int[,] field, Vector2 head, Vector2 food, float pTime) {
        time = pTime;
        position_head = head;
        position_food = food;
        position_tail = new List<Vector2>();
        position_empty = new List<Vector2>();
        position_walls = new List<Vector2>();
        for (int i = 0; i < field.GetLength(0); i++) {
            for (int j = 0; j < field.GetLength(1); j++) {
                switch (field[i, j]) {
                    case 0:
                        position_empty.Add(new Vector2(j, i));
                        break;
                    case 2:
                        position_tail.Add(new Vector2(j, i));
                        break;
                    case 9:
                        position_walls.Add(new Vector2(j, i));
                        break;
                }
            }
        }


    }
}