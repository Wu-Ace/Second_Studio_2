using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePhone : MonoBehaviour
{
    private float old_y = 0;
    private float new_y;
    private float currentDistance = 0;

    public float distance = 1;

    void Update () {
        new_y           = Input.acceleration.y;
        currentDistance = new_y - old_y;
        old_y           = new_y;

        if (currentDistance > distance&&Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            Handheld.Vibrate();
        }
    }
}