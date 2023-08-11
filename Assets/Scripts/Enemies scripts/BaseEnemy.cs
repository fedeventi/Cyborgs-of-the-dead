using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Linq;
using JoostenProductions;

public class BaseEnemy : OverridableMonoBehaviour
{
    public EnemyType enemyType;
    //Variables
    public float life;
    public float dist;
    public float speed = 1;
    public float stoppingDistanceChase;
    public float viewDistance = 200;
    [Range(0, 360)]
    public int angleVision = 90;
    public float attackDistance = 30;
    public int enemigosAlrededor = 0;
    public float rangoDetectarEnemigos = 5000;
    //Componentes 
    [Header("Player")]
    public PlayerModel player;
    public EnemyView enemyView;
    public Rigidbody rb;
    protected CapsuleCollider myCollider;
    [Header("Enemy melee attack")]
    public EnemyMeleeAttack meleeAttack;
    public float headHight = 100;
    public Waves wv;
    public List<BaseEnemy> enemigosDetectados = new List<BaseEnemy>();
    public bool tankBuff = false;
    public bool buffController = false;
    public bool buffControllerLessLife = false;

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
    public event Action<BaseEnemy> Recycle;
    //Recycle Variables
    protected Vector3 _initialPosition;
    protected float _initialSpeed;
    protected float _initialLife;
    //bool 
    [Header("BOOLS")]
    public bool hasTouchBullet = false;
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
    public Vector3[] waypointsEnemy;
    public int currentWaypoint = 0;

    //Roulette

    protected Roulette roulette;
    string actionToDo;
    bool actionOfRoulette = false;
    float timerRoulette = 0;
    [Header("RAGDOLL")]
    public List<Collider> ragdollColliders = new List<Collider>();
    public bool ragdoll;
    protected EnemySaveData _saveData;


    public BaseEnemy SetRecycleAction(Action<BaseEnemy> action)
    {
        Recycle += action;

        return this;
    }
    public virtual void Awake()
    {

        //Componentes
        _initialPosition = transform.position;
        _initialSpeed = speed;
        _initialLife = life;
        enemyView = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        baseEnemy = GetComponent<BaseEnemy>();
        _targetDetection = GetComponent<TargetDetection>();
        wv = FindObjectOfType<Waves>();



        player = FindObjectOfType<PlayerModel>();
        deathAction += player.CriticalKill;
        weaponHolder = FindObjectOfType<WeaponHolder>();


        //Fsm

        SetStateMachine();
        //roulette
        roulette = new Roulette();
    }
    public override void Start()
    {
        base.Start();
        Reset();

    }
    public override void UpdateMe()
    {

        if (ragdoll) return;
        if (this.gameObject.tag == "ZombieTank")
        {
            enemyType = EnemyType.tank;
            DetectarEnemigos();
            ActualizarListaEnemigos();
        }

        if (this.gameObject.tag == "Zombie")
        {
            if (tankBuff == true && buffController == false)
            {
                life += 75;
                buffController = true;
                buffControllerLessLife = false;
            }
            if (tankBuff == false && buffControllerLessLife == false)
            {
                life -= 75;
                buffControllerLessLife = true;
            }
        }

        // if (player)
        //     if (Vector3.Distance(baseEnemy.transform.position, baseEnemy.player.transform.position) > 2500)
        //     {
        //         return;
        //     }
        if (life > 0)
        {
            fsm.OnUpdate();

        }

        //EJECUTA LA MUERTE 

    }

