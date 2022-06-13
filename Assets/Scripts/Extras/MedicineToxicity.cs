using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineToxicity : MonoBehaviour
{
    PlayerModel player;

    //
    float timer;

    public Animator animator;
    public ParticleSystem ps;
    private AudioSource audioSource;

    private void Start()
    {
        player = FindObjectOfType<PlayerModel>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.tag == "MedicineBox")
        {
            player.isInMedicineBox = true;
            RestToxicity();
            animator.SetBool("activate", true);
            //audioSource.Play();
            if(!audioSource.isPlaying)
                audioSource.PlayOneShot(audioSource.clip);
        }

        if(collision.gameObject.tag =="Player" && this.gameObject.tag == "MedicineObject")
        {
            ps.Play();
            player.toxicity = 0;
            audioSource.Play();
            Destroy(gameObject, 0.5f);
            Destroy(ps, 2);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag =="Player" && this.gameObject.tag == "MedicineBox")
        {
            player.isInMedicineBox = false;
            animator.SetBool("activate", false);
            ps.Stop();
            audioSource.Stop();
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

    //se llama en la animacion
    public void StartPSAnimation()
    {
        ps.Play();
    }
}
