using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchBoxController : MonoBehaviour
{
    public enum State
    {
        SNAKE=0,
        LADDER=1,
        DELETE=2,
        NONE=3
    };
    public State state;
    private static TouchBoxController _instance;
    public static TouchBoxController Instance
    {
        get;
        private set;
    }
    Color selectTint = new Color(1, 0.39f, 0.39f, 0.58f);
    public Transform SnakeHolder, LadderHolder;
    public GameObject[] TouchableTiles = new GameObject[BoardWaypoints._NUM_BOXES];
    public GameObject TouchableTilePrefab, SnakePrefab, LadderPrefab;
    private Vector3 offset;
    public int[] catchClick = new int[2];
    private int _clickNo;
    public int clickNo
    {
        get
        {
            return _clickNo;
        }
        set
        {
            if (value % 2 == 0 && _clickNo == 1)
                ClearTileTint(catchClick[0]);
            if (value % 2 == 1 && _clickNo == 1)
                SetTileTint(catchClick[0]);
            _clickNo = value % 2;
        }
    }

    void Start()
    {
        state = State.NONE;
        clickNo = 0;
        TouchableTiles[0] = TouchableTilePrefab;
        offset = TouchableTilePrefab.transform.position - BoardWaypoints.Instance.waypoints[0];
        for (int i = 1; i < BoardWaypoints._NUM_BOXES; i++)
        {
            TouchableTiles[i] =Instantiate(TouchableTilePrefab,transform);
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
        if (state == State.LADDER && index == 0) return;//no ladder from the starting point
        if (state == State.SNAKE && index == BoardWaypoints._NUM_BOXES - 1) return;//no snake to the winning position
        if(state==State.DELETE)
        {
            DelLink(index);
            return;
        }
        clickNo++;
        catchClick[clickNo] = index;
        if (clickNo == 0)
            Create_snl();
    }

    void Create_snl()
    {
        if (catchClick[0] == catchClick[1]) return;
        int consecutive = 0;
        int up, low;
        up = (catchClick[0] > catchClick[1] ? catchClick[0] : catchClick[1]);
        low = (catchClick[0] > catchClick[1] ? catchClick[1] : catchClick[0]);

        if(state==State.LADDER)
        {
            if (low == 0) return;
            consecutive = 0;
            for(int i=(low-5<0?0:low-5);i<(low+5>BoardWaypoints._NUM_BOXES-1?BoardWaypoints._NUM_BOXES-1:low+5);i++)
            {
                if (BoardWaypoints.Instance.snl[i] != -1||i==low)
                    consecutive++;
                else
                    consecutive = 0;
            }
            if (consecutive >= 5) return;

            if(BoardWaypoints.Instance.snl[low]!=-1)
            {
                DelLink(low);
            }

            BoardWaypoints.Instance.snl[low] = up;

            BoardWaypoints.Instance.sprites[low] = 
                DrawLadder(BoardWaypoints.Instance.waypoints[low], BoardWaypoints.Instance.waypoints[up]);
        }

        if (state == State.SNAKE)
        {
            if (up == BoardWaypoints._NUM_BOXES-1) return;
            consecutive = 0;
            for (int i = (up - 5 < 0 ? 0 : up - 5); i < (up + 5 > BoardWaypoints._NUM_BOXES - 1 ? BoardWaypoints._NUM_BOXES - 1 : low + 5); i++)
            {
                if (BoardWaypoints.Instance.snl[i] != -1||i==up)
                    consecutive++;
                else
                    consecutive = 0;
            }
            if (consecutive >= 6) return;

            if (BoardWaypoints.Instance.snl[up] != -1)
            {
                DelLink(up);
            }

            BoardWaypoints.Instance.snl[up] = low;

            BoardWaypoints.Instance.sprites[up] =
                DrawSnake(BoardWaypoints.Instance.waypoints[up], BoardWaypoints.Instance.waypoints[low]);
        }
    }

    void DelLink(int index)
    {
        if(BoardWaypoints.Instance.snl[index]!=-1)
        {
            BoardWaypoints.Instance.snl[index] = -1;
            Destroy(BoardWaypoints.Instance.sprites[index]);
            BoardWaypoints.Instance.sprites[index] = null;
        }
        clickNo = 0;
    }
    
    public void SetState(State s)
    { 
        clickNo = 0;
        state = s;
    }

    GameObject DrawSnake(Vector3 init,Vector3 end)
    {
        Vector3 center = (init + end) / 2;
        GameObject snake = GameObject.Instantiate(SnakePrefab,SnakeHolder);
        snake.transform.position = center;
        snake.transform.localScale *= 0.11f * Vector3.Distance(init, end)/BoardWaypoints.Instance.diff.x;
        snake.transform.Rotate(0f, 0f, Vector3.Angle(Vector3.right,(end-init)));
        return snake;
    }

    GameObject DrawLadder(Vector3 init, Vector3 end)
    {
        Vector3 center = (init + end) / 2;
        GameObject ladder = GameObject.Instantiate(LadderPrefab,LadderHolder);
        ladder.transform.position = center;
        ladder.transform.localScale *= 0.11f * Vector3.Distance(init, end) / BoardWaypoints.Instance.diff.x;
        ladder.transform.Rotate(0f, 0f, Vector3.Angle(Vector3.right, (end - init)));
        return ladder;   
    }    

    void SetTileTint(int index)
    {
        TouchableTiles[index].GetComponent<SpriteRenderer>().color = selectTint;
    }

    void ClearTileTint(int index)
    {
        TouchableTiles[index].GetComponent<SpriteRenderer>().color = Color.clear;
    }

    private void Awake()
    {
        _instance = this;
    }

}
