using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBouncing : MonoBehaviour
{
    float timer;
    public Vector2 startPosition;
    public Vector2 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timer %= 1;
        transform.localPosition = Vector2.Lerp(startPosition, endPosition, Mathf.Abs(Mathf.Cos(timer * Mathf.PI)));
    }
}
