using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZombie : BaseEnemy
{
    [Header("Distance attack")]
    public DistanceAttackZE prefabAttack;
    public Transform posForDAttack;

    [Header("Particle system")]
    public ParticleSystem particleSystemExplosion;
    public Transform psPosition;
    public bool canShoot=true;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if(life>0)
        {
            fsm.OnUpdate();

            var distance = Vector3.Distance(transform.position, player.transform.position);

            if (LookingPlayer())
            {
                CloseEnemies();
                hasListenGunShoot = false;
                isInIdle = false;
                isInPatrol = false;
                
                if (!InRangeToAttack())
                {

                    if (canShoot)
                    {
                        
                        StartCoroutine(Shoot());
                    }
                   
                }
                
            }

            if (hasListenGunShoot| hasTouchBullet || distance < 10 && !LookingPlayer())
            {
                
                isInIdle = true;
                isInPatrol = false;
                CloseEnemies();
                StartCoroutine(TouchBulletBool());
            }


            

           
        }

        //EJECUTA LA MUERTE 
        else if (life <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
    }
    IEnumerator Shoot()
    {
        canShoot=false;
        Transition("Range");
        yield return new WaitForSeconds(Random.Range(7,15));
        
        canShoot = true;
    }
    public override void SetStateMachine()
    {
        //Estados
        var idle = new IdleState<string>(this, enemyView);
        var patrol = new PatrolState<string>(this, enemyView);
        var attack = new AttackState<string>(this, enemyView);
        var chase = new ChaseState<string>(this, enemyView);
        var range = new RangeAttack<string>(this, enemyView);

        //Transiciones
        idle.AddTransition("Patrol", patrol); //Va de idle a patrol
        idle.AddTransition("Chase", chase); //Va de idle a chase

        patrol.AddTransition("Idle", idle); //Va de patrol a idle
        patrol.AddTransition("Chase", chase); //Va de patrol a chase

        attack.AddTransition("Chase", chase); //Va de attack a chase

        range.AddTransition("Chase", chase);

        chase.AddTransition("Attack", attack); //Va de chase a attack
        chase.AddTransition("Idle", idle); //Va de chase a idle
        chase.AddTransition("Patrol", patrol);
        chase.AddTransition("Range", range);
        //El FSM empieza con el patrol.
        fsm = new FSM<string>(patrol);

    }
    public void DistanceAttack()
    {
        enemyView.DistanceAttackAnimation();
    }

    //se llama en la animacion de ataque a distancia
    public void SpawnAttack()
    {
        
        DistanceAttackZE temp = Instantiate(prefabAttack, posForDAttack.transform.position, posForDAttack.transform.rotation);
        Transition("Chase");
    }
}
