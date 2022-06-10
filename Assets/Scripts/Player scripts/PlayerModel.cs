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
    public int toxicity = 0;
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


    private void Start()
    {
        //Game Master. Posicion al iniciar el juego.
        gameMaster = FindObjectOfType<GameMaster>();
        transform.position = gameMaster.lastCPos;
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
        rb.AddForce(Physics.gravity * rb.mass*20);
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
        //Muerte 
        if(life<=0)
        {
            life = 0;
            StartCoroutine(Death());
        }

        //random para las toxinas
        //TOXICITY
        Toxicity();

        //
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, myCamera.transform.forward, out hit, 200))
        //{
        //    if(hit.transform.gameObject.layer == 8)
        //    {
        //        Debug.Log("hay una pared");
        //    }
        //}
    }

    //Toxicity 
    void Toxicity()
    {

        if (toxicity > 50 && toxicity < 80)
        {
            StartCoroutine(view.ToxicitySound());
            toxicityText.text = increaseDamage ? "More damage" : "More speed";
            //mejoras, como mas daño, velocidad, lo que sea y estas se activan al azar
            //que sea por tiempo 
            toxicityText.enabled = true;

            if (!toxicityImprovementsRandom)
            {
                toxicityImprovementsRandom = true;

                var r = Random.Range(0, 100);

                if (r <= 50)
                {
                    
                    Debug.Log("mas daño");
                    increaseDamage = true;
                    if (hasPickUpPistol)
                    {
                        weaponHolder.weapons[0].GetComponent<GunPistol>().damage *= 2;
                    }
                    if (hasPickUpShotgun)
                    {
                        weaponHolder.weapons[1].GetComponent<GunPistol>().damage *= 2;
                    }
                }
                if (r > 50)
                {
                    
                    Debug.Log("mas velocidad");
                    speed = normalSpeed * 2f;
                    runSpeed = speed * 2f;
                }
            }


        }

        if (toxicity > 80 )
        {
            //se tiene que curar el jugador por que esta al borde de empezar a perder vida
            toxicityText.text = $" Toxicity bar has reach max level \n{(increaseDamage ? "More damage" : "More speed")}";
        }
        //resta vida mientras tiene toxinas en el cuerpo
        if (toxicity >= 100)
        {
            timerLifeToxic += Time.deltaTime;
            if (timerLifeToxic > 1)
            {
                timerLifeToxic = 0;
                life--;
                //TakeDamage(1);
                
            }
        }
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
        }

        //sacar la screen toxicity
        if (isInMedicineBox)
        {
            view.toxicityScreen.enabled = false;
        }
    }

    
    //Movimiento del jugador.
    public void Move(float axisHorizontal, float axisVertical)
    {
        Vector3 movement = transform.right * axisHorizontal + transform.forward * axisVertical;
        _directionDebug = movement;
        if (!Physics.Raycast(transform.position, movement, 50, (int)Mathf.Pow(8 * 2,2)))
        {
            rb.MovePosition(transform.position + movement.normalized * speed * Time.deltaTime);
        }
    }
    Vector3 _directionDebug;
    public void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        Gizmos.DrawRay(transform.position, _directionDebug*50);
    }
    public void Run(float axisHorizontal, float axisVertical)
    {
        Vector3 movement = transform.right * axisHorizontal + transform.forward * axisVertical;
        rb.MovePosition(transform.position + movement.normalized * runSpeed * Time.deltaTime);
    }

    //Movimiento de la camara
    public void RotationCamera()
    {
        var hCamera = 120 * Input.GetAxis("Mouse X") * Time.deltaTime;
        var vCamera = Input.GetAxis("Mouse Y") * Time.deltaTime * 120;
        transform.Rotate(0, hCamera, 0);
        myCamera.transform.Rotate(-vCamera, 0, 0);

        if (myCamera.transform.localRotation.eulerAngles.x < 300 && myCamera.transform.localRotation.eulerAngles.x > 270)
        {
            var auxRotation = myCamera.transform.localRotation.eulerAngles;
            auxRotation.x = 300;
            myCamera.transform.localRotation = Quaternion.Euler(auxRotation);
        }
        else if (myCamera.transform.localRotation.eulerAngles.x > 70 && myCamera.transform.localRotation.eulerAngles.x < 90)
        {
            var auxRotation = myCamera.transform.localRotation.eulerAngles;
            auxRotation.x = 70;
            myCamera.transform.localRotation = Quaternion.Euler(auxRotation);
        }
    }



    public void TakeDamage(int damage)
    {
        if (_hasBeenHit) return;
        life -= damage;
        view.DamageFeedback();

        StartCoroutine(Invulnerability());
        StartCoroutine(HitKnockback(3.5f));
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
        weaponHolder.weapons[weaponHolder.actualWeapon].GetComponent<GunPistol>().ShootRaycast();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "GlassFragments")
        {
            rb.velocity = Vector3.zero;

            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
