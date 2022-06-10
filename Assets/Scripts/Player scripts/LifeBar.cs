using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    //Componente
    PlayerModel player;

    [Header("BARRA DE VIDA")]
    public Image myImage;

    //
    float maxLife = 100;

    private void Awake()
    {
        //componente del jugador.
        player = FindObjectOfType<PlayerModel>();

        myImage.fillAmount = 0;
    }

    private void Update()
    {
        myImage.fillAmount = player.life / maxLife;
    }
}
