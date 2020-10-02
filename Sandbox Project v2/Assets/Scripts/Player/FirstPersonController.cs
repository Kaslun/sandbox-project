using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using System.Threading;

namespace Player
{
    public class FirstPersonController : MonoBehaviour
    {
        public Transform fpsCamPos;

        private PlayerController playerController;
        private Vector3 movement;
        private CharacterController characterController;

        private Camera cam;
        public float sensitivity = 100f;
        private float rotX;
        private float rotY;

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
            cam = camManager.cameras[0];
  
        }

        private void Update()
        {
            cam.transform.position = fpsCamPos.position;
            cam.enabled = isEnabled;
            if (isEnabled)
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

            rotX = Mathf.Clamp(rotX, -90f, 90f);

            transform.Rotate(Vector3.up * mouseX);
            cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, 0f);

        }

        private void SwitchController()
        {
            isEnabled = !isEnabled;
        }
    }
}
