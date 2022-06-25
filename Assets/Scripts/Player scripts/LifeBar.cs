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
    public Image myImageSlow;

    float t = 0;
    //
    float maxLife = 100;

    private void Awake()
    {
        //componente del jugador.
        player = FindObjectOfType<PlayerModel>();

        myImage.fillAmount = 0;
        myImageSlow.fillAmount = 0;
    }

    private void Update()
    {
        myImage.fillAmount = player.life / maxLife;
        myImageSlow.fillAmount = player.lifeSlow / maxLife;

        if(player.lifeSlow != player.life)
        {
            player.lifeSlow = Mathf.Lerp(player.lifeSlow, player.life, t);
            t += 1.0f * Time.deltaTime;
        }
    }
}
