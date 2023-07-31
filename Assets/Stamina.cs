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
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerModel>();
        staminaBar.color = new Color(1, 1, 1, 0);
        staminaBckgrnd.color = new Color(1, 0, 0, 1);


    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;

        staminaBckgrnd.color = Color.HSVToRGB()
        if (player.isRunning)
            staminaBar.fillAmount = Mathf.Lerp(0, 1, player.estamina / 100);

    }
}
