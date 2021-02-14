﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BigDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private String color;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Button"))
        {
            switch (color)
            {
                case "blue":
                    BigDoorOpen.blue = true;
                    Debug.Log("blue is now true");
                    break;
                case "black":
                    BigDoorOpen.black = true;
                    Debug.Log("black is now true");
                    break;
                case "purple":
                    BigDoorOpen.purple = true;
                    Debug.Log("purple is now true");
                    break;
                case "orange":
                    BigDoorOpen.orange = true;
                    Debug.Log("orange is now true");
                    break;
                case "green":
                    BigDoorOpen.green = true;
                    Debug.Log("green is now true");
                    break;
            }
        }
        
        Debug.Log("blue: " + BigDoorOpen.blue + 
        "black: " + BigDoorOpen.black + "purple: " + BigDoorOpen.purple + "orange: " + BigDoorOpen.orange + "green: " + BigDoorOpen.green);
        
    }
}
