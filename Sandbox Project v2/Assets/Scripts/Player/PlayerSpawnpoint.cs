using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Cinemachine;

namespace Player {
    public class PlayerSpawnpoint : MonoBehaviour
    {
        public GameObject player;

        private void Start()
        {
            SpawnPlayer(transform, player);
        }

        public static void SpawnPlayer(Transform spawnPoint, GameObject player)
        {
            GameObject inst = Instantiate(player);
            inst.name = player.name;
            inst.transform.position = spawnPoint.position;
        }
    }
}
