using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowKart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerKart;
    private Vector3 playerX, playerY, playerZ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerX = playerKart.transform.right;  
        playerY = playerKart.transform.up;
        playerZ = playerKart.transform.forward;
        transform.position = playerKart.transform.position;               
        //transform.position += playerX * 1f;
        transform.position += playerY * 2f;
        transform.position += playerZ * -7f;
        transform.LookAt(playerKart.transform);
    }
}
