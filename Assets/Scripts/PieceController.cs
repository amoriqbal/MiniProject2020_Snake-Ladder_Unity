using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public int pos;//initial position of the piece
    public Vector3 offset;
    void Start()
    {
        pos = 0;
        JumpTo(0);
    }

    public void JumpTo(int i)
    {
        if (i < 99)
        {
            pos = i;
            transform.position =offset+ BoardWaypoints.Instance.waypoints[i];
            if (BoardWaypoints.Instance.snakes[pos] != -1)
            {
                JumpTo(BoardWaypoints.Instance.snakes[pos]);
                return;
            }
            if (BoardWaypoints.Instance.ladders[pos] != -1)
            {
                JumpTo(BoardWaypoints.Instance.ladders[pos]);
                return;
            }
            return;
        }
        if(i==99)
        {
            //trigger Win game animation
            pos = i;
            transform.position = offset + BoardWaypoints.Instance.waypoints[i];
            return;
        }

        //show message that the players turn has gone to waste
    }

    public void Jump(int i)
    {
        JumpTo(pos + i);
    }


}
