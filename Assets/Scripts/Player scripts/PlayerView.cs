using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //animaciones, sonidos, feedback general

    //Variables
    //Componentes:
    [Header("Componentes")]
    public Animator animator;
    PlayerModel model;

    //Sonido 
    [Header("Sonidos")]
    public List<AudioClip> audioClips = new List<AudioClip>();
    public float nextStepSound = 0f;
    public float delayStepSound = 1f;
    AudioSource audioSource;

    [Header("Daño")]
    public List<Image> damageImage = new List<Image>();
    [Header("Stunned")]
    public Image stunnedScreen;
    [Header("Toxicity screen")]
    public Image toxicityScreen;

    //
    bool changeHitSound = false;

    private void Start()
    {
        //Componentes
        audioSource = GetComponent<AudioSource>();
        model = GetComponent<PlayerModel>();
    }

    private void Update()
    {
        //screen de daño en el casco
        DamageImages();
    }

    //Movimiento
    public void MovementAnimation(float auxAxisV, float auxAxisH)
    {
        if(!model.isReloading && !model.animationShooting)
        {
            if (auxAxisV != 0 || auxAxisH != 0)
            {
                animator.SetBool("walking", true);
                animator.SetBool("idle", false);
                animator.SetBool("running", false);
            }
            else if (auxAxisH == 0 || auxAxisV == 0)
            {
                animator.SetBool("walking", false);
                animator.SetBool("idle", true);
            }
        }
    }

    public void RunningAnimation(float auxAxisV, float auxAxisH)
    {
        if (!model.isReloading && !model.animationShooting)
        {
            if (auxAxisV != 0 || auxAxisH != 0)
            {
                animator.SetBool("running", true);
                animator.SetBool("idle", false);
                animator.SetBool("walking", false);
            }
            else if (auxAxisH == 0 || auxAxisV == 0)
            {
                animator.SetBool("running", false);
                animator.SetBool("idle", true);
            }
        }
    }
    //Daño 
    public void DamageFeedback()
    {
        if(!changeHitSound)
        {
            changeHitSound = true;
            var r = Random.Range(0, 3);
            audioSource.PlayOneShot(audioClips[r]);

            StartCoroutine(HitSound());
        }
    }

    IEnumerator HitSound()
    {
        yield return new WaitForSeconds(0.1f);
        changeHitSound = false;
        yield break;
    }

    public IEnumerator ToxicitySound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(audioClips[5], 0.4f);
        yield return new WaitForSeconds(0.5f);
        yield break;
    }

    void DamageImages()
    {
        if(model.life<35)
        {
            foreach (var d in damageImage.GetRange(0, damageImage.Count))
            {
                d.enabled = true;
            }
            damageImage[0].enabled = true;
            damageImage[1].enabled = true;
            damageImage[2].enabled = true;
        }
        if(model.life>=35 && model.life<70)
        {
            damageImage[0].enabled = true;
            damageImage[1].enabled = true;
            damageImage[2].enabled = false;
        }
        if(model.life>=70 && model.life<85)
        {
            damageImage[0].enabled = true;
            damageImage[1].enabled = false;
            damageImage[2].enabled = false;
        }
        //
        if (model.life >= 85)
        {
            foreach (var d in damageImage.GetRange(0, damageImage.Count))
            {
                d.enabled = false;
            }
        }
    }
    //Pantalla de stun
    public IEnumerator StunScreen()
    {
        stunnedScreen.enabled = true;
        yield return new WaitForSeconds(2f);
        stunnedScreen.enabled = false;
        yield break;
    }
    //Muerte 
    public void DeathFeedback()
    {
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(audioClips[3], 0.2f);
    }

    //Sonido de los pasos
    //se llama en animaciones de caminata y correr
    public void StepSound()
    {
        audioSource.PlayOneShot(audioClips[4], 0.02f);
    }
}

