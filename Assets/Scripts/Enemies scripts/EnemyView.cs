using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyView : MonoBehaviour
{
    //feedback de los enemigos

    //Componentes
    public Animator animator;
    AudioSource audioSource;
    public GameObject head;
    public GameObject headExplosion;
    [Header("Sonidos")]
    public List<AudioClip> myClips = new List<AudioClip>();
    GameObject _lastHeadExplosion;
    public GameObject lastHeadExplosion { get => _lastHeadExplosion; }
    public Grid grid;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    //Funciones de animaciones
    public void WalkingAnimation()
    {
        animator.SetBool("walking", true);
        animator.SetBool("idle", false);
        animator.SetBool("chasing", false);
        animator.SetBool("distanceAttack", false);
    }

    public void ChasingAnimation()
    {
        animator.SetBool("chasing", true);
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
       
        animator.SetBool("distanceAttack", false);
    }

    public void IdleAnimation()
    {
        animator.SetBool("walking", false);
        animator.SetBool("idle", true);
        animator.SetBool("chasing", false);
        animator.SetBool("distanceAttack", false);
    }
    public void AttackAnimation()
    {

        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
        animator.SetTrigger("attacking");
        animator.SetBool("chasing", false);
        animator.SetBool("distanceAttack", false);
    }
    public void DeathAnimation()
    {
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);

        animator.SetBool("chasing", false);
        animator.SetBool("death", true);
        animator.SetBool("distanceAttack", false);
    }
    public void SetAnimator(bool value)
    {
        animator.enabled = value;

    }
    public void DistanceAttackAnimation()
    {
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);

        animator.SetBool("chasing", false);
        animator.SetBool("death", false);
        animator.SetTrigger("distanceAttack");
    }
    public void WaitForCharge()
    {
        animator.SetBool("walking", false);
        animator.SetBool("Preparation", true);
       
        animator.SetBool("chasing", false);
        animator.SetBool("charging", false);
        animator.SetBool("death", false);
    }
    public void Charge()
    {
        animator.SetBool("walking", false);
        animator.SetBool("Preparation", false);

        animator.SetBool("chasing", false);
        animator.SetBool("charging", true);
        animator.SetBool("death", false);
    }
    public void DestroyHead()
    {

        if (!head.activeSelf) return;
        _lastHeadExplosion= Instantiate(headExplosion,head.transform.position,head.transform.rotation,head.GetComponent<SkinnedMeshRenderer>().rootBone);
        head.SetActive(false);
    }
    public void Stunned()
    {
        animator.SetTrigger("stun");
    }

    //Funciones de sonidos 
    public void DamageSound()
    {
        audioSource.PlayOneShot(myClips[0]);
    }
    public void DeathSound()
    {
        audioSource.PlayOneShot(myClips[1]);
    }
    public void Step()
    {
        audioSource.PlayOneShot(myClips[2]);
    }
    public void AttackSound()
    {
        audioSource.PlayOneShot(myClips[3]);
    }
    public void OnDrawGizmos()
    {
        
    }
}
