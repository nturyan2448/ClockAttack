using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartButton : MonoBehaviour
{
    public void StartGame () {
        SceneManager.LoadScene("MainScene");
    }
}
