using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    public List<CinemachineVirtualCamera> vCams;

    private int prevState;
    private int currentState;

    private void Start()
    {
        EventManager.StartListening("SwitchMode", UpdateCamera);
        EventManager.StartListening("Loaded", Loaded);
    }

    private void Loaded()
    {
        foreach(CinemachineVirtualCamera c in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            vCams.Add(c);   
        }
    }

    public void UpdateCamera()
    {
        prevState = currentState;

        currentState = (currentState + 1) % vCams.Count;

        vCams[prevState].enabled = false;
        vCams[currentState].enabled = true;

        Debug.Log("Switched camera to: " + vCams[currentState].name);
    }
}
