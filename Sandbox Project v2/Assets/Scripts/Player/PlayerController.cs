using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using System.Security.Cryptography;
using System.Threading;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Controller controller;
        private CharacterController characterController;
        private PlayerCamera playerCamera;
        private GameManager gameManager;

        public Transform lookTarget;
        private Vector3 targetOrigin;

        [SerializeField]
        private List<Transform> intObjects;
 
        private Transform currentCam;
        private RaycastHit hit;

        public float jumpForce;
        public float cameraSensitivity = 5;
        private Vector3 movement;
        private Vector3 moveDir;

        public float rotSensitivity = 3;
        public float sensitivity = 100f;
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
            characterController = GetComponent<CharacterController>();
            targetOrigin = lookTarget.position;
            isLoaded = true;
        }

        void Update()
        {
            if (!isLoaded) return;

            Move();
            Rotate();
            //LookAtObject();

            if (Input.GetButtonDown("Interact") && CanInteract())
                Interact();

            if (Input.GetButtonDown("Switch Camera")) {
                EventManager.TriggerEvent("SwitchMode");
                firstPerson = !firstPerson;
            }
        }

        private void Move()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            movement = new Vector3(moveX, 0f, moveZ).normalized;

            if (movement.magnitude >= 0.1f)
            {
                targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * currentCam.forward;

                characterController.Move((moveDir + Physics.gravity) * controller.speed * Time.deltaTime);
            }
        }
        private void Rotate()
        {
            currentCam = playerCamera.vCams[playerCamera.currentState].transform;

            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

            Vector3 rotX = Vector3.up * mouseX;
            Vector3 rotY = Vector3.right * -mouseY;
            Quaternion playerTargetRot;

            print("Movement = " + movement);

            playerTargetRot = Quaternion.LookRotation(moveDir + transform.forward);

            //Rotates the camera towards the direction of the player
            if (!firstPerson)
            {
                if(currentCam.forward != transform.forward)
                    currentCam.rotation = Quaternion.Lerp(currentCam.rotation, transform.rotation, Time.deltaTime * cameraSensitivity);

                transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRot, Time.deltaTime * rotSensitivity);
            }
            else
            {
                //Rotates the camera up and down

                Vector3 cameraTargetRot = rotY;
                currentCam.Rotate(cameraTargetRot);
                transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRot, Time.deltaTime * rotSensitivity);
            }
        }

        void LookAtObject()
        {
            if(intObjects.Count <= 0) return;

            float closestDistance = Mathf.Infinity;
            Transform targetObj = null;

            foreach (Transform obj in intObjects)
            {
                float distance = gameManager.DistanceToObject(obj, transform);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetObj = obj;

                    if(gameManager.IsObjectInView(targetObj, lookTarget))
                        lookTarget.position = targetObj.position;
                }
            }
        }

        private void Interact()
        {
            hit.transform.GetComponent<Interactable>().Interact();
        }

        private bool CanInteract(Transform target = null)
        {
            if(target == null)
            {
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, currentCam.transform.forward, out hit))
                {
                    if (hit.transform.CompareTag("Interactable"))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            else
            {
                if (target.CompareTag("Interactable"))
                    return true;
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactable")) return;

            if (!intObjects.Contains(other.transform))
                intObjects.Add(other.transform);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Interactable")) return;

            if (intObjects.Contains(other.transform))
                intObjects.Remove(other.transform);
        }
    }
}
