using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteState<T> : State<T>
{
    BaseEnemy _zombie;
    Animator _zombieAnimator;
    public BiteState(BaseEnemy zombie)
    {
        _zombie= zombie;
        _zombieAnimator= zombie.enemyView.GetComponent<Animator>();
    }
    public override void Awake()
    {
        base.Awake();
        _zombie.isBitting = true;
        _zombieAnimator.SetBool("Bite", true);
    }
    public override void Execute()
    {
        base.Execute();
        _zombie.player.ReleaseBitting();
        _zombie.player.LookTowards(_zombie.transform.position);
    }
    public override void Sleep()
    {
        _zombie.isBitting = false;
        _zombieAnimator.SetBool("Bite", false);
    }
}
