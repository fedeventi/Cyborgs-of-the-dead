using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeavyEnemy : BaseEnemy
{
    public float timer = 0;

    //
    bool hasSavePlayerPos = false;
    Vector3 playerPos;
    bool _stunned;
    public bool stunned { get => _stunned; set => _stunned = value; }
    //
    float timerFollowAgain = 0;
    bool hasTouchPlayer = false;
    bool _charging;
    public bool charging { get=>_charging; set=>_charging=value; }
    bool _checkedCharge;
    bool _hasChargedPlayer;
    public void ChargedPlayer()=>_hasChargedPlayer=false; 

    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        
        base.Update();
        //StunAttack();

        if (LookingPlayer() && !_checkedCharge && !InRangeToAttack())
        {
            StartCoroutine(checkCharge());
            _checkedCharge = true;
        }
        
    }
    IEnumerator checkCharge()
    {
        yield return new WaitForSeconds(3);
        Charge();
        _checkedCharge = false;
    }
    public void Charge()
    {
        var roulleteWheel = new RoulleteWheel<bool>();
        var probabilities = new List<Tuple<int, bool>> { new Tuple<int, bool>(6,true),
                                                         new Tuple<int, bool>(10,false) };
        if (roulleteWheel.ProbabilityCalculator(probabilities))
        {

            Transition("Charge");
        }
    }
    public override void SetStateMachine()
    {
        //Estados
        var idle = new IdleState<string>(this, enemyView);
        var patrol = new PatrolState<string>(this, enemyView);
        var attack = new AttackState<string>(this, enemyView);
        var chase = new ChaseState<string>(this, enemyView);
        var charge = new ChargeState<string>(this);
        
        //Transiciones
        idle.AddTransition("Patrol", patrol); //Va de idle a patrol
        idle.AddTransition("Chase", chase); //Va de idle a chase

        patrol.AddTransition("Idle", idle); //Va de patrol a idle
        patrol.AddTransition("Chase", chase); //Va de patrol a chase

        attack.AddTransition("Chase", chase); //Va de attack a chase
        
        charge.AddTransition("Chase", chase);
        
        chase.AddTransition("Attack", attack); //Va de chase a attack
        chase.AddTransition("Idle", idle); //Va de chase a idle
        chase.AddTransition("Patrol", patrol);
        chase.AddTransition("Charge", charge);
        //El FSM empieza con el patrol.
        fsm = new FSM<string>(idle);
    }
    IEnumerator WaitOnPoint()
    {
        yield return new WaitForSeconds(2f);
        hasSavePlayerPos = false;
        isInIdle = false;

        yield break;
    }


    IEnumerator FollowAgain()
    {
        yield return new WaitForSeconds(1);

        hasTouchPlayer = false;
        hasSavePlayerPos = false;

        yield break;
    }


    //Funcion para activar el stun 
    public void StunAttack()
    {
        if (meleeAttack.hasHit)
        {
            player.isStunned = true;
        }
    }
    IEnumerator Stunned()
    {
        
        enemyView.Stunned();
        _stunned = true;
        yield return new WaitForSeconds(2);
        _stunned = false;
        Transition("Chase");
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        
            if(collision.gameObject.GetComponent<PlayerModel>() && charging)
            {
                if (_hasChargedPlayer) return;
                collision.rigidbody.AddForce( transform.up*20000,
                                              ForceMode.Impulse);
                collision.gameObject.GetComponent<PlayerModel>().TakeDamage(30);
                _hasChargedPlayer = true;
            }
        if (collision.gameObject.layer == 8 && charging)
            StartCoroutine(Stunned());
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
    }

}
