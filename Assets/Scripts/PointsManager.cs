﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PointsManager : MonoBehaviour
{
    int Deaths;
    int points;
    int[] kills= new int[3];
    int totalKills;
    int[] pointsPerEnemyType= new int[3] {50,120,200};
    public float finalCombo;
    public float combo;
    float time; 
    float[] comboLimits = new float[5] { 100, 250, 400, 600, 800};
    float[] comboDecreasement = new float[5] { 5, 8, 15, 25, 50 };
    float[] comboMultiplier = new float[5] { 1, 1.3f, 1.5f, 1.8f, 2 };
    public float comboTemp;
    public int currentComboInstance;
    public static PointsManager instance;
    public Slider slider;
    bool isFinished;
    public GameObject canvas;
    public Text DeathText, killText, pointText, timeText, x,xshadow, multiplier,multiplierShadow;
    bool show;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {

            time += Time.deltaTime;
            ChangeIndex();
            if ( combo > 0)
            {
                combo -= Time.deltaTime*comboDecreasement[currentComboInstance];
                if(multiplier && multiplierShadow)
                {
                    multiplier.text=comboMultiplier[currentComboInstance].ToString();
                    multiplierShadow.text=comboMultiplier[currentComboInstance].ToString();
                }
            }
            else
            {
                if (multiplier && multiplierShadow)
                {
                    multiplier.text = "";
                    multiplierShadow.text = "";
                }
            }
            if(x && xshadow)
            {
                x.gameObject.SetActive(combo > 0);
                xshadow.gameObject.SetActive(combo > 0);

            }
            
        
            comboTemp = ExtensionMethods.Remap(combo, currentComboInstance > 0 ? comboLimits[currentComboInstance - 1] :0, comboLimits[currentComboInstance], 0, 1);
            if(slider)
                slider.value = comboTemp;

        }
        if(SceneManager.GetActiveScene().name== "end level cinematic" && !show)
        {
            show = true;
            StartCoroutine(WaitToFinish());
        }
    }

    public void AddDeath()
    {
        Deaths++;
    }
    public void AddKill(int enemyType)
    {
        kills[enemyType]++;
    }
    public void AddCombo(int damage)
    {
        finalCombo += damage*comboMultiplier[currentComboInstance];
        if(combo<comboLimits[comboLimits.Length-1])
            combo += damage;
        
    }
    public void ChangeIndex()
    {
        if (combo > comboLimits[currentComboInstance] && currentComboInstance<comboLimits.Length-1)
            currentComboInstance++;
        else if (currentComboInstance > 0)
        {
            if(combo < comboLimits[currentComboInstance - 1])
            {
                Debug.Log(combo + " es menor a " + comboLimits[currentComboInstance - 1]);
                
                currentComboInstance--;

            }
        }
    }
    public void Finish()
    {
        isFinished = true;
        CalculatePoints();
    }
    public IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(1.6f);
        Finish();
    }
    public void CalculatePoints()
    {
        canvas.SetActive(true);
        float _points = 0;
        for (int i = 0; i < kills.Length; i++)
        {
            for (int x = 0; x < kills[i]; x++)
            {
                totalKills++;
                _points += pointsPerEnemyType[i];
            }
        }
        _points -= Deaths * 100;

        if ((time / 60) > 5)
            _points -= time;

        points = (int)_points;
        DeathText.text += Deaths;
        killText.text += totalKills;
        string temp = (time / 60).ToString("n2");
        temp.Replace(',', ':');
        timeText.text += temp;
        pointText.text += points;

    }
}
public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}