using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoorOpen : MonoBehaviour
{
    private bool green;     // 1. button to be pressed
    private bool blue;      // 2. button to be pressed 
    private bool black;     // 3. button to be pressed
    private bool purple;    // 4. button to be pressed
    private bool orange;    // 5. button to be pressed

    private void Update()
    {
        if (checkIfColorSequenceIsOk)
        {
            // 
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
