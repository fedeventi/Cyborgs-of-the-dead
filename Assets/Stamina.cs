using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    PlayerModel player;
    public Image staminaBar;
    public Color normalColor;
    public Color critColor;
    [Range(0, 100)]
    public float changeRed;

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



    }


    // Update is called once per frame
    void Update()
    {
        _breath += Time.deltaTime;
        if (!player) return;
        LerpColor(player.stamina);
        MakeVisible(staminaBar, player.isRunning || player._stamRcvng);
        BarFillAmount();



    }
    float _visValue = 1;
    void MakeVisible(Image img, bool visible)
    {
        if (visible)
            _visValue += _visValue < 1 ? Time.deltaTime * 4 : 0;
        else
            _visValue -= _visValue > 0 ? Time.deltaTime * 4 : 0;

        normalColor = new Color(normalColor.r, normalColor.g, normalColor.b, _visValue);
    }
    void BarFillAmount()
    {
        staminaBar.fillAmount = Mathf.Lerp(0, 1, player.stamina / 100);
    }
    void LerpColor(float value)
    {
        var newValue = map(40, 36, 0, 1, value);
        if (!player.exhausted)
            staminaBar.color = Color.Lerp(normalColor, critColor, newValue);
    }
    void Breath(Image img)
    {
        img.color = Color.HSVToRGB(0.016f, 1, map(0, 1, 0.35f, 0.80f, (Mathf.Cos(_breath * 5) / 2) + 0.5f));
    }
}
