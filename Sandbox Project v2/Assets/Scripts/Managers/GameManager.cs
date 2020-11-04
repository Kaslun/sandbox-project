﻿using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Manager {
    public class GameManager : MonoBehaviour
    {
        
        public bool debugMode;

        [SerializeField]
        float delay = 1;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(LoadDelay(delay));
        }

        IEnumerator LoadDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EventManager.TriggerEvent("Loaded");

        }

        public static float GetAnimLenght(Animation animation)
        {
            return animation.clip.length;
        }

        public static void SendDamage(int damage, Controller controller)
        {
            Debug.Log("Damaged object for: " + damage + " health");
        }

        public static float DistanceToObject(Transform target, Transform origin)
        {
            float distance = Vector3.Distance(target.position, origin.position);
            return distance;
        }

        public static bool IsObjectInView(Transform target, Transform origin)
        {
            RaycastHit hit;

            Vector3 targetDir = target.position - origin.position;
            float angleToObject = (Vector3.Angle(targetDir, origin.forward));

            //Checks if player is within set FOV
            if (Physics.Raycast(origin.position, targetDir, out hit))
            {
                //If player is in FOV, within distance and in view, start chasing
                if (angleToObject >= -90 && angleToObject <= 90)
                {
                    if (hit.collider.tag == target.tag)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
}
