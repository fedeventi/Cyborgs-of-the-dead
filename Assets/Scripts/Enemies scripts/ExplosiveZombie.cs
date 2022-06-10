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
                if(distance>3)
                {
                    //rota mirando al target.
                    var rot = Quaternion.LookRotation(player.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 6).normalized;

                }
                if (distance > 4 && distance<8)
                {
                    DistanceAttack();
                }
                else if(distance>=8 && !InRangeToAttack())
                {
                    fsm.Transition("Chase");
                }
            }

            if (hasListenGunShoot| hasTouchBullet || distance < 10 && !LookingPlayer())
            {
                
                isInIdle = true;
                isInPatrol = false;
                CloseEnemies();
                StartCoroutine(TouchBulletBool());
            }


            //Transicion al idle
            if (isInIdle && !InRangeToAttack())
            {
                fsm.Transition("Idle");
            }
            //Transicion al patrol
            if (isInPatrol && !InRangeToAttack() )
            {
                //ActionPatrol();
            }

            if (InRangeToAttack() || distance <= 4)
            {
                //navMesh.speed = 0;
                //navMesh.velocity = Vector3.zero;
                //navMesh.stoppingDistance = 10;
                enemyView.AttackAnimation();
            }
        }

        //EJECUTA LA MUERTE 
        else if (life <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
    }
    void DistanceAttack()
    {
        //navMesh.speed = 0;
        //navMesh.velocity = Vector3.zero;
        //navMesh.stoppingDistance = 10;
        speed = 0;
        stoppingDistanceChase = 10;
        enemyView.DistanceAttackAnimation();
    }

    //se llama en la animacion de ataque a distancia
    public void SpawnAttack()
    {
        DistanceAttackZE temp = Instantiate(prefabAttack, posForDAttack.transform.position, posForDAttack.transform.rotation);
        Destroy(temp, 1f);
    }
}
