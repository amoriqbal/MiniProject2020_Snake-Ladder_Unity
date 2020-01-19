using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchBoxController : MonoBehaviour
{
    private static TouchBoxController _instance;
    public static TouchBoxController Instance
    {
        get;
        private set;
    }

    public GameObject[] TouchableTiles=new GameObject[BoardWaypoints._NUM_BOXES];
    public GameObject TouchableTilePrefab;
    //public Vector3 diff;
    //public Vector3 Pos1;
    private Vector3 offset;
    void Start()
    {
        //TouchableTiles[0]=GameObject.Instantiate(TouchableTilePrefab);
        //TouchableTiles[0].transform.position = Pos1;
        offset = TouchableTilePrefab.transform.position - BoardWaypoints.Instance.waypoints[0];
        for (int i = 1; i < BoardWaypoints._NUM_BOXES; i++)
        {
            TouchableTiles[i] = GameObject.Instantiate(TouchableTilePrefab);
            TouchableTiles[i].transform.position = BoardWaypoints.Instance.waypoints[i] + offset;
            /*if (i % BoardWaypoints._NUM_COLUMNS == 0)
                TouchableTiles[i].transform.position = new Vector3(TouchableTiles[i - 1].transform.position.x, TouchableTiles[i - 1].transform.position.y + diff.y, 0f);
            else if ((i / BoardWaypoints._NUM_COLUMNS) % 2 == 0)
                TouchableTiles[i].transform.position = new Vector3(TouchableTiles[i - 1].transform.position.x + diff.x, TouchableTiles[i - 1].transform.position.y, 0f);
            else
                TouchableTiles[i].transform.position = new Vector3(TouchableTiles[i - 1].transform.position.x - diff.x, TouchableTiles[i - 1].transform.position.y, 0f);
            */

        }
    }


    private void Awake()
    {
        _instance = this;
    }

}
