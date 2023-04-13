using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public Text textSpeed;
    public Text textTime;
    float timeSpent = 0f;
    int hour, minute, second, millisecond;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeSpent += Time.deltaTime;
        hour = (int)timeSpent / 3600;
        minute = ((int)timeSpent - hour * 3600) / 60;
        second = (int)timeSpent - hour * 3600 - minute * 60;
        millisecond = (int)((timeSpent - (int)timeSpent) * 1000);
        textTime.text = string.Format("{0:D2}:{1:D2}.{2:D3}", minute, second, millisecond);

        textSpeed.text = player.GetComponent<Rigidbody>().velocity.magnitude.ToString("f1");
    }
}
