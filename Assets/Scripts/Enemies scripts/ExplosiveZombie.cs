using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZombie : BaseEnemy
{
    [Header("Distance attack")]
    public DistanceAttackZE prefabAttack;
    public Transform posForDAttack;
    
    [Header("Particle system")]
    public GameObject Explosion;
    public Transform psPosition;
    public bool canShoot=true;
    public bool hasSeenPlayer;
    public GameObject vomit;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {

        base.Update();


            if (LookingPlayer())
            {
                if (!hasSeenPlayer)
                {
                    CloseEnemies();
                    hasSeenPlayer = true;
                }

            if (!InRangeToAttack())
            {

                if (canShoot)
                {

                    StartCoroutine(Shoot());
                }

            }

        }

            


            

           
        

      
    }
    public override IEnumerator Death()
    {

        
        speed = 0;
        
        
        var particle = Instantiate(Explosion, transform.position+transform.up*(headHight/2), transform.rotation);
        
        meleeAttack.gameObject.SetActive(false);
        isDead = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        myCollider.enabled = false;
        enemyView.DeathAnimation();

        yield break;
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
        fsm = new FSM<string>(idle);

    }
    public void DistanceAttack()
    {
        enemyView.DistanceAttackAnimation();
    }

    //se llama en la animacion de ataque a distancia
    public void SpawnAttack()
    {
        Instantiate(vomit, posForDAttack.position, transform.rotation);
        DistanceAttackZE temp = Instantiate(prefabAttack, posForDAttack.transform.position+ transform.forward*10, posForDAttack.transform.rotation);
        temp.destinyPosition = player.transform.position;
        Transition("Chase");
    }
}
