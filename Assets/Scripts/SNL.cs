using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNL : MonoBehaviour
{
    public void SetDelete()
    {
        TouchBoxController.Instance.SetState(TouchBoxController.State.DELETE);
    }
    public void SetLadder()
    {
        TouchBoxController.Instance.SetState(TouchBoxController.State.LADDER);
    }
    public void SetSnake()
    {
        TouchBoxController.Instance.SetState(TouchBoxController.State.SNAKE);
    }
}
