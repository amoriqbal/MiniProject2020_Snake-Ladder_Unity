using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private bool _pieceMoving;
    public bool pieceMoving
    {
        get
        {
            return _pieceMoving;
        }
        set
        {
            _pieceMoving = value;
            Debug.Log("piecemoving :" + value.ToString());
        }
    }
    public const int ladderAnimFrames=10;
    public int pos;//initial position of the piece
    public Vector3 offset;

    void Start()
    {
        pieceMoving = false;
        JumpTo(0);
    }

    public void JumpTo(int i)
    {
        if(i<0)
        {
            Debug.LogError("Jumpto negative command:" + i);
        }
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
        while (i < BoardWaypoints._NUM_BOXES-1&&i>=0)
        {
            Debug.Log("transitto function running");
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
                //i = BoardWaypoints.Instance.snl[pos];

                //snake or ladder encountered
                pieceMoving = false;
                i = BoardWaypoints.Instance.snl[pos];
                SnlTransition(pos);
                while (pieceMoving)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                pieceMoving = true;
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

    public void SnlTransition(int i)
    {
        StartCoroutine(_SnlTransition(i));
    }

    IEnumerator _SnlTransition(int i)
    {
        while(pieceMoving)
        {
            yield return new WaitForSeconds(0.1f);
        }
        pieceMoving = true;
        
        if(BoardWaypoints.Instance.snl[i]<i)
        {
            //snake
            if(i>=BoardWaypoints._NUM_BOXES||i<0)
            {
                Exception e = new Exception("snl data corrupted");
                throw e;
            }
            JumpTo(BoardWaypoints.Instance.snl[i]);
            
        }

        else if(BoardWaypoints.Instance.snl[i]>i)
        {
            //ladder
            if (i >= BoardWaypoints._NUM_BOXES || i < 0)
            {
                Exception e = new Exception("snl data corrupted");
                throw e;
            }
            pieceMoving = false;
            LadderAnimation(BoardWaypoints.Instance.snl[i]);
            while (pieceMoving)
            {
                yield return new WaitForSeconds(0.1f);
            }
            pieceMoving = true;
        }

        pieceMoving = false;
    }

    public void LadderAnimation(int i)
    {
        StartCoroutine(_LadderAnimaton(i));
    }

    IEnumerator _LadderAnimaton(int i)
    {
        while (pieceMoving)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        pieceMoving = true;

        Vector3 d = (BoardWaypoints.Instance.waypoints[i] + offset - transform.position)/ladderAnimFrames;
        for(int count=1;count<=ladderAnimFrames;count++)
        {
            transform.position = transform.position + d;
            yield return new WaitForSeconds(0.1f);
        }
        pieceMoving = false;
        Debug.Log("ladder anim finished");
        //pos = i;
        JumpTo(i);
    }

}


