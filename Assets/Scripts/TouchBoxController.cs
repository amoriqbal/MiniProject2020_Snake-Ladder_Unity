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
    
    public GameObject[] TouchableTiles = new GameObject[BoardWaypoints._NUM_BOXES];
    public GameObject TouchableTilePrefab, SnakePrefab, LadderPrefab;

    public float snakeSizeRatio, ladderSizeRatio;
    bool snakeState, ladderState, createLadder, createSnake;
    int ladderStartIndex, snakeStartIndex;
    private Vector3 offset;
    void Start()
    {
        snakeSizeRatio = 880f / Screen.height;
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
            CreateLadder(index);
        }
        if (createSnake)
        {
            CreateSnake(index);
        }
    }

    void CreateSnake(int index)
    {
        if (snakeState)
        {
            int begin, end;
            end = (index > snakeStartIndex ? index : snakeStartIndex);
            begin = (index > snakeStartIndex ? snakeStartIndex : index);
            BoardWaypoints.Instance.snakes[end] = begin;
            DrawSnake(BoardWaypoints.Instance.waypoints[begin], BoardWaypoints.Instance.waypoints[end]);
            snakeState = false;
        }
        else
        {
            snakeStartIndex = index;
            snakeState = true;
        }
    }

    void CreateLadder(int index)
    {
        if (ladderState)
        {
            int begin, end;
            end = (index > ladderStartIndex ? index : ladderStartIndex);
            begin = (index > ladderStartIndex ? ladderStartIndex : index);
            BoardWaypoints.Instance.ladders[end] = begin;
            DrawLadder(BoardWaypoints.Instance.waypoints[begin], BoardWaypoints.Instance.waypoints[end]);
            ladderState = false;
        }
        else
        {
            ladderStartIndex = index;
            ladderState = true;
        }
    }

    void ToggleCreateLadder()
    {
        SetCreateLadder(!createLadder);
    }

    void ToggleCreateSnake()
    {
        SetCreateSnake(!createSnake);
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

    void DrawSnake(Vector3 init,Vector3 end)
    {
        Vector3 center = (init + end) / 2;
        GameObject snake = GameObject.Instantiate(SnakePrefab);
        snake.transform.position = center;
        snake.transform.localScale *= 0.11f * Vector3.Distance(init, end)/BoardWaypoints.Instance.diff.x;
        snake.transform.Rotate(0f, 0f, Vector3.Angle(Vector3.right,(end-init)));
    }
    void DrawLadder(Vector3 init, Vector3 end)
    {
        Vector3 center = (init + end) / 2;
        GameObject ladder = GameObject.Instantiate(LadderPrefab);
        ladder.transform.position = center;
        ladder.transform.localScale *= 0.11f * Vector3.Distance(init, end) / BoardWaypoints.Instance.diff.x;
        ladder.transform.Rotate(0f, 0f, Vector3.Angle(Vector3.right, (end - init)));
    }
    private void Awake()
    {
        _instance = this;
    }

}
