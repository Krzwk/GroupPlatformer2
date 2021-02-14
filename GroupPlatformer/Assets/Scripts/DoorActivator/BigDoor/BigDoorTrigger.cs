using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BigDoorTrigger : MonoBehaviour
{
    private String color;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Button"))
        {
            switch (color)
            {
                case "blue":
                    BigDoorOpen.blue = true;
                    break;
                case "black":
                    BigDoorOpen.black = true;
                    break;
                case "purple":
                    BigDoorOpen.purple = true;
                    break;
                case "orange":
                    BigDoorOpen.orange = true;
                    break;
                case "green":
                    BigDoorOpen.green = true;
                    break;
            }
        }
    }
}
