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
    bool cough;
    public AudioClip heartBeat;
    
    public float nextStepSound = 0f;
    public float delayStepSound = 1f;
    AudioSource audioSource;
    //Casco
    public Image casco;
    [Range(0f, 1f)]
    public float toxicityLevel;
    [Range (0f, 1f)]
    public float hitValue;
    [Header("Daño")]
    public List<Image> damageImage = new List<Image>();
    [Header("Stunned")]
    public Image stunnedScreen;
    [Header("Toxicity screen")]
    public Image toxicityScreen;
    public Sprite[] toxicityEffects=new Sprite[2];
    public Image mytoxicityEffect ;
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
    public void ChangeSprite(int effect)
    {
        if (mytoxicityEffect == null) return; 
        if (effect == 0) mytoxicityEffect.gameObject.SetActive(false);
        else mytoxicityEffect.gameObject.SetActive(true);
        mytoxicityEffect.sprite=toxicityEffects[effect];
    }
    public void RunningAnimation(bool running)
    {
        if (!model.isReloading && !model.animationShooting)
        {
            animator.SetBool("running", running);
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
    public void LowLife(float lowLife)
    {
        casco.material.SetInt("_LowLife", lowLife<20 ? 1 : 0);
        if (lowLife < 20) 
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(heartBeat, 10);
    }
    IEnumerator HitSound()
    {
        yield return new WaitForSeconds(0.1f);
        changeHitSound = false;
        yield break;
    }
    
    public IEnumerator ToxicitySound()
    {
        if (cough) yield break;
        if (!audioSource.isPlaying)
        {
            cough = true;
            audioSource.PlayOneShot(audioClips[5], 3f);
        }
        yield return new WaitForSeconds(Random.Range(5, 16));
        cough = false;

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
        hitValue = 0;
        animator.SetBool("Death", true);
        casco.material.SetFloat("_hitValue", hitValue);
    }
    public void Toxic(bool bValue)
    {
        if (bValue)
            toxicityLevel += Time.deltaTime;
        else
            toxicityLevel -= Time.deltaTime;

        toxicityLevel = Mathf.Clamp01(toxicityLevel);
        casco.material.SetFloat("_ToxicityValue", toxicityLevel);
    }
    public IEnumerator hitFeedback()
    {
        bool _endCycle=false;
        float _value=1;
        while (!_endCycle)
        {
            _value -= Time.deltaTime*2;
            yield return new WaitForSeconds(.01f);
            casco.material.SetFloat("_hitValue", _value);
            if(_value<=0)
                _endCycle = true;
        }
        
     
       
    }
    //Sonido de los pasos
    //se llama en animaciones de caminata y correr
    public void StepSound()
    {
        audioSource.PlayOneShot(audioClips[4], 0.02f);
    }
}

