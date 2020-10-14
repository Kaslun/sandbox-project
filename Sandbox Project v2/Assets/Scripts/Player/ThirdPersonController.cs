using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Manager;
using Cinemachine;

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
        private bool isLoaded = false;

        private void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
            EventManager.StartListening("SwitchMode", Enable);
        }

        private void Loaded()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<PlayerController>();

            cam = Camera.main;

            isLoaded = true;
        }

        private void Update()
        {
            if (!isLoaded || !isEnabled)
                return;

            if (cam.enabled)
            {
                MovePlayer();
                LookRotation();
            }
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


        public float sensitivity = 100f;
        private float rotX;
        private float rotY;

        private void LookRotation()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

            rotX -= mouseY;
            rotY += mouseX;

            rotX = Mathf.Clamp(rotX, -90f, 90f);
            rotY = Mathf.Clamp(rotY, -60f, 60f);

            transform.Rotate(Vector3.up * mouseX);
            //cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, 0f);
            cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, rotY);

        }

        private void Enable()
        {
            isEnabled = !isEnabled;
        }
    }
}