using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;

    public GameObject goal;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(level1.transform.position, player.transform.position) < 5)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
        }

        if (Vector3.Distance(level2.transform.position, player.transform.position) < 5)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(3, LoadSceneMode.Single);
            }
        }

        if (Vector3.Distance(level3.transform.position, player.transform.position) < 5)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(4, LoadSceneMode.Single);
            }
        }

        if (Vector3.Distance(level4.transform.position, player.transform.position) < 5)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(5, LoadSceneMode.Single);
            }
        }

        if (Vector3.Distance(goal.transform.position, player.transform.position) < 5)
        {
            if ((Input.GetKeyDown(KeyCode.E)) && (PlayerPrefs.GetInt("Money") >= 2000))
            {
                SceneManager.LoadScene(6, LoadSceneMode.Single);
            }
        }
    }
}
