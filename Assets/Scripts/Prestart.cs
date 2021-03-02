using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prestart : MonoBehaviour
{
    public float timerStart = 4.0f; // 3, 2, 1, Start
    public float maxFontSize = 100f;
    public float minFontSize = 40f;
    public TextMeshPro countdownText;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                GameController.instance.StartGame();
                gameObject.SetActive(false);
            }
            else {
                int count = Mathf.FloorToInt(timer);
                if (count >= 1)
                    countdownText.text = count.ToString();
                else countdownText.text = "Start";
                countdownText.fontSize = GetFontSize(timer - count);
            }
        }
    }

    public void PreStart() {
        timer = timerStart;
    }

    public float GetFontSize(float f) {
        // 0 <= f < 1
        if (f < 0 || f >= 1) Debug.LogError("Error in GetFontSize");

        if (f >= 0.5f)
            return Mathf.Lerp(minFontSize, maxFontSize, (f - 0.5f) * 2);
        else return minFontSize;
    }
}
