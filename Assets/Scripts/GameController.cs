using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float startTime;
    public float currentHour = 0;
    public float currentMinute = 0;
    public string state = "idle";
    public int round = 0;
    public GameObject players;
    public GameObject winText;
    public Prestart prestart;
    public float speedUpScale = 0.5f;
    public float maxSpeedScale = 3f;
    public AudioClip winAudio;
    public AudioClip loseAudio;

    float tempTimeScale;
    int callStopID;

    private void Awake () {
        //If we don't currently have a game control...
        if (instance == null) {
            //...set this one to be it...
            instance = this;
        }
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    private void Start () {
        SetWinTime();
        GetComponent<Deck>().Deal();
        prestart.PreStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "playing") {
            float currentTime = Time.time - startTime;
            currentHour = Mathf.FloorToInt(currentTime / 60);
            if (currentHour >= 12) {
                startTime += 12 * 60;
                currentHour -= 12;
            }
            else if (currentHour < 0) {
                startTime -= 12 * 60;
                currentHour += 12;
            }
            currentMinute = Mathf.FloorToInt(currentTime % 60);
        }
    }

    public void StartGame() {
        startTime = Time.time;
        state = "playing";
        players.transform.GetChild(0).GetComponent<Player>().countdown.StartCountdown();
    }

    public void AddTime(float t, string scale) {
        if (scale == "minute")
            startTime -= t;
        else if (scale == "hour")
            startTime -= t * 60;
        else {
            Debug.LogWarning("Add time got a wrong scale");
        }
    }

    public void SubtractTime(float t, string scale) {
        if (scale == "minute")
            startTime += t;
        else if (scale == "hour")
            startTime += t * 60;
        else {
            Debug.LogWarning("Subtract time got a wrong scale");
        }
    }

    public void SetTime(int hour, int minute) {
        startTime = Time.time - hour * 60 - minute;
    }

    public void SpeedUp() {
        if (Time.timeScale == 0) {
            if (tempTimeScale < maxSpeedScale)
                tempTimeScale += speedUpScale;
        }
        else if (Time.timeScale < maxSpeedScale) {
            Time.timeScale += speedUpScale;
        }
    }

    public void StopClock(int callerID) {
        if (Time.timeScale != 0)
            tempTimeScale = Time.timeScale;
        Time.timeScale = 0;
        callStopID = callerID;
    }

    public void RestartClock() {
        if (round == callStopID)
            Time.timeScale = tempTimeScale;
    }


    public void NextRound(int num_rounds = 1) {
        players.transform.GetChild(round).GetComponent<Player>().countdown.StopCountdown();
        round = (round + num_rounds) % players.transform.childCount;
        if (Time.timeScale == 0 && round == callStopID)
            players.transform.GetChild(round).GetComponent<Player>().countdown.StartCountdown(true);
        else players.transform.GetChild(round).GetComponent<Player>().countdown.StartCountdown();
        Debug.Log("Current Round: " + round.ToString());
    }

    public void SkipRound() {
        // Next round but not starting the next player's countdown
        players.transform.GetChild(round).GetComponent<Player>().countdown.StopCountdown();
        round = (round + 1) % players.transform.childCount;
        Debug.Log("Current Round: " + round.ToString());
    }

    private void SetWinTime() {
        for (int i = 0; i < players.transform.childCount; i++) {
            players.transform.GetChild(i).GetComponent<Player>().SetWinTime(Random.Range(1, 12),Random.Range(0, 60));
        }
    }

    public void EndGame(int winnerID) {
        Debug.Log("WinnerID: " + winnerID.ToString());
        winText.SetActive(true);
        Image image = winText.GetComponent<Image>();
        TextMeshProUGUI text = winText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (text == null) {
            Debug.Log("Didn't find the text");
            Debug.Log(winText.ToString());
            Debug.Log(winText.transform.GetChild(0).ToString());
            Debug.Log(winText.transform.GetChild(0).GetComponent<TextMeshPro>());
        }
        if (winnerID == 0) {
            image.color = new Color32(0xDB, 0x4F, 0x5F, 0xFF);
            text.text = "You Win!";
            AudioSource.PlayClipAtPoint(winAudio, Vector3.zero);
        }
        else {
            image.color = new Color32(0x61, 0x86, 0xA9, 0xFF);
            text.text = string.Format("AI {0} Win!", winnerID);
            AudioSource.PlayClipAtPoint(loseAudio, Vector3.zero);
        }
        state = "end";
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }
}
