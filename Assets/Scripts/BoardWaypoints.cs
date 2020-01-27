using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardWaypoints : MonoBehaviour
{
    public const int _NUM_BOXES = 100;
    public const int _NUM_COLUMNS = 10;
    private static BoardWaypoints _instance;
    public static BoardWaypoints Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        _instance = this;
    }
    /*public static Vector3[] waypoints {
        get
        {
            return _waypoints;
        }
    }*/
    public Vector3[] waypoints;
    public int[] snl;
    public GameObject[] sprites;
    //public int[] ladders = new int[_NUM_BOXES];
    public Vector3 point1;//location of point 1 on the board
    public Vector3 diff; //space difference bw two consecutive blocks
    void Start()
    {
        waypoints = new Vector3[_NUM_BOXES];
        snl = new int[_NUM_BOXES];
        sprites = new GameObject[_NUM_BOXES];
        waypoints[0] = point1;
        for(int i=1;i<_NUM_BOXES;i++)
        {
            if (i % _NUM_COLUMNS == 0)
                waypoints[i] = new Vector3(waypoints[i - 1].x, waypoints[i - 1].y + diff.y, 0f);
            else if ((i / _NUM_COLUMNS) % 2 == 0)
                waypoints[i] = new Vector3(waypoints[i - 1].x + diff.x, waypoints[i - 1].y, 0f);
            else
                waypoints[i] = new Vector3(waypoints[i - 1].x - diff.x, waypoints[i - 1].y, 0f);
        }

        for(int i=0;i<_NUM_BOXES;i++)
        {
            snl[i] = -1;
            //ladders[i] = -1;
        }
    }

    
}
