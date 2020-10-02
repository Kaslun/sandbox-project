using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Cinemachine;

namespace Player {
    public class PlayerSpawnpoint : MonoBehaviour
    {
        public GameObject player;
        public Camera[] cameras;

        private GameObject inst;

        public void Start()
        {
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            if (player == null) return;
            inst = Instantiate(player);
            inst.name = player.name;
            inst.transform.position = transform.position;
        }
    }
}
