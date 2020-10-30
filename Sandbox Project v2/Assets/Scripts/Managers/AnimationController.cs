using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public enum AnimationState
    {
        idle,
        walk,
        run,
        attack,
        die
    }

    public AnimationState prevState;
    public AnimationState state;

    public void UpdateState(AnimationState nextState, Animator animator)
    {
        if (nextState == state) return;
        prevState = state;
        state = nextState;

        animator.SetTrigger(state.ToString());
    }
}
