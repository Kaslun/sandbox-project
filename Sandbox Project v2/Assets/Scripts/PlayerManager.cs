using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class PlayerManager : MonoBehaviour
{
    public Controller playerController;
    public CharacterController characterController;

    public Camera mainCam;
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
        mainCam = Camera.main;
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
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
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

    private bool CanInteract()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, mainCam.transform.forward, out hit))
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
