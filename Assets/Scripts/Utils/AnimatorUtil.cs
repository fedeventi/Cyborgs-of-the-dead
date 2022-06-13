using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorUtil 
{
    public static bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public static bool AnimatorIsPlaying(Animator animator,string stateName)
            
    {
        return AnimatorIsPlaying(animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
