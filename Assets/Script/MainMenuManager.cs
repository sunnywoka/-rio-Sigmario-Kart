using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void OnPlay()
    {
        PlayerPrefs.SetInt("Money",0);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
