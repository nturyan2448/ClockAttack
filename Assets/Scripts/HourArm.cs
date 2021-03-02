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
            float currentTime = Time.time;
            transform.localRotation = Quaternion.Euler(0, 0, -((currentTime - GameController.instance.startTime) % 720) / 2);
        }
        else {
            transform.localRotation = Quaternion.identity;
        }

    }
}
