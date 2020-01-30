using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public static bool pieceMoving;
    public int pos;//initial position of the piece
    public Vector3 offset;

    void Start()
    {
        pieceMoving = false;
        JumpTo(0);
    }

    public void JumpTo(int i)
    {
        
        if (i < BoardWaypoints._NUM_BOXES-1)
        {
            pos = i;
            transform.position =offset+ BoardWaypoints.Instance.waypoints[i];
            /*if (BoardWaypoints.Instance.snl[pos] != -1&&BoardWaypoints.Instance.snl[pos]!=pos)
            {
                JumpTo(BoardWaypoints.Instance.snl[pos]);
                return;
            }*/
            return;
        }
        if(i==BoardWaypoints._NUM_BOXES-1)
        {
            //trigger Win game animation
            pos = i;
            transform.position = offset + BoardWaypoints.Instance.waypoints[i];
            return;
        }
        
        //StartCoroutine(TransitTo(i));
        //show message that the players turn has gone to waste
    }

    public IEnumerator TransitTo(int i)
    {
        while (pieceMoving)
            yield return new WaitForSeconds(0.1f);
        pieceMoving = true;
        while (i < BoardWaypoints._NUM_BOXES-1)
        {
            if (pos >= BoardWaypoints._NUM_BOXES - 1||pos<0)
            {
                JumpTo(i);
            }
            while (pos != i)
            {
                if (pos > i)
                    Jump(-1);
                else
                    Jump(1);
                yield return new WaitForSeconds(0.1f);
            }
            if (BoardWaypoints.Instance.snl[pos] != -1 && BoardWaypoints.Instance.snl[pos] != pos)
            {
                i = BoardWaypoints.Instance.snl[pos];
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.5f);

        }
        if (i == BoardWaypoints._NUM_BOXES-1)
        {
            while (pos != i)
            {
                //pos += (pos > i ? -1 : 1);
                //transform.Translate(offset + BoardWaypoints.Instance.waypoints[pos]);
                if (pos < i)
                    Jump(1);
                yield return new WaitForSeconds(0.1f);
            }
        }
        pieceMoving = false;
    }

    public void Jump(int i)
    {
        JumpTo(pos + i);
    }

    public void Transit(int i)
    {
        StartCoroutine(_Transit(i));
    }

    public IEnumerator _Transit(int i)
    {
        yield return TransitTo(pos + i);
    }

}
