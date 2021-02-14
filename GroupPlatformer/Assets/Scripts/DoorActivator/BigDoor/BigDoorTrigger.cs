using System;
using System.Collections.Generic;
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
        
        Debug.Log("blue: " + BigDoorOpen.blue + "\n" +
        "black: " + BigDoorOpen.black + "\n" + "purple: " + BigDoorOpen.purple  + "\n" + 
        "orange: " + BigDoorOpen.orange + "\n" + "green: " + BigDoorOpen.green);
        
    }
}
