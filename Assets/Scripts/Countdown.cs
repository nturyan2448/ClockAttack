using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public List<float> countdownTime;
    TextMeshPro tmp;
    float startTime;
    bool is_running;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        is_running = false;
        countdownTime = new List<float> { 10f, 10f, 10f };
    }

    // Update is called once per frame
    void Update()
    {
        int i = GameController.instance.round;
        //Debug.LogWarning("Round for countdowns " + i.ToString());
        if (is_running) {
            float timeLeft;
            if (Time.timeScale == 0)
                timeLeft = countdownTime[i] - (Time.unscaledTime - startTime);
            else timeLeft = countdownTime[i] - (Time.time - startTime);
            if (timeLeft <= 0) {
                StopCountdown();
                GameController.instance.NextRound();
            }
            else tmp.text = Mathf.CeilToInt(timeLeft).ToString();
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void StartCountdown(bool shouldUseNormalTime = false) {
        is_running = true;
        if (Time.timeScale == 0 && !shouldUseNormalTime)
            startTime = Time.unscaledTime;
        else startTime = Time.time;
        gameObject.SetActive(true);
    }

    public void StopCountdown() {
        is_running = false;
        //gameObject.SetActive(false);
    }

    public void IncreaseCountdown(int playerID, float newTime) {
        countdownTime[playerID] += newTime;
    }
}
