using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Manager;

public class EnemyAI : MonoBehaviour
{
    public Controller playerController;
    public AIController aiController;
    public NavMeshAgent agent;
    public StateMachine stateMachine;
    public GameManager gameManager;
    public GameObject playerObject;
    public List<Transform> path;

    private void Start()
    {
        stateMachine = FindObjectOfType<StateMachine>() as StateMachine;
        gameManager = FindObjectOfType<GameManager>() as GameManager;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerManager>().playerController;
        agent = GetComponent<NavMeshAgent>();

        agent.enabled = false;

        EventManager.StartListening("Loaded", Loaded);

    }

    //Game loaded
    private void Loaded()
    {
        agent.enabled = true;
        agent.baseOffset = agent.height / 2;
    }

    public void FixedUpdate()
    {
        if (!agent.isOnNavMesh || agent == null) return;

        //Checks to see if target is in AI-FOV
        if (!gameManager.IsObjectInView(playerObject.transform, transform))
        {
            //If last AI-action was chasing, search for player
            if (stateMachine.prevState == StateMachine.State.chase)
            {
                stateMachine.UpdateState(StateMachine.State.search, aiController, agent, playerObject.transform);
            }

            if(!stateMachine.isSearching)
            {
                Debug.Log("Setting Idle");
                stateMachine.UpdateState(StateMachine.State.idle, aiController, agent, null, this);
            }
        }

        //Target is in AI-FOV
        else
        {
            //If target is within reach, start attacking
            if (gameManager.DistanceToObject(playerObject.transform, transform) < aiController.reach)
                stateMachine.UpdateState(StateMachine.State.attack, aiController, agent, playerObject.transform);
            //If target is out of reach, chase target
            else
            {
                if (stateMachine.isSearching)
                {
                    stateMachine.StopCoroutine(stateMachine.SearchTimer());
                    stateMachine.isSearching = false;
                }
                stateMachine.UpdateState(StateMachine.State.chase, aiController, agent, playerObject.transform);
            }
        }
    }
}
