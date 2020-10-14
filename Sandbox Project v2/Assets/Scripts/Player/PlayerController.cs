using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Controller controller;
        private CharacterController characterController;

        private Camera cam;

        private RaycastHit hit;

        public float jumpForce;
        private Vector3 movement;

        public float sensitivity = 100f;
        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;
        private float rotX;
        private float rotY;

        private bool firstPerson;
        private bool isLoaded = false;

        void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
        }

        private void Loaded()
        {
            cam = Camera.main;
            characterController = GetComponent<CharacterController>();
            isLoaded = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLoaded) return;

            if (firstPerson)
                MoveFPS();
            else
                MoveTPS();

            LookRotation();

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
                movement.y = jumpForce * Time.deltaTime;

            if (Input.GetButtonDown("Interact") && CanInteract())
                Interact();

            if (Input.GetButtonDown("Switch Camera")) {
                EventManager.TriggerEvent("SwitchMode");
                firstPerson = !firstPerson;
            }
        }

        private void MoveFPS()
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
                characterController.Move((moveDir + Physics.gravity) * controller.speed * Time.deltaTime);
            }
            else
                characterController.Move(Physics.gravity);
        }

        private void MoveTPS()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            movement = ((transform.right * moveX) + (transform.forward * moveZ)) + Physics.gravity;

            if (movement.magnitude >= 0.1f)
            {
                characterController.Move(movement * controller.speed * Time.deltaTime);
            }
            else
                characterController.Move(Physics.gravity * Time.deltaTime);
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
            //cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, 0f);
            cam.transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, rotY);

        }

        private void Interact()
        {
            hit.transform.GetComponent<Interactable>().Interact();
        }

        private bool CanInteract()
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, cam.transform.forward, out hit))
            {
                if (hit.transform.CompareTag("Interactable"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
