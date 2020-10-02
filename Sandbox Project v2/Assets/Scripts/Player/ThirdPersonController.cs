using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Manager;

namespace Player
{
    public class ThirdPersonController : MonoBehaviour
    {
        private PlayerController playerController;
        private CharacterController characterController;
        private Camera cam;
        private Vector3 movement;
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;

        public bool isEnabled;

        private void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
            EventManager.StartListening("SwitchMode", SwitchController);
        }

        private void Loaded()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<PlayerController>();

            CameraManager camManager = playerController.cameraManager;
            cam = camManager.cameras[1];
        }

        private void Update()
        {
            cam.enabled = isEnabled;
            if (isEnabled) MovePlayer();
        }

        private void MovePlayer()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            movement = new Vector3(moveX, 0f, moveZ).normalized;

            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move((moveDir + Physics.gravity) * playerController.controller.speed * Time.deltaTime);
            }
            else
                characterController.Move(Physics.gravity);

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                float jumpSpeed = playerController.jumpForce * Time.deltaTime;
                movement.y = jumpSpeed;
            }
        }

        private void SwitchController()
        {
            isEnabled = !isEnabled;
        }
    }
}