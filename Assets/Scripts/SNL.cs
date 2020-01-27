using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNL : MonoBehaviour
{
    public TouchBoxController tbc;
    public void SetDelete()
    {
        tbc.SetState(TouchBoxController.State.DELETE);
    }
    public void SetLadder()
    {
        tbc.SetState(TouchBoxController.State.LADDER);
    }
    public void SetSnake()
    {
        tbc.SetState(TouchBoxController.State.SNAKE);
    }
}
