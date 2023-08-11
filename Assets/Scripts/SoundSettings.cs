using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image imageMute;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;
        Check();
    }

    public void ChangeSlider(float v)
    {
        sliderValue = v;
        AudioListener.volume = slider.value;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        Check();
    }

    public void Check()
    {
        if (sliderValue == 0)
        {
            imageMute.enabled = true;
        }
        else
        {
            imageMute.enabled = false;
        }
    }
}
