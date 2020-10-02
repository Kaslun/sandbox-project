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
        public float jumpForce;
        //Placeholder indicator
        private RawImage interactionIndicator;

        [HideInInspector]
        public Camera currentCam;
        public CameraManager cameraManager;

        void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
        }

        private void Loaded()
        {
            interactionIndicator = FindObjectOfType<RawImage>();
            cameraManager = FindObjectOfType<CameraManager>();
            currentCam = cameraManager.currentCam;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentCam == null) currentCam = FindObjectOfType<Camera>();
            interactionIndicator.enabled = CanInteract();

            if (Input.GetButtonDown("Interact") && CanInteract())
            {
                Interact();
            }

            if (Input.GetButtonDown("Switch Camera"))
            {
                EventManager.TriggerEvent("SwitchMode");
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
}
