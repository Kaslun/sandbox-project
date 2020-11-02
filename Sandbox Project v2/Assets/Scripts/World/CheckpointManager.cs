using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager {
    public class CheckpointManager : MonoBehaviour
    {
        private GameObject[] checkpointList;
        public Transform activeCheckpoint;

        public Color active;
        public Color inactive;

        private bool isLoaded = false;

        private void Start()
        {
            EventManager.StartListening("Loaded", Loaded);
        }

        private void Loaded()
        {
            checkpointList = GameObject.FindGameObjectsWithTag("Checkpoint");
            isLoaded = true;
        }

        public void UpdateCheckpoint(Transform cp)
        {
            if (cp == activeCheckpoint) return;

            for (int i = 0; i < checkpointList.Length; i++)
            {
                if (cp == checkpointList[i].transform)
                {
                    activeCheckpoint = cp;
                    cp.GetComponent<Renderer>().material.color = active;
                }
                else
                {
                    checkpointList[i].GetComponent<Renderer>().material.color = inactive;
                }
            }
        }
    }
}
