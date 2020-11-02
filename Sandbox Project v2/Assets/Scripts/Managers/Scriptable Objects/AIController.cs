using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Enemy", menuName = "NPC/Enemy")]
public class AIController : Controller
{
    [Header("AI")]
    public int maxPatrolPoints;
    public float maxViewDistance, maxSearchTime, pauseBetweenPoints;
    public Controller playerController;
}
