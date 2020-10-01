using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using System.Security.Cryptography;

public class PlayerManager : MonoBehaviour
{
    public int currentMode;
    public int prevMode;
    public List<Camera> cameras;

    public Controller playerController;
    public CharacterController characterController;

    public Camera currentCam;
    public RawImage interactionIndicator;
    private Vector3 movement;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public float jumpForce = 0;

    private void Start()
    {
        EventManager.StartListening("Loaded", Loaded);
    }

    private void Loaded()
    {
        interactionIndicator = FindObjectOfType<RawImage>();
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            cameras.Add(cam);
        }
        UpdateCamera(0);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        interactionIndicator.enabled = CanInteract();

        if (Input.GetButtonDown("Interact") && CanInteract())
        {
            Interact();
        }

        if(Input.GetButtonDown("Switch Camera"))
        {
            currentMode = (currentMode + 1) % cameras.Count;
            UpdateCamera(currentMode);
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + currentCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move((moveDir + Physics.gravity) * playerController.speed * Time.deltaTime);
        }
        else
            characterController.Move(Physics.gravity);

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            float jumpSpeed = jumpForce * Time.deltaTime;
            movement.y = jumpSpeed;
        }
    }

    private void Interact()
    {
        Debug.Log("Interacted with object");
    }

    private void UpdateCamera(int nextMode)
    {
        prevMode = currentMode;
        currentMode = nextMode;

        cameras[prevMode].enabled = false;
        cameras[currentMode].enabled = true;

        Debug.Log("Switched camera to: " + cameras[currentMode].name);
    }

    private bool CanInteract()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, currentCam.transform.forward, out hit))
        {
            if (hit.transform.tag == "Interactable")
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
