using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public enum AnimState
    {
        idle,
        walk,
        run,
        attack,
        die
    }

    public AnimState prevState;
    public AnimState state;

    public void UpdateState(AnimState nextState, Animator animator)
    {
        /*if (nextState == state) return;
        prevState = state;
        state = nextState;

        animator.SetTrigger(state.ToString());*/
    }
}
