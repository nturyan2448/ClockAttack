using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameController.instance.round) {
        case 0:
            transform.rotation = Quaternion.identity;
            break;
        case 1:
            transform.rotation = Quaternion.Euler(0, 0, -90);
            break;
        case 2:
            transform.rotation = Quaternion.Euler(0, 0, 90);
            break;
        }
    }
}
