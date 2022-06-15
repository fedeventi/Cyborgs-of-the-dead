﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GunPistol : Weapon
{
    //Variables 
    //Componentes 
    [Header("Componentes")]
    public Camera myCamera;
    PlayerModel model;
    public Transform forwardWeapons;

    [Header("UI")]
    public Text ammoTextCurrent;
    public Text ammoTextTotal;

    //Ammo
    [Header("Ammo")]
    public int maxClip;
    public int maxAmmo;
    int currentAmmo;
    public int currentMaxAmmo;
    //
    int countBullets = 0;
    //flotante dependiendo del arma, para terminar la animacion de recarga
    public float stopReloadAnimation;

    //Shoot
    [Header("Shoot Shake")]
    public float shake;
    [Header("Particle shoot")]
    public GameObject muzzleFlashObject;
    public ParticleSystem muzzleFlash;
    [Header("Bullet hole effect")]
    public GameObject bulletHole;
    public GameObject bulletDecal;
    [Header("Bullet")]
    public GameObject lineBullet;
    public Collider gunSoundArea;

    //Sonidos
    [Header("Sonidos")]
    public List<AudioClip> myClips = new List<AudioClip>();
    AudioSource audioSource;
    public float nextReloadSound = 0f;
    public float delayReloadSound = 3f;

    [Header("DAMAGE")]
    public int damage;
    public int startDamage;
    public bool onZoneMoreDamageShotgun = false;

    [Header("TIMER TO SHOOT AGAIN")]
    public float timerToShootAgain;

    //
    

    //Recoil
    [Header("RECOIL")]
    public float recoilXAxis;
    public float recoilYAxis;
    Vector3 recoilRemaining;

    //
    bool explosionForce = false;

    

    void Start()
    {
        model = FindObjectOfType<PlayerModel>();

        //Municion al principio del juego.
        currentAmmo = maxClip;
        currentMaxAmmo = maxAmmo;

        //Audio source 
        audioSource = GetComponent<AudioSource>();

        //damage
        startDamage = damage;

        //
        muzzleFlashObject.SetActive(false);

    }


    void Update()
    {
        //Actualiza el texto de la munición.
        if(ammoTextCurrent != null && ammoTextTotal!=null) 
        { 
            ammoTextCurrent.text = currentAmmo.ToString();
            ammoTextTotal.text = currentMaxAmmo.ToString();
        }
        //Shoot
        if (Input.GetMouseButtonDown(0) && currentAmmo>0)
        {
            ShootNoAmmo();
        }
        else if(Input.GetMouseButtonDown(0) && currentAmmo <= 0)
        {
            NoAmmo();
        }


        //Recargar
        if (currentAmmo == 0 || Input.GetKey(KeyCode.R) && currentAmmo<maxClip)
        {
            if(currentMaxAmmo>0)
            {
                StartCoroutine(Reload());
            }
        }
        if (model.isReloading)
        {
            return;
        }

        //AIM
        //Aim();

        //contador de balas, para recargar
        if(currentAmmo==maxClip)
        {
            countBullets = 0;
        }

        //damage 
        if (!model.increaseDamage && !onZoneMoreDamageShotgun) 
        {
            damage = startDamage;
        }

    }
    //Aim
    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
             Debug.Log("aiming");
        }
        //Animacion
    }

    //Recargar.
    IEnumerator Reload()
    {
        if(!model.isShooting)
        {
            model.isReloading = true;

            if (nextReloadSound <= 0)
            {
                audioSource.PlayOneShot(myClips[1], 0.2f);
                nextReloadSound += delayReloadSound;
            }
            animator.SetBool("reloading", true);
            animator.SetBool("idle", false);
            animator.SetBool("walking", false);
            animator.SetBool("running", false);
            yield return new WaitForSeconds(stopReloadAnimation);
            animator.SetBool("reloading", false);
            yield return new WaitForSeconds(stopReloadAnimation + 0.1f);

            nextReloadSound = 0;

            if (currentMaxAmmo >= maxClip && model.isReloading)
            {
                model.isReloading = false;
                currentMaxAmmo -= countBullets;
                currentAmmo = maxClip;
            }
            else if (currentAmmo < maxClip && model.isReloading)
            {
                model.isReloading = false;
                currentAmmo = currentMaxAmmo;
                currentMaxAmmo = 0;
            }

            yield break;
        }
    }

    //Funcion de disparo, produce la animacion de este
    public override void ShootNoAmmo()
    {
        if (!model.isShooting && !model.isReloading)
        {
            model.isShooting = true;
            model.animationShooting = true;
            StartCoroutine(BoolShoot());
            
            ShootSound();

            animator.SetTrigger("shoot");
            animator.SetBool("idle", false);
            animator.SetBool("walking", false);
            animator.SetBool("running", false);
        }


        if (currentAmmo == 0)
        {
            NoAmmo();
        }
    }
    //funcion disparo con raycast + efectos. //se llama en el playerModel, en cierto momento de la animacion
    public override void Shoot()
    {
        muzzleFlashObject.SetActive(true);
        StartCoroutine(MuzzleFlash());

        currentAmmo--;
        countBullets++;

        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward,out hit,5000,collisionMask))
        {
            //Si el hit es BaseEnemy, ejecuto el daño de este enemigo.
            var target = hit.transform.GetComponent<BaseEnemy>();
            if (target != null && !target.isDead)
            {
                target.hasTouchBullet = true;
                target.TakeDamage(damage);
                var bloodEffect = Instantiate(target.bloodSpray, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffect, 0.5f);
            }
            //Si el hit es el headshot, el enemigo muere al instante.
            var headShot = hit.transform.GetComponent<HeadShot>();
            if (headShot != null && headShot.myEnemy.tag == "NormalEnemy"&&!headShot.myEnemy.isDead)
            {
                headShot.myEnemy.life = 0;
                headShot.myEnemy.CloseEnemies();
                var bloodE = Instantiate(headShot.myEnemy.bloodSpray, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodE, 0.5f);
            }
            if (headShot != null && headShot.myEnemy.tag == "ZFEnemy" && !headShot.myEnemy.isDead)
            {
                headShot.myEnemy.life -= damage*2;
                headShot.myEnemy.CloseEnemies();
                var bloodE = Instantiate(headShot.myEnemy.bloodSpray, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodE, 0.5f);
            }
            //Layer del escanario. Si colisiona con algun objeto, genera el bullet hole
            if (hit.transform.gameObject.layer == 8)
            {
                var impactEffect = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                if (hit.transform.gameObject.tag != "GlassFragments")
                {
                    var decal = Instantiate(bulletDecal, hit.point, Quaternion.LookRotation(hit.normal));

                }
                
                
            }
            //el hit es la explosion del enemigo explosivo
            if(hit.transform.gameObject.tag=="ExplosionZoneZE" && !explosionForce)
            {
                if(hit.transform.GetComponent<ExplosionZoneZE>().myEnemy.life>0)
                {
                    explosionForce = true;
                    hit.transform.GetComponent<ExplosionZoneZE>().myEnemy.life = 0;
                    hit.transform.GetComponent<ExplosionZoneZE>().hasCollision = true;
                    var p = Instantiate(hit.transform.GetComponent<ExplosionZoneZE>().myEnemy.particleSystemExplosion, hit.transform.GetComponent<ExplosionZoneZE>().myEnemy.psPosition);
                    Destroy(p, 1f);
                }

                //StartCoroutine(StopExplosion());
                //trabajar en esto
                myCamera.GetComponent<ShakeCamera>().ActivateShake(2f);
            }

            
        }

        myCamera.GetComponent<ShakeCamera>().ActivateShake(shake);
        Recoil();
        SpawnLineRenderer(hit.point);
    }

    //funcion para los fragmentos de vidrios de las barricadas
    

    //corrutina para parar el movimiento de la explosion
    IEnumerator StopExplosion()
    {
        model.transform.GetComponent<Rigidbody>().AddExplosionForce(100, hit.transform.position, 10);
        model.speed = 0;

        yield return new WaitForSeconds(0.3f);

        model.transform.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(0.5f);

        model.speed = model.normalSpeed;
        model.transform.GetComponent<Rigidbody>().isKinematic = false;
        explosionForce = false;

        yield break;
    }

    //Recoil
    void Recoil()
    {
        recoilRemaining.x = Random.Range(-recoilXAxis, recoilXAxis);
        recoilRemaining.y = Random.Range(0f, -recoilYAxis);

        model.transform.Rotate(Vector3.up, recoilRemaining.x);
        myCamera.transform.Rotate(Vector3.right, recoilRemaining.y);
    }
    IEnumerator MuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(0.15f);
        muzzleFlash.Stop();
        muzzleFlashObject.SetActive(false);
        yield break;
    }

    IEnumerator BoolShoot()
    {
        gunSoundArea.enabled = true;
        yield return new WaitForSeconds(0.4f);
        model.animationShooting = false;
        gunSoundArea.enabled = false;
        yield return new WaitForSeconds(timerToShootAgain);
        model.isShooting = false;
        yield break;
    }

    

    //line renderer para disparo
    void SpawnLineRenderer(Vector3 hitPoint)
    {
        GameObject bulletLineEffect = Instantiate(lineBullet.gameObject, forwardWeapons.position, lineBullet.transform.rotation);
        LineRenderer lineR = bulletLineEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, forwardWeapons.position);
        lineR.SetPosition(1, hitPoint);


        Destroy(bulletLineEffect, 1f);
    }

    //SONIDOS EN ANIMACIONES.

    //Sonido de recarga.
    public void ReloadSound()
    {
        audioSource.PlayOneShot(myClips[1]);
    }
    //Sonido de disparo.
    public void ShootSound()
    {
        audioSource.PlayOneShot(myClips[0]);
    }

    public void NoAmmo()
    {
        audioSource.PlayOneShot(myClips[2]);
    }

}






