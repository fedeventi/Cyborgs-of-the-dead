using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BaseEnemy : MonoBehaviour
{
    //Variables
    public float life;
    public float dist;
    public float speed=1;
    public float deathTimer;
    public float stoppingDistanceChase;
    public float viewDistance=200;
    [Range(0,360)]
    public int angleVision=90;
    public float attackDistance=30;
    //Componentes 
    [Header("Player")]
    public PlayerModel player;
    public EnemyView enemyView;
    Rigidbody rb;
    CapsuleCollider myCollider;
    [Header("Enemy melee attack")]
    public EnemyMeleeAttack meleeAttack;
    public float headHight=100;
    //
    BaseEnemy baseEnemy;
    //
    TargetDetection _targetDetection;
    public TargetDetection targetDetection { get { return _targetDetection; } set { _targetDetection = value; } }
    //
    WeaponHolder weaponHolder;
    //
    //[Header("NAVMESH")]
    //public NavMeshAgent navMesh;

    //Efecto sangre 
    [Header("Blood spray")]
    public GameObject bloodSpray;

    //bool 
    [Header("BOOLS")]
    public bool hasTouchBullet=false;
    public bool isInIdle = false; //Bool para saber cuando este en idle y cuando no
    public bool hasListenGunShoot = false;
    public bool isDead = false;
    public bool isDamage = false;
    public bool isInPatrol = false;
    public bool waitOnWP = false;
    

    //FSM
    public QuestionNode isSeeingPlayer;

    ActionNode patrolAction;
    ActionNode chaseAction;

    public FSM<string> fsm;

    [Header("LAYERS")]
    public LayerMask maskObstacles;
    public LayerMask enemiesMask;

    [Header("Patrol waypoints")]
    public Transform[] waypointsEnemy;
    public int currentWaypoint = 0;

    //Roulette

    protected Roulette roulette;
    string actionToDo;
    bool actionOfRoulette = false;
    float timerRoulette = 0;

   
    public virtual void Start()
    {
        //Componentes
        player = FindObjectOfType<PlayerModel>();
        enemyView = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        baseEnemy = GetComponent <BaseEnemy>();
        weaponHolder = FindObjectOfType<WeaponHolder>();
        _targetDetection = GetComponent<TargetDetection>();
        
        //Fsm
        SetStateMachine();


        
        isSeeingPlayer = new QuestionNode(LookingPlayer, chaseAction, patrolAction);

        //roulette
        roulette = new Roulette();
    }
    
    public virtual void Update()
    {
        if (life>0)
        {
           
            fsm.OnUpdate();
            

            
           
            

            

            //si escucha el sonido del disparo, y no esta viendo al jugador, mira para su direccion para poder perseguirlo.
            //si recibe un disparo del jugador, y no esta viendo al jugador, mira para su direccion para poder perseguirlo.
            //si esta a tal distancia del jugador, y no esta viendo al jugador, mira para su direccion para poder perseguirlo.
            
           
            

            //Transicion al idle
            //if (isInIdle)
            //{
            //    fsm.Transition("Idle");
            //}

            //RULETA (IDLE - PATROL)
            //if(this.gameObject.tag=="NormalEnemy")
            //{
            //    if (!isLookingAtPlayer || !LookingPlayer())
            //    {
            //        //Timer para que se ponga en idle
            //        timerRoulette += Time.deltaTime;
            //        if (timerRoulette > 5f)
            //        {
            //            //Aca quiero que elija patrol o idle
            //            if (!actionOfRoulette)
            //            {
            //                Dictionary<string, int> dic = new Dictionary<string, int>(); //Añado ambos estados al diccionario
            //                dic.Add("GoIdle", 40);
            //                dic.Add("KeepPatrol", 30);
            //                actionToDo = roulette.Execute(dic); //Ejecuto la ruleta 

            //                if (actionToDo == "GoIdle")
            //                {
            //                    isInIdle = true;
            //                    isInPatrol = false;
            //                    actionOfRoulette = true;
            //                }
            //                if (actionToDo == "KeepPatrol")
            //                {
            //                    isInIdle = false;
            //                    isInPatrol = true;
            //                    actionOfRoulette = true;
            //                }
            //            }
            //            if (timerRoulette > 10f)
            //            {
            //                timerRoulette = 0f;
            //                isInIdle = false;
            //                actionOfRoulette = false;
            //            }
            //        }
            //    }
            //}
            
        }

        //EJECUTA LA MUERTE 
        else if(life<=0 && !isDead)
        {
            StartCoroutine(Death());
        }
    }
    //calcula si la distancia es la adecuada para atacar
    public bool InRangeToAttack()
    {
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= attackDistance)
        {
            return true;
        }
        else
            return false;
    }

    //funcion para avisar a los enemigos cercanos, que miren al jugador.
    public void CloseEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 500, enemiesMask);
        foreach (var c in colliders)
        {
            //activa el looking player
            //c.GetComponent<BaseEnemy>().isLookingAtPlayer = true;
        }
    }
    
   

    //Esta funcion solamente es un bool para saber si ve o no al jugador
    public bool LookingPlayer()
    {
        Vector3 difference = (player.transform.position - transform.position);
        //A--->B
        //B-A
        float distance = difference.magnitude;
        //Se saca la distancia entre el enemigo y el jugador, haciendo B-A. 

        if (distance > viewDistance || Vector3.Angle(transform.forward, difference) > angleVision) //Si la distancia es mayor al rango que tiene el enemigo, no lo esta viendo.
        {
            return false;
        }
        else
        {
            if (Physics.Raycast(transform.position+transform.up*headHight, difference.normalized, distance, maskObstacles)) //Si choca contra la pared (el rango de visión, devuelve falso)
            {
                return false;
            }
            else
                return true;
        }

        
    }
    
    
    public virtual void OnDrawGizmos()
    {
        if(player!=null)
        if(LookingPlayer())
            Gizmos.color= Color.green;
        else
            Gizmos.color = Color.blue;

        for (int i = -angleVision/2; i < angleVision/2; i+=5)
        {
            Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(i, transform.up) * transform.forward * viewDistance);
        }
        
        Gizmos.color = Color.red;
        for (int i = -angleVision/2; i < angleVision/2; i += 5)
        {
            Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(i, transform.up) * transform.forward * attackDistance);
        }
        for (int i = -angleVision/2; i < angleVision/2; i += 10)
        {
            Gizmos.DrawRay(transform.position + transform.up * headHight, Quaternion.AngleAxis(i, transform.up) * transform.forward * 10);
            
        }
        

    }
    //Con esto Transiciono a cualquier estado
    public void Transition(string actionName)
    {
        fsm.Transition(actionName);
    }
    

    //Daño que recibe
    //Funcion que llama el script de GunPistol
    public void TakeDamage(float damage)
    {
        StartCoroutine(DamageVelocity());
        life -= damage;
        enemyView.DamageSound();

    }
    //Creo la state Machine
    public virtual void SetStateMachine()
    {
        //FSM
        var idle = new IdleState<string>(baseEnemy, enemyView);
        var patrol = new PatrolState<string>(baseEnemy, enemyView);

        var attack = new AttackState<string>(baseEnemy, enemyView);
        var chase = new ChaseState<string>(baseEnemy, enemyView);

        idle.AddTransition("Patrol", patrol); //Va de idle a patrol
        idle.AddTransition("Chase", chase); //Va de idle a chase

        patrol.AddTransition("Idle", idle); //Va de patrol a idle
        patrol.AddTransition("Chase", chase); //Va de patrol a chase

        attack.AddTransition("Chase", chase); //Va de attack a chase

        chase.AddTransition("Attack", attack); //Va de chase a attack
        chase.AddTransition("Idle", idle); //Va de chase a idle
        chase.AddTransition("Patrol", patrol);
        //El FSM empieza con el patrol.
        fsm = new FSM<string>(patrol);
    }
    
    //corrutina para delay en la velocidad cuando recibe daño.
    IEnumerator DamageVelocity()
    {
        isDamage = true;
        //navMesh.speed = 0;
        //navMesh.stoppingDistance = 100;
        speed = 0;
        stoppingDistanceChase = 100;
        yield return new WaitForSeconds(0.3f);

        isDamage = false;

        yield break;
    }
    
    //corrutina de la muerte
    public IEnumerator Death()
    {
        isDead = true;
        //navMesh.speed = 0;
        speed = 0;
        //navMesh.stoppingDistance = 100;
        stoppingDistanceChase = 100;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        enemyView.DeathAnimation();
        //Destroy(gameObject, deathTimer);
        myCollider.enabled = false;
        yield break;
    }

    //corrutina para poner en falso, el bool de si colisiono con la bala
    public IEnumerator TouchBulletBool()
    {
        yield return new WaitForSeconds(0.2f);
        hasTouchBullet = false;
        yield break;
    }

    //corrutina para pararse en los waypoints, y para volver a patrullar
    public IEnumerator WaitOnWaypoint()
    {

        waitOnWP = true;
        isInPatrol = false;
        isInIdle = true;

        yield return new WaitForSeconds(2.5f);

        isInPatrol = true;
        isInIdle = false;
        waitOnWP = false;

        yield break;
    }


    //Colisiones 

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Colision con el area del sonido del arma y si esta dispara, se activa el bool
        if (other.gameObject.tag == "GunSoundArea" && weaponHolder.weapons[weaponHolder.actualWeapon].GetComponent<GunPistol>().model.isShooting)
        {
            hasListenGunShoot = true;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        //Colision con el area del sonido del arma y si esta dispara, se activa el bool
        if (other.gameObject.tag == "GunSoundArea" && weaponHolder.weapons[weaponHolder.actualWeapon].GetComponent<GunPistol>().model.isShooting)
        {
            hasListenGunShoot = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GunSoundArea" && weaponHolder.weapons[weaponHolder.actualWeapon].GetComponent<GunPistol>().model.isShooting)
        {
            hasListenGunShoot = false;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //para evitar que al colisionar con el jugador, el enemigo se mueva.
        if (collision.gameObject.tag == "Player")
        {
            
            
            rb.velocity = Vector3.zero;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
        }
    }

}
