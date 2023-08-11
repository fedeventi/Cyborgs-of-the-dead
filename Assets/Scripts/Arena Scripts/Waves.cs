using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Waves : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();// Arreglo con los prefabs de los enemigos
    public GameObject[] walls;
    public GameObject[] coleccionables;
    public GameObject zombieBrazo;
    public GameObject zombieDist;
    public GameObject zombieTank;
    public Transform[] spawnCol;
    public Transform[] spawnPoints;
    public Transform[] spawnPoints2;
    public Transform[] spawnPoints3; // Arreglo con los puntos de aparición de los enemigos
    public float timeBetweenWaves = 5f; // Tiempo entre cada oleada
    public float enemyIncreaseFactor = 1.1f; // Factor de aumento de dificultad de los enemigos
    public int enemyAmount;
    public int maxEnemyAmount;
    public bool isInWave = false;
    public int currentWave = 0;
    private bool isSpawning = false;

    public Text enemyAccText;
    public Text waveText;

    private void Start()
    {
        isInWave = false;
        maxEnemyAmount = 10;
    }

    private void Update()
    {
        if (enemyAmount > 1)
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }

        if (currentWave >= 5)
        {
            enemyPrefabs.Add(zombieBrazo);
        }

        if (currentWave >= 8)
        {
            enemyPrefabs.Add(zombieDist);
        }

        if (currentWave >= 11)
        {
            enemyPrefabs.Add(zombieTank);
        }

        enemyAccText.text = enemyAmount.ToString();
        waveText.text = currentWave.ToString();
    }

    public IEnumerator SpawnColect()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < spawnCol.Length; i++)
        {
            int randomIndex = Random.Range(0, coleccionables.Length);
            int randomSpawnPoint = Random.Range(0, spawnCol.Length);

            Instantiate(coleccionables[randomIndex], spawnCol[randomSpawnPoint].position, spawnCol[randomSpawnPoint].rotation);
        }
    }

    public IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(SpawnColect());

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }

    public IEnumerator SpawnWave2()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            int randomSpawnPoint = Random.Range(0, spawnPoints2.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints2[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }

    public IEnumerator SpawnWave3()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            int randomSpawnPoint = Random.Range(0, spawnPoints3.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints3[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }
}
