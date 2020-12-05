using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TobiiConnected : MonoBehaviour
{
    public GameObject Icon;

    // Update is called once per frame
    void Update()
    {
        if(TobiiAPI.IsConnected)
        {
            Icon.SetActive(true);
        }
        else
        {
            Icon.SetActive(false);
        }
    }
}
