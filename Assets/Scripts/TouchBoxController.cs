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

    bool snakeState, ladderState, createLadder,createSnake;
    int ladderStartIndex, snakeStartIndex;
    private Vector3 offset;
    void Start()
    {
        snakeState = ladderState = createLadder = createSnake = false;
        offset = TouchableTilePrefab.transform.position - BoardWaypoints.Instance.waypoints[0];
        for (int i = 1; i < BoardWaypoints._NUM_BOXES; i++)
        {
            TouchableTiles[i] = GameObject.Instantiate(TouchableTilePrefab);
            TouchableTiles[i].GetComponent<ID_number>().ID = i;
            TouchableTiles[i].transform.position = BoardWaypoints.Instance.waypoints[i] + offset;
        }
    }

    void Update()
    {
        Vector3 mousepos;
        RaycastHit2D[] hit=new RaycastHit2D[10];
        Ray2D ray;
        ContactFilter2D cf=new ContactFilter2D();
        if(Input.GetMouseButtonDown(0))
        {
            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray = new Ray2D(mousepos, Vector2.zero);
            Physics2D.Raycast(ray.origin,ray.direction,cf,hit,100.0f);
            if(hit[0])
            {
                GetBox(hit[0].collider.gameObject.GetComponent<ID_number>().ID);
                //Debug.Log(hit[0].collider.gameObject.name + hit[0].collider.gameObject.GetComponent<ID_number>().ID+" has been hit");
            }
        }
    }

    void GetBox(int index)
    {
        
        Debug.Log("Caught box " + index);
        if (createLadder)
        {
            if (ladderState)
            {
                BoardWaypoints.Instance.ladders[(index<ladderStartIndex?index:ladderStartIndex)] = (index<ladderStartIndex?ladderStartIndex:index);
                ladderState = false;
            }
            else
            {
                ladderStartIndex = index;
                ladderState = true;
            }
        }
        if (createSnake)
        {
            if (snakeState)
            {
                BoardWaypoints.Instance.snakes[(index > snakeStartIndex ? index : snakeStartIndex)] = (index > snakeStartIndex ? snakeStartIndex : index);
                snakeState = false;
            }
            else
            {
                snakeStartIndex = index;
                snakeState = true;
            }
        }
    }

    void SetCreateLadder(bool val)
    {
        createLadder = val;
        ladderState = false;
        if (val)
            SetCreateSnake(false);
    }
    void SetCreateSnake(bool val)
    {
        createSnake = val;
        snakeState = false;
        if (val)
            SetCreateLadder(false);
    }
    private void Awake()
    {
        _instance = this;
    }

}
