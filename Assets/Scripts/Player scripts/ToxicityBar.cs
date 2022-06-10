using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToxicityBar : MonoBehaviour
{
    //Componente
    PlayerModel player;

    [Header("BARRA DE TOXICIDAD")]
    public Image myImage;

    //
    float maxToxicity = 100;

    private void Awake()
    {
        //componente del jugador.
        player = FindObjectOfType<PlayerModel>();

        myImage.fillAmount = 0;
    }

    private void Update()
    {
        myImage.fillAmount = player.toxicity / maxToxicity;
    }
}
