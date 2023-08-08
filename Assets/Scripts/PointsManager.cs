using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PointsManager : MonoBehaviour
{
    int Deaths; //tus muertes   
    int points; //puntos acumulados 
    int[] kills = new int[4]; // muertes de zombies de cada tipo 
    int totalKills; //total de muertes de zombies 
    int[] pointsPerEnemyType = new int[4] { 50, 120, 200, 250 }; //puntos que da cada tipo de enemigo 
    float finalCombo;
    float combo; //acumulacion de combo para subir el bonificador
    float time;
    float[] comboLimits = new float[5] { 100, 250, 400, 600, 800 }; //limite para cada etapa de bonificacion
    float[] comboDecreasement = new float[5] { 15, 23, 30, 45, 60 };  //velocidad en la que decae el combo en cada etapa 
    float[] comboMultiplier = new float[5] { 1, 1.3f, 1.5f, 1.8f, 2 }; //multiplicador en la cantidad de puntos que se consigue en cada etapa de combo
    float comboTemp;
    int currentComboInstance;
    public static PointsManager instance;
    public Slider slider;
    bool isFinished;
    public GameObject canvas;
    public Text DeathText, killText, pointText, timeText, multiplier, multiplierShadow;
    public GameObject sldrPfb, cmbPfb, cmbShdwPfb;
    bool show;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        if (!slider)
        {
            slider = Instantiate(sldrPfb).GetComponent<Slider>();
            slider.transform.SetParent(canvas.transform);
            slider.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            slider.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        }

        if (!multiplier)
        {
            multiplier = Instantiate(cmbPfb).GetComponent<Text>();
            multiplier.transform.SetParent(canvas.transform);
            multiplier.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            multiplier.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        }
        if (!multiplierShadow)
        {
            multiplierShadow = Instantiate(cmbShdwPfb).GetComponent<Text>();
            multiplierShadow.transform.SetParent(canvas.transform);
            multiplierShadow.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            multiplierShadow.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {

            time += Time.deltaTime;
            ChangeIndex();
            if (combo > 0)
            {
                combo -= Time.deltaTime * comboDecreasement[currentComboInstance];
                if (multiplier && multiplierShadow)
                {
                    multiplier.text = comboMultiplier[currentComboInstance].ToString();
                    multiplierShadow.text = comboMultiplier[currentComboInstance].ToString();
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



            comboTemp = ExtensionMethods.Remap(combo, currentComboInstance > 0 ? comboLimits[currentComboInstance - 1] : 0, comboLimits[currentComboInstance], 0, 1);
            if (slider)
                slider.value = comboTemp;

        }
        if (SceneManager.GetActiveScene().name == "end level cinematic" && !show)
        {
            show = true;
            StartCoroutine(WaitToFinish());
        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
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
        finalCombo += damage * comboMultiplier[currentComboInstance];
        if (combo < comboLimits[comboLimits.Length - 1])
            combo += damage;

    }
    public void ChangeIndex()
    {
        if (combo > comboLimits[currentComboInstance] && currentComboInstance < comboLimits.Length - 1)
            currentComboInstance++;
        else if (currentComboInstance > 0)
        {
            if (combo < comboLimits[currentComboInstance - 1])
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