    //Esto es para que el Zombie Tank detecte a los enemigos que estan cerca
    public void DetectarEnemigos()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangoDetectarEnemigos);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Zombie") && collider.gameObject != gameObject)
            {
                BaseEnemy enemigo = collider.GetComponent<BaseEnemy>();

                if (!enemigosDetectados.Contains(enemigo))
                {
                    enemigosDetectados.Add(enemigo);
                }
            }
        }
    }
    public void ActualizarListaEnemigos()
    {
        for (int i = enemigosDetectados.Count - 1; i >= 0; i--)
        {
            BaseEnemy enemigo = enemigosDetectados[i];

            if (enemigo == null || Vector3.Distance(transform.position, enemigo.transform.position) > rangoDetectarEnemigos)
            {
                enemigosDetectados.RemoveAt(i);
                Debug.Log("Me aleje");
                enemigo.tankBuff = false;
                enemigo.buffController = false;
            }

            if (enemigosDetectados.Count >= 5)
            {
                Debug.Log("Ok");
                enemigo.tankBuff = true;
            }
        }

    }

    //calcula si la distancia es la adecuada para atacar
    public bool InRangeToAttack()
    {
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackDistance)
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
            if (enemy != null)
                enemy.SeeEnemy();
        }
    }
    //Automaticamente ve al jugador
    public IEnumerator SeeEnemy()
    {
        Transition("Idle");
        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (viewDistance < distance)
            viewDistance = distance;

        var target = player.transform.position;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;

        var rot = Quaternion.LookRotation(dir);
        var angle = Vector3.Angle(transform.forward, dir);
        while (angle > 30)
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
        if (!player) return false;
        // else return true; //para que lo vea siempre, si se quiere que haya que calcular si realmente lo esta viendo o no, hay que desactivar esta linea
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
            if (Physics.Raycast(transform.position + transform.up * headHight, difference.normalized, distance, maskObstacles)) //Si choca contra la pared (el rango de visión, devuelve falso)
            {
                return false;
            }
            else
                return true;
        }
    }

    public Vector3[] _waypoints;
    public int _currentWaypoint;
    public virtual void OnDrawGizmos()
    {
        if (player != null)
            if (LookingPlayer())
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.blue;
        if (_waypoints != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _waypoints.Length; i++)
                if (i >= _currentWaypoint)
                    Gizmos.DrawSphere(_waypoints[i], 20);
        }
        // for (int i = -angleVision / 2; i < angleVision / 2; i += 5)
        // {
        //     Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(i, transform.up) * transform.forward * viewDistance);
        // }
        foreach (Vector3 pos in waypointsEnemy)
        {
            Gizmos.DrawSphere(pos, 10);

        }

        Gizmos.color = Color.red;
        for (int i = -angleVision / 2; i < angleVision / 2; i += 5)
        {
            Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(i, transform.up) * transform.forward * attackDistance);
        }
        for (int i = -angleVision / 2; i < angleVision / 2; i += 10)
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
    public void TakeDamage(float damage, bool headshot = false)
    {
        if (PointsManager.instance)
            PointsManager.instance.AddCombo((int)damage);
        //StartCoroutine(DamageVelocity());
        life -= damage;
        if (life <= 0 && !isDead)
        {
            if (headshot)
                enemyView.DestroyHead();
            isDead = true;
            StartCoroutine(Death());
        }

        enemyView.DamageSound();
        if (player)
            if (!LookingPlayer())
                StartCoroutine(SeeEnemy());

    }
    //Creo la state Machine
    public virtual void SetStateMachine()
    {
        //FSM
        var idle = new IdleState<string>(baseEnemy, enemyView);
        var patrol = new PatrolState<string>(baseEnemy, enemyView, true);

        var attack = new AttackState<string>(baseEnemy, enemyView);
        var chase = new ChaseState<string>(baseEnemy, enemyView);
        var search = new SearchState<string>(this, enemyView);
        idle.AddTransition("Patrol", patrol); //Va de idle a patrol
        idle.AddTransition("Chase", chase); //Va de idle a chase
        idle.AddTransition("Search", search); //Va de idle a search

        patrol.AddTransition("Idle", idle); //Va de patrol a idle
        patrol.AddTransition("Chase", chase); //Va de patrol a chase
        patrol.AddTransition("Search", search); //Va de patrol a search

        attack.AddTransition("Chase", chase); //Va de attack a chase
        attack.AddTransition("Idle", idle);

        search.AddTransition("Chase", chase);

        chase.AddTransition("Attack", attack); //Va de chase a attack
        chase.AddTransition("Idle", idle); //Va de chase a idle
        chase.AddTransition("Search", search);

        //El FSM empieza con el patrol.
        fsm = new FSM<string>(idle);
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


        enemyView.DeathSound();
        if (PointsManager.instance)
            PointsManager.instance.AddKill((int)enemyType);
        meleeAttack.gameObject.SetActive(false);
        enemyView.SetAnimator(false);
        RoulleteWheel<bool> rw = new RoulleteWheel<bool>();
        bool lostHead = enemyView.head && !enemyView.head.activeSelf;
        Tuple<int, bool>[] probabilities = new Tuple<int, bool>[2]
        {
            new Tuple<int,bool>(100, false),
            new Tuple<int,bool>(lostHead?200:10, true),
        };
        //if (rw.ProbabilityCalculator(probabilities))
        //    deathAction();

        wv.enemyAmount -= 1;
        if (!ragdoll)
            ActiveRagdoll(true);
        StartCoroutine(Dissolve());
        yield break;
    }
    public void ActiveRagdoll(bool activate)
    {
        ragdoll = activate;
        enemyView.SetAnimator(!activate);
        rb.isKinematic = activate;
        myCollider.enabled = !activate;
        foreach (var item in ragdollColliders)
        {
            if (item.tag == "headshot") continue;
            item.enabled = activate;
            item.GetComponent<Rigidbody>().isKinematic = !activate;

        }
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(1);
        enemyView.CreatePoolBlood();
        yield return new WaitForSeconds(5);
        foreach (var item in ragdollColliders)
        {
            item.enabled = false;
            item.GetComponent<Rigidbody>().isKinematic = true;
        }
        float seconds = 0;
        while (seconds < 5)
        {
            Debug.Log("subiendo");
            transform.position -= transform.up * 60 * Time.deltaTime;
            seconds += Time.deltaTime;
            yield return null;
        }
        // if (Recycle != null)
        //     StartCoroutine(RecycleCR());
        Destroy(gameObject);
        //else
        // gameObject.SetActive(false);
    }
    public IEnumerator RecycleCR()
    {
        if (Recycle != null)
        {
            transform.position = _initialPosition;
            //fsm.GetState();
            Reset();
            if (Recycle != null)
                Recycle.Invoke(this);

        }
        yield return null;
    }
    public virtual void Reset()
    {

        life = _initialLife;
        speed = _initialSpeed;
        enemyView.DestroyPoolBlood();
        enemyView.SetAnimator(true);
        Destroy(enemyView.lastHeadExplosion);
        if (enemyView.head)
            enemyView.head.SetActive(true);
        meleeAttack.gameObject.SetActive(true);
        isDead = false;
        ActiveRagdoll(false);
        Transition("Idle");
    }

    //Colisiones 

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Colision con el area del sonido del arma y si esta dispara, se activa el bool
        if (other.gameObject.tag == "GunSoundArea")
        {
            hasListenGunShoot = true;
            if (!LookingPlayer()) StartCoroutine(SeeEnemy());
        }

        if (other.gameObject.tag == "Zombie")
        {
            enemigosAlrededor++;
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
        if (other.gameObject.tag == "Zombie")
        {
            enemigosAlrededor--;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //para evitar que al colisionar con el jugador, el enemigo se mueva.
        if (collision.gameObject.tag == "Player")
        {



        }

        // if (collision.gameObject.layer == 13)
        // {
        //     life -= 200;
        //     enemyView.CreatePoolBlood();
        //     if(life <= 0)
        //     {
        //         Destroy(this.gameObject);
        //     }
        // }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
    protected override void OnDisable()
    {

        base.OnDisable();
    }

    public void OnShoot(params object[] variables)
    {
        TakeDamage((float)variables[0], (bool)variables[1]);
    }
}
public enum EnemyType
{
    normal,
    explosive,
    heavy,
    tank
}
public class EnemySaveData
{
    Vector3 _position;
    Quaternion _rotation;
    float life;
    bool _activeSelf;
    public EnemySaveData(BaseEnemy enemy)
    {
        _position = enemy.transform.position;
        _rotation = enemy.transform.rotation;
        life = enemy.life;
        _activeSelf = enemy.gameObject.activeSelf;

    }
    public void Restore(BaseEnemy enemy)
    {
        enemy.transform.position = _position;
        enemy.transform.rotation = _rotation;
        enemy.life = life;
        enemy.gameObject.SetActive(_activeSelf);
    }
}
