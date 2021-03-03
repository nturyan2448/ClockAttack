using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourArm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.state == "playing") {
            float currentTime;
            if (GameController.instance.clockStopped) {
                currentTime = GameController.instance.tempCurrentTime;
            }
            else currentTime = Time.time - GameController.instance.startTime;
            transform.localRotation = Quaternion.Euler(0, 0, -(currentTime % 720) / 2);
        }
        else {
            transform.localRotation = Quaternion.identity;
        }

    }
}
