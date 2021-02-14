using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigDoorOpen : MonoBehaviour
{
    public static bool green;     // 1. button to be pressed
    public static bool blue;      // 2. button to be pressed 
    public static bool black;     // 3. button to be pressed
    public static bool purple;    // 4. button to be pressed
    public static bool orange;    // 5. button to be pressed
    
    // Pressing order: green -> blue -> black -> purple -> orange

    [SerializeField]
    private Transform bigDoor;

    private void Update()
    {
        if (checkIfColorSequenceIsOk())
        {
            // open big door
            if (bigDoor.position.y <= 11.0f)
                bigDoor.Translate(Vector3.up * Time.deltaTime, Space.World);
        }
    }

    bool checkIfColorSequenceIsOk()
    {
        // start from the last button that should be pressed and check if other buttons have been pressed
        if (orange) {
            if (purple) {
                if (black) {
                    if (blue) {
                        if(green) {
                            // open the big door
                            Debug.Log("Big door opens!");
                            return true;
                        } else
                        {
                            resetSequence();
                            return false;
                        }
                    } else {
                        resetSequence();
                        return false;
                    }
                } else {
                    resetSequence();
                    return false;
                }
            } else {
                resetSequence();
                return false;
            }
        } else {
            resetSequence();
            return false;
        }
    }

    void resetSequence()
    {
        green = false;
        blue = false;
        black = false;
        purple = false;
        orange = false;
    }
    
}
