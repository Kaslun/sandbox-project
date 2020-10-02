using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Cinemachine;
using Player;

public class CameraManager : MonoBehaviour
{
    public List<Camera> cameras;
    public Camera currentCam;
    public CinemachineFreeLook cineCam;
    private int prevState;
    private int currentState;

    private void Start()
    {
        EventManager.StartListening("Loaded", Loaded);
        EventManager.StartListening("SwitchMode", UpdateCamera);
    }

    private void Loaded()
    {
        SpawnCamera(FindObjectOfType<ThirdPersonController>().transform);
    }

    public void UpdateCamera()
    {
        prevState = currentState;

        currentState = (currentState + 1) % cameras.Count;

        cameras[prevState].enabled = false;
        cameras[currentState].enabled = true;

        Debug.Log("Switched camera to: " + cameras[currentState].name);

        /*currentMode = (currentMode + 1) % cameras.Count;
        cameraManager.UpdateCamera(currentMode);*/
    }

    private void SpawnCamera(Transform target)
    {
        cineCam.Follow = target;
        cineCam.LookAt = target;
        UpdateCamera();
    }
}
