using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Manager;
using JetBrains.Annotations;

public class EnemyAI : MonoBehaviour
{
    public AIController aiController;

    private NavMeshAgent agent;
    private StateMachine stateMachine;
    private GameObject playerObject;

    private Animator anim;

    [HideInInspector]
    public List<Transform> path;

    private void Start()
    {
        stateMachine = FindObjectOfType<StateMachine>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
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

        //Target is not in AI-FOV
        if (!GameManager.IsObjectInView(playerObject.transform, transform))
        {
            //If last AI-action was chasing, search for player
            if (stateMachine.prevState == StateMachine.State.chase)
            {
                stateMachine.UpdateState(StateMachine.State.search, aiController, agent, playerObject.transform);
            }

            if(!stateMachine.isSearching)
            {
                stateMachine.UpdateState(StateMachine.State.idle, aiController, agent, null, this);
            }
        }

        //Target is in AI-FOV
        else
        {
            //If target is within reach, start attacking
            if (GameManager.DistanceToObject(playerObject.transform, transform) < aiController.reach)
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

    public IEnumerator Die()
    {
        anim.Play("Die");

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
