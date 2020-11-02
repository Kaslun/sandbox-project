using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Manager;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        idle,
        chase,
        search,
        attack
    }

    public State state;
    public State prevState;
    private int currPoint = 0;
    public bool isSearching = false;

    public void UpdateState(State nextState, AIController controller, NavMeshAgent agent = null, Transform target = null, EnemyAI enemyAI = null)
    {
        prevState = state;
        state = nextState;

        switch (state)
        {
            case State.idle:
                DoPatrol(controller, agent, enemyAI);
                break;
            case State.chase:
                DoChase(controller, agent, target);
                break;
            case State.search:
                DoSearch(controller, agent, target);
                break;
            case State.attack:
                DoAttack(controller, target);
                break;
        }
    }

    private void DoPatrol(AIController controller, NavMeshAgent agent, EnemyAI enemyAI)
    {
        if (agent == null) return;

        if (agent.remainingDistance < 0.5f || prevState == State.search)
            SetNextDestination(controller.pauseBetweenPoints, enemyAI.path, agent);
    }

    private void DoChase(Controller controller, NavMeshAgent agent, Transform target)
    {
        if (agent == null) return;
        agent.destination = target.transform.position;
    }

    private void DoSearch(AIController controller, NavMeshAgent agent, Transform target)
    {
        if (agent == null) return;
        agent.destination = target.position;

        StartCoroutine(SearchTimer(controller.maxSearchTime));
    }

    private void DoAttack(Controller controller, Transform target)
    {
        GameManager.SendDamage(4, null);
    }

    public IEnumerator SearchTimer(float maxTime = 0)
    {
        float timer = 0;
        while (timer < maxTime && state == State.search)
        {
            isSearching = true;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(.01f);
        }

        isSearching = false;
        yield break;
    }

    private void SetNextDestination(float timer, List<Transform> path, NavMeshAgent agent)
    {
        if (agent.pathPending || agent.isStopped) return;

        agent.destination = path[currPoint].position;
        currPoint = (currPoint + 1) % path.Count;

        agent.isStopped = false;
    }
}
