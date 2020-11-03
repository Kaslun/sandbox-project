using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using System.Security.Cryptography;
using System.Threading;
using Cinemachine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Dev Tools")]
        public Controller controller;
        private CharacterController characterController;
        private PlayerCamera playerCamera;
        private GameManager gameManager;
        private CheckpointManager checkpointManager;
        public bool debugMode;

        [Header("IK Controls")]
        public Transform lookTarget;
        public Transform lookTargetOrigin;
        public Transform head;
        public Animator anim;

        [SerializeField]
        private List<Transform> intObjects;

        private Transform currentCam;

        [Header("Settings")]
        public float jumpForce;
        private Vector3 moveDir;

        [Range(1, 2)]
        public float speedModifier;
        [Range(0, 10)]
        public float xCamSensitivity;
        [Range(0, 10)]
        public float yCamSensitivity;
        [Range(0, 10)]
        public float camRotSensitivity;
        [Range(0, 1)]
        public float rotSensitivity;

        private float targetAngle;

        private bool firstPerson = true;
        private bool isLoaded = false;

        void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
        }

        private void Loaded()
        {
            playerCamera = GetComponent<PlayerCamera>();
            gameManager = FindObjectOfType<GameManager>();
            checkpointManager = FindObjectOfType<CheckpointManager>();
            characterController = GetComponent<CharacterController>();
            isLoaded = true;
        }

        void Update()
        {
            if (!isLoaded) return;

            Move();
            Rotate();
            LookAtObject();

            if (Input.GetButtonDown("Interact"))
                Interact();

            if (Input.GetButtonDown("Switch Camera"))
            {
                EventManager.TriggerEvent("SwitchMode");
                firstPerson = !firstPerson;
            }

            //Debug Tools for testing features in PlayerController
            if (!debugMode) return;

            if (Input.GetButtonDown("Respawn"))
            {
                Respawn();
            }
        }

        private void Move()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

            anim.SetFloat("Speed", movement.magnitude);

            if (movement.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * currentCam.forward;

                float speed = controller.speed;

                if (Input.GetButton("Run"))
                {
                    anim.SetFloat("Speed", movement.magnitude * 2);
                    speed = speed * speedModifier;
                }

                characterController.Move((moveDir + Physics.gravity) * speed * Time.deltaTime);
            }
        }
        private void Rotate()
        {
            currentCam = playerCamera.vCams[playerCamera.currentState].transform;

            float mouseX = Input.GetAxisRaw("Mouse X") * (xCamSensitivity * 10) * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * (yCamSensitivity * 10) * Time.deltaTime;

            Vector3 rotY = Vector3.up * mouseX;
            Vector3 rotX = Vector3.right * -mouseY;

            //First person camera- and player rotation-controls
            if (firstPerson)
            {
                currentCam.Rotate(rotX);
                transform.Rotate(rotY);
            }

            //Third person camera- and player rotation-controls
            else
            {
                Quaternion playerTargetRot = Quaternion.LookRotation(moveDir + transform.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRot, Time.deltaTime * rotSensitivity * 10);
                currentCam.rotation = Quaternion.Lerp(currentCam.rotation, playerTargetRot, Time.deltaTime * rotSensitivity);
            }
        }

        public void Respawn()
        {
            gameObject.SetActive(false);
            transform.position = checkpointManager.activeCheckpoint.position;
            gameObject.SetActive(true);
        }

        private void LookAtObject()
        {
            if (intObjects.Count <= 0)
            {
                lookTarget.position = lookTargetOrigin.position;
                return;
            }

            float closestDistance = Mathf.Infinity;

            foreach (Transform obj in intObjects)
            {
                RaycastHit hit;
                if (Physics.Raycast(head.position, obj.position, out hit))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;

                        if(GameManager.IsObjectInView(hit.transform, head))
                        {
                            lookTarget.Translate(hit.point);
                        }
                    }
                }
            }
        }

        private void Interact(Transform target = null)
        {
            RaycastHit hit;
            if(target == null)
            {
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(head.position, currentCam.transform.forward, out hit))
                {
                    if (hit.transform.CompareTag("Interactable"))
                    {
                        if (hit.transform.GetComponent<Interactable>().interactableObject.isBreakable)
                            intObjects.Remove(hit.transform);

                        hit.transform.GetComponent<Interactable>().Interact();
                    }
                }
            }

            else
            {
                if (target.CompareTag("Interactable"))
                {
                    if (target.GetComponent<Interactable>().interactableObject.isBreakable)
                        intObjects.Remove(target);
                    target.GetComponent<Interactable>().Interact();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interactable"))
            {
                if (!intObjects.Contains(other.transform))
                    intObjects.Add(other.transform);
            }

            if (other.CompareTag("Checkpoint"))
            {
                checkpointManager.UpdateCheckpoint(other.transform);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Interactable")) return;

            if (intObjects.Contains(other.transform))
                intObjects.Remove(other.transform);
        }
    }
}
