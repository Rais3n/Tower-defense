using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSquare : MonoBehaviour
{
    private int dir;
    private void Update()
    {
        if (dir == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(1, -4), 5*Time.deltaTime);
            if (transform.position.y == -4) dir = 1;
        }
        else if (dir == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-3, -4), 5 * Time.deltaTime);
            if (transform.position.x == -3) dir = 2;
        }
        else if (dir == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-3, 0), 5 * Time.deltaTime);
            if (transform.position.y == 0) dir = 3;
        }
        else if (dir == 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(1, 0), 5 * Time.deltaTime);
            if (transform.position.x == 1) dir = 0;
        }
    }

}
