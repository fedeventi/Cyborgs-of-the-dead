using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : Weapon
{
    [Header("Componentes")]
    public Camera myCamera;

    [Header("DAMAGE")]
    public int damage=30;

    //Bool
    public bool isAttacking = false;
    bool combo;
    [Header("UI")]
    public Text ammoText;
    [Header("PARTICULA ")]
    public GameObject particle;
    public TimeManager timeManager;
    string shootString;

    //
    [Header("SONIDOS")]
    public List<AudioClip> clips = new List<AudioClip>();
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Actualiza el texto de la munición.
        if (ammoText != null)
        {
            ammoText.text = "";
        }

        if(timeManager ==null) timeManager =FindObjectOfType<TimeManager>();

        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                Shoot();
                combo=!combo;
                if(audioSource != null)
                    audioSource.PlayOneShot(clips[0]);
            }
        }
    }


    
    
    public override void Shoot()
    {
        
        
        isAttacking = true;
        shootString = combo?"attack":"secondAttack";
        if (!combo) Debug.Log("secondAttack");
        animator.SetTrigger(shootString);
        animator.SetBool("idle", false);
        animator.SetBool("walking", false);
        animator.SetBool("running", false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if(audioSource!=null)
                audioSource.PlayOneShot(clips[1]);
            
            var target = other.transform.GetComponent<BaseEnemy>();
            bool headshot = other.transform.gameObject.tag == "headshot";
            var _damage = headshot ? damage * 3 : damage;
            
            if (headshot)
            {
                target = other.transform.GetComponentInParent<BaseEnemy>();
            }
            if (target != null )
            {
                if(!target.isDead)
                    target.TakeDamage(_damage,headshot);
                var bloodEffect = Instantiate(target.bloodSpray, myCamera.transform.position+myCamera.transform.forward*50,Quaternion.LookRotation(transform.forward*-1));
                //Destroy(bloodEffect, 0.5f);
            }
        }
        if(other.gameObject.layer == 8)
        {
            var _myParticle=Instantiate(particle, myCamera.transform.position + myCamera.transform.forward * 50, transform.rotation);
            _myParticle.transform.localScale = Vector3.one*10;

        }

        if (other.transform.gameObject.tag == "GlassFragments")
        {
            var _myParticle = Instantiate(particle, myCamera.transform.position + myCamera.transform.forward * 50, transform.rotation);
            
            var obj = other.GetComponent<BreakGlass>();
            if (obj == null) return;
            obj.ReplaceGlass();
            //sonido de los vidrios rompiendose 
            if(audioSource!=null)
                audioSource.PlayOneShot(clips[2]);
            Destroy(obj.gameObject);
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(myCamera.transform.position, myCamera.transform.forward*50);
    }
    
}
