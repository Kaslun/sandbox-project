using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using System.Threading;
using Cinemachine;

namespace Player
{
    public class FirstPersonController : MonoBehaviour
    {
        public Transform camPos;

        private PlayerController playerController;
        private Vector3 movement;
        private CharacterController characterController;

        private Camera cam;
        public float sensitivity = 100f;
        private float rotX;
        private float rotY;

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
            if (!isLoaded || !isEnabled) return;

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

            movement = ((transform.right * moveX) + (transform.forward * moveZ)) + Physics.gravity;

            if (movement.magnitude >= 0.1f)
            {
                characterController.Move(movement * playerController.controller.speed * Time.deltaTime);
            }
            else
                characterController.Move(Physics.gravity * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                float jumpSpeed = playerController.jumpForce * Time.deltaTime;
                movement.y = jumpSpeed;
            }
        }

        private void LookRotation()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

            rotX -= mouseY;
            rotY += mouseX;

            rotX = Mathf.Clamp(rotX, -90f, 90f);
            rotY = Mathf.Clamp(rotY, -60f, 60f);

            transform.Rotate(Vector3.up * mouseX);
            cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, rotY);

        }

        private void Enable()
        {
            isEnabled = !isEnabled;
        }
    }
}
