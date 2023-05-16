using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButtonListener : MonoBehaviour
{
    GameObject chestOpen, chestClose, player;
    bool isClose;
    public float maxDist = 3;
    
    private void Awake()
    {
        isClose = true;

        chestOpen = GameObject.Find("chest_open");
        chestClose = GameObject.Find("chest_close");

        player = GameObject.FindGameObjectWithTag("Player");

        chestOpen.SetActive(false);
        chestClose.SetActive(true);
        
        Debug.Assert(chestOpen != null && chestClose != null && player != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("e arrow key is held down");
            float dist = Vector3.Distance(chestClose.transform.position, player.transform.position);
            Debug.Log("dist: " + dist);
            
            bool isCloseEnough = dist < maxDist;
            Debug.Log("isCloseEnough: " + isCloseEnough);
            if (isCloseEnough)
            {
                chestOpen.SetActive(isClose);
                chestClose.SetActive(!isClose);
                isClose = !isClose;
            }

            
        }
    }
}
