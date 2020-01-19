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

    public RectTransform[] boxPositions=new RectTransform[100];
    public RectTransform Box1;
    void Start()
    {
        boxPositions[0] = Box1;

    }


    private void Awake()
    {
        _instance = this;
    }

}
