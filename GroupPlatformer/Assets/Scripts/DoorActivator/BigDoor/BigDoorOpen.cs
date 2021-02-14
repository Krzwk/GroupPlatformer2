using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BigDoorOpen : MonoBehaviour
{
    public static bool green; // 1. button to be pressed
    public static bool blue; // 2. button to be pressed 
    public static bool black; // 3. button to be pressed
    public static bool purple; // 4. button to be pressed
    public static bool orange; // 5. button to be pressed

    // Pressing order: green -> blue -> black -> purple -> orange

    private bool[] colorSequence = {green, blue, black, purple, orange};

    [SerializeField]
    private Transform bigDoor;

    private float speed = 0.25f;
    
    private void Update()
    {

        if (!checkCurrentSequence())
        {
            resetSequence();
        }
        
        if (green && blue && black && purple && orange)
        {
            // open big door
            if (bigDoor.position.y <= 10.0f)
                bigDoor.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
    }

    // checks whether the current sequence could possibly lead to a valid state
    // (green, blue, !black, !purple, !orange) --> should return true
    // (!green, blue, !black, !purple, !orange) --> should return false
    
    bool checkCurrentSequence()
    {
        if ((green && !blue && !black && !purple && !orange) ||
            (green && blue && !black && !purple && !orange) ||
            (green && blue && black && !purple && !orange) ||
            (green && blue && black && purple && !orange) ||
            (green && blue && black && purple && orange))
        {
            return true;
        }
        else
        {
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
