using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    PlayerModel player;
    public Image staminaBar, staminaBckgrnd;
    Color _BckgrndHSV;
    Color _BckgrndRGB;
    float _breath;
    // Start is called before the first frame update
    float map(float fromA, float toA, float fromB, float ToB, float value)
    {
        return fromB + (value - fromA) * (ToB - fromB) / (toA - fromA);
    }
    void Start()
    {
        player = FindFirstObjectByType<PlayerModel>();
        staminaBar.color = new Color(1, 1, 1, 0);
        staminaBckgrnd.color = new Color(1, 0, 0, 1);


    }


    // Update is called once per frame
    void Update()
    {
        _breath += Time.deltaTime;
        if (!player) return;


        staminaBckgrnd.color = Color.HSVToRGB(0.016f, 1, map(0, 1, 0.35f, 0.80f, (Mathf.Cos(_breath * 5) / 2) + 0.5f));
        if (player.isRunning)
            staminaBar.fillAmount = Mathf.Lerp(0, 1, player.estamina / 100);

    }
}
