using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineToxicity : MonoBehaviour
{
    PlayerModel player;

    //
    float timer;
    bool pressed;
    public Animator animator;
    public ParticleSystem ps;
    AudioSource audioSource;
    public GameObject healingEffect;
    public Vector3 interactionPosition;
    private void Start()
    {
        player = FindObjectOfType<PlayerModel>();
        audioSource = GetComponent<AudioSource>();
        if(healingEffect!=null)
            healingEffect.SetActive(false);
    }
    void Interaction(bool pressing)
    {
        
        pressed = pressing;
       

    }
    void OnTriggerEnter(Collider collision)
    {
        player.interaction += Interaction;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "MedicineBox")
        {
            if (pressed)
            {

                player.isInMedicineBox = true;
                RestToxicity();
                animator.SetBool("activate", true);
                if(healingEffect != null)
                    healingEffect.SetActive(true);

                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(audioSource.clip);

            }
            else
            {
                if (healingEffect != null)
                    healingEffect.SetActive(false);

                player.isInMedicineBox = false;
                animator.SetBool("activate", false);
                if (ps != null)
                    ps.Stop();
                audioSource.Stop();
                
                
                

                
            }
            player.GetComponent<PlayerView>().InteractionImage(transform.position + interactionPosition, pressed ? false : true);

        }

        if(collision.gameObject.tag =="Player" && this.gameObject.tag == "MedicineObject")
        {
            if(ps!=null)
                ps.Play();
            player.toxicity = 0;
            audioSource.Play();
            Destroy(gameObject, 0.5f);
            if (ps != null)
                Destroy(ps, 2);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag =="Player" && this.gameObject.tag == "MedicineBox")
        {
            player.interaction -= Interaction;
            if (healingEffect != null)
                healingEffect.SetActive(false);

            player.isInMedicineBox = false;
            animator.SetBool("activate", false);
            if (ps != null)
                ps.Stop();
            audioSource.Stop();
            player.GetComponent<PlayerView>().InteractionImage(transform.position + interactionPosition, false);
        }
    }

    void RestToxicity()
    {
        timer += Time.deltaTime;
        if(timer>0.05f)
        {
            player.toxicity -= 3;
            timer = 0;
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + interactionPosition, 10);
    }

    //se llama en la animacion
    public void StartPSAnimation()
    {
        if (ps != null)
            ps.Play();
    }
}
