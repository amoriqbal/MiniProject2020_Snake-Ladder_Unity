using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardWaypoints : MonoBehaviour
{
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
    public Vector3[] waypoints=new Vector3[100];
    //public int[] snakes = new int[100];
    //public int[] ladders = new int[100];
    public Vector3 point1;//location of point 1 on the board
    public Vector3 diff; //space difference bw two consecutive blocks
    void Start()
    {
        waypoints[0] = point1;
        for(int i=1;i<100;i++)
        {
            if (i % 10 == 0)
                waypoints[i] = new Vector3(waypoints[i - 1].x, waypoints[i - 1].y + diff.y, 0f);
            else if ((i / 10) % 2 == 0)
                waypoints[i] = new Vector3(waypoints[i - 1].x + diff.x, waypoints[i - 1].y, 0f);
            else
                waypoints[i] = new Vector3(waypoints[i - 1].x - diff.x, waypoints[i - 1].y, 0f);


        }
    }

    
}
