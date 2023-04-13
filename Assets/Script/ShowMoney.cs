using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMoney : MonoBehaviour
{
    public Text textMoney;
    // Start is called before the first frame update
    void Start()
    {
        textMoney.text = "Money: " + PlayerPrefs.GetInt("Money").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
