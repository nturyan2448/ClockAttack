using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalClock : MonoBehaviour
{
    TextMeshPro TMP;
    public bool realWorldTime = false;
    // Start is called before the first frame update
    void Start()
    {
        TMP = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (realWorldTime)
            TMP.text = string.Format("{0:00}:{1:00}", System.DateTime.Now.Hour % 12, System.DateTime.Now.Minute);
        else TMP.text = string.Format("{0:00}:{1:00}", GameController.instance.currentHour, GameController.instance.currentMinute);
    }
}
