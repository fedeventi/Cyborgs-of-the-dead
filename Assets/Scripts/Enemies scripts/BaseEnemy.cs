using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Linq;

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
    public Rigidbody rb;
    protected CapsuleCollider myCollider;
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
    public event Action deathAction;
    

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
    [Header("RAGDOLL")]
    public List<Collider> ragdollColliders = new List<Collider>();
    

    public virtual void Start()
    {
        //Componentes
        player = FindObjectOfType<PlayerModel>();
        deathAction += player.CriticalKill;
        enemyView = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        baseEnemy = GetComponent <BaseEnemy>();
        weaponHolder = FindObjectOfType<WeaponHolder>();
        _targetDetection = GetComponent<TargetDetection>();
        
        //Fsm
        SetStateMachine();


        
        

        //roulette
        roulette = new Roulette();
    }
    
    public virtual void Update()
    {
       

        if (life>0)
        {
           
            fsm.OnUpdate();
            

            
           
            

            
            
            
            
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

    //funcion para avisar a los enemigos cercanos, que miren al jugador. No usada
    public void CloseEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 500, enemiesMask);
        
        foreach (var c in colliders)
        {
            var enemy = c.gameObject.GetComponent<BaseEnemy>();
            if(enemy != null)
                enemy.SeeEnemy();
        }
    }
    //Automaticamente ve al jugador
    public IEnumerator SeeEnemy()
    {
        Transition("Idle");
        var distance = Vector3.Distance(player.transform.position, transform.position);
        if(viewDistance<distance)
            viewDistance = distance;
        
        var target = player.transform.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;

        var rot = Quaternion.LookRotation(dir);
        var angle=Vector3.Angle(transform.forward, dir);
        while (angle>30)
        {
            yield return new WaitForSeconds(0.05f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3).normalized;
            angle = Vector3.Angle(transform.forward, dir);
        }
        yield break;
        
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
    public void TakeDamage(float damage,bool headshot=false)
    {
        
        //StartCoroutine(DamageVelocity());
        life -= damage;
        if (headshot)
            if (life <= 0)
                enemyView.DestroyHead();
        enemyView.DamageSound();
        if (!LookingPlayer())
            StartCoroutine(SeeEnemy());

    }
    //Creo la state Machine
    public virtual void SetStateMachine()
    {
        //FSM
        var idle = new IdleState<string>(baseEnemy, enemyView);
        var patrol = new PatrolState<string>(baseEnemy, enemyView,true);

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
        var _speedAux = speed;
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        speed = _speedAux;
        isDamage = false;

       
    }
    
    //corrutina de la muerte
    public virtual IEnumerator Death()
    {
        
        speed = 0;
        deathAction();
        meleeAttack.gameObject.SetActive(false);
        isDead = true;
        foreach (var item in ragdollColliders)
        {
            item.enabled = true;
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        myCollider.enabled = false;
        enemyView.DisableAnimator();
        
        yield break;
    }

    


    //Colisiones 

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Colision con el area del sonido del arma y si esta dispara, se activa el bool
        if (other.gameObject.tag == "GunSoundArea")
        {
            hasListenGunShoot = true;
            if(!LookingPlayer()) StartCoroutine(SeeEnemy());
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GunSoundArea")
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
