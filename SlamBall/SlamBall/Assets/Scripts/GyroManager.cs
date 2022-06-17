using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroManager : MonoBehaviour
{
    public Text gyroEnabled, gyroRot;


    private Gyroscope gyro;
    private Vector3 rot;
    private bool gyroActive = false;


    void Start()
    {
        EnableGyro();
    }

    private void Update()
    {
        if(gyroActive)
        {
            rot = gyro.rotationRate;
            print(rot);
        }

        gyroEnabled.text = $"Gyro enabled: {gyroActive}";
        gyroRot.text = $"Gyro rot: {rot}";
    }

    private void EnableGyro()
    {
        if (gyroActive)
            return;

        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
        }

        print($"Gyro {gyroActive}");
    }
    
}
