using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public GameObject finishLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Finish();
    }
    void Finish()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 500);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
