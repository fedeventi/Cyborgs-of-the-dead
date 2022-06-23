using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerModel : MonoBehaviour
{
    //Variables
    //Componentes:
    PlayerView view;
    Rigidbody rb;
    public GameObject body;
    [Header("Camara")]
    public Camera myCamera;

    //Movimiento:
    [Header("Movimiento")]
    public bool isRunning = false;
    public bool isMoving = false;
    public float speed;
    public float normalSpeed;
    public float runSpeed;

    //Vida y muerte
    [Header("Vida")]
    public int life;
    bool isDead = false;
    float timerLifeToxic = 0;
    bool _hasBeenHit;
    //Stunned
    [Header("Stunned")]
    public bool isStunned = false;

    //Game Master
    GameMaster gameMaster;
    //Scene
    Scene actualScene;

    //Toxicidad de radiacion
    [Header("TOXICITY")]
    public float toxicity = 0;
    public float timerToxicity = 0;
    public Text toxicityText;
    public bool increaseDamage = false;
    bool toxicityImprovementsRandom = false;
    public bool isInMedicineBox = false;

    //Weapon
    [Header("WEAPON")]
    public bool isReloading = false;
    public bool isShooting = false;
    public bool animationShooting = false;
    public WeaponHolder weaponHolder;
    public bool hasPickUpPistol = false;
    public bool hasPickUpShotgun = false;
    
    public TimeManager timeManager;
    public bool IsDead => isDead;
    private void Start()
    {
        //Game Master. Posicion al iniciar el juego.
        gameMaster = FindObjectOfType<GameMaster>();
       
        //Escena actual.
        actualScene = SceneManager.GetActiveScene();

        //Componentes
        view = GetComponent<PlayerView>();
        rb = GetComponent<Rigidbody>();

        //
        weaponHolder = FindObjectOfType<WeaponHolder>();

        //Variables
        life = 100;

        //
        toxicityText.enabled = false;

        //
        normalSpeed = speed;
        runSpeed = speed * 2;
    }
    void FixedUpdate()
    {
       
    }
    private void Update()
    {
        
        //Recargar los niveles al morir. Inicia en la ultima posición guardada, en relación a los checkpoints.
        if(isDead)
        {
            
            if (actualScene.name == "Level")
            {
                SceneManager.LoadScene("Level");
                life = 100;
            }
        }
        else
            view.LowLife(life);
        //Muerte 
        if (life <= 0)
        {
            life = 0;
            StartCoroutine(Death());
        }
        
        //random para las toxinas
        //TOXICITY
        Toxicity();

        //
        
    }

    //Toxicity 
    void Toxicity()
    {
        if(toxicity>25)
            StartCoroutine(view.ToxicitySound());

        if (toxicity > 50 && toxicity < 80)
        {
            //mejoras, como mas daño, velocidad, lo que sea y estas se activan al azar
            //que sea por tiempo 
            toxicityText.enabled = true;

            if (!toxicityImprovementsRandom)
            {
                toxicityImprovementsRandom = true;

                var r = Random.Range(0, 100);

                if (r <= 50)
                {
                    
                   
                    increaseDamage = true;
                    if (hasPickUpPistol)
                    {
                        weaponHolder.weaponsCollected[0].GetComponent<GunPistol>().damage *= 2;
                    }
                    if (hasPickUpShotgun)
                    {
                        weaponHolder.weaponsCollected[1].GetComponent<GunPistol>().damage *= 2;
                    }
                }
                if (r > 50)
                {
                    

                    speed = normalSpeed * 2f;
                    runSpeed = speed * 2f;
                }
                view.ChangeSprite(increaseDamage ? 1 : 2);
            }


        }
                
        if (toxicity > 80 )
        {
            //se tiene que curar el jugador por que esta al borde de empezar a perder vida
            toxicityText.text = $" Toxicity bar has reach max level ";
            

        }
        //resta vida mientras tiene toxinas en el cuerpo
        if (toxicity >= 100)
        {
            toxicity = 100;
            timerLifeToxic += Time.deltaTime;
            view.Toxic(true);
            if (timerLifeToxic > 1)
            {
                timerLifeToxic = 0;
                life--;

            }
        }
        else 
            view.Toxic(false);
        
        if (toxicity <= 0)
        {
            timerLifeToxic = 0;
            toxicity = 0;
            toxicityText.enabled = false;
            //vuelve los valores a la normalidad
            speed = normalSpeed;
            runSpeed = normalSpeed * 2;
            increaseDamage = false;
            toxicityImprovementsRandom = false;
            view.toxicityScreen.enabled = false;
        }

        //
        if (toxicity < 50)
        {
            speed = normalSpeed;
            runSpeed = normalSpeed * 2;
            increaseDamage = false;
            toxicityText.enabled = false;
            view.ChangeSprite( 0);
        }

        //sacar la screen toxicity
        if (isInMedicineBox)
        {
            view.toxicityScreen.enabled = false;
        }
    }


    public void CriticalKill()
    {
        
        timeManager.CreateShockwave(myCamera.transform.position + myCamera.transform.forward * 50, transform.rotation);
    }

    //Movimiento del jugador.
    public void Move(float axisHorizontal, float axisVertical)
    {
        Vector3 movement = (transform.right * axisHorizontal + transform.forward * axisVertical).normalized;
        _directionDebug = movement;
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }
    Vector3 _directionDebug;
    public void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        //Gizmos.DrawRay(transform.position, _directionDebug * (isRunning?runSpeed:speed)*.2f);
       //Gizmos.DrawRay(transform.position, _directionDebug * 40f);
    }
    public void Run(float axisHorizontal, float axisVertical)
    {
        Vector3 movement = (transform.right * axisHorizontal + transform.forward * axisVertical).normalized;
        if(movement.magnitude<=0) isRunning = false;
        _directionDebug = movement;
        var _speed = runSpeed;
        if (Physics.Raycast(transform.position, movement, 40, (int)Mathf.Pow(8 * 2, 2)))
        {
            _speed = normalSpeed;
        }
        rb.MovePosition(transform.position + movement * _speed * Time.deltaTime);

    }

    //Movimiento de la camara
    public void RotationCamera()
    {
        var hCamera = 120 * Input.GetAxis("Mouse X") * Time.deltaTime;
        var vCamera = Mathf.Clamp(Input.GetAxis("Mouse Y") * Time.deltaTime * 120,-20,20);
        transform.Rotate(0, hCamera, 0);
        myCamera.transform.Rotate(-vCamera, 0, 0);
        if (myCamera.transform.localRotation.x > 0.7f)
        {
            var auxRotation = myCamera.transform.localRotation;
            auxRotation.x = 0.7f;
            myCamera.transform.localRotation = auxRotation;
        }
        else if (myCamera.transform.localRotation.x < -0.7f)
        {
            var auxRotation = myCamera.transform.localRotation;
            auxRotation.x = -0.7f;
            myCamera.transform.localRotation = auxRotation;
        }


    }



    public void TakeDamage(int damage)
    {
        if (_hasBeenHit) return;
        life -= damage;
        view.DamageFeedback();
        myCamera.GetComponent<ShakeCamera>().ActivateShake(.5f);
        view.hitFeedback();
        StartCoroutine(Invulnerability());
        StartCoroutine(HitKnockback(3.5f));
        StartCoroutine(view.hitFeedback());
        //audioSource.PlayOneShot(myClips[1]);

    }

    //Para mover la camara cuando es golpeado
    IEnumerator HitKnockback(float force)
    {
        float currentForce = 0;
        float movement = 1;
        while (currentForce < force)
        {
            currentForce++;
            movement += movement;
            myCamera.transform.Rotate(-movement, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        movement = 1;
        while (currentForce > 0)
        {
            currentForce--;
            movement += movement;
            myCamera.transform.Rotate(movement, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    //invulnerabilidad para cuando te golpean
    IEnumerator Invulnerability()
    {
        _hasBeenHit = true;
        yield return new WaitForSeconds(.5f);
        _hasBeenHit = false;
    }
    //Stun
    public IEnumerator Stunned()
    {
        speed = 0;
        runSpeed = 0;
        StartCoroutine(view.StunScreen());
        yield return new WaitForSeconds(2f);
        isStunned = false;
        speed = normalSpeed;
        runSpeed = normalSpeed * 2;
        yield break;
    }
    //Muerte
    IEnumerator Death()
    {
        view.DeathFeedback();
        speed = 0;
        runSpeed = 0;
        //audioSource.PlayOneShot(myClips[0]);
        yield return new WaitForSeconds(1f);
        isDead = true;
        yield break;
    }

    //llamo a la funcion de disparo, para llamarla en la animacion 
    public void ShootFromGun()
    {
        Debug.Log("disparo");
        weaponHolder.weaponsCollected[(int)weaponHolder.actualWeapon].GetComponent<Weapon>().Shoot();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag == "GlassFragments")
        //{
        //    rb.velocity = Vector3.zero;

        //    collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
    }

}
