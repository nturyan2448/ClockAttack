using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMinuteArm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, System.DateTime.Now.Minute * (-6));
    }
}
