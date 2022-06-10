using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicityZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(!other.gameObject.GetComponent<PlayerModel>().isInMedicineBox)
            {
                other.gameObject.GetComponent<PlayerModel>().timerToxicity += Time.deltaTime;

                if (other.gameObject.GetComponent<PlayerModel>().timerToxicity > 0.07f)
                {
                    other.gameObject.GetComponent<PlayerModel>().toxicity += 1;
                    other.gameObject.GetComponent<PlayerModel>().timerToxicity = 0;
                    other.gameObject.GetComponent<PlayerView>().toxicityScreen.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerView>().toxicityScreen.enabled = false;
        }
    }
}
