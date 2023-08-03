using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public GameObject[] enemyPrefabs;// Arreglo con los prefabs de los enemigos
    public GameObject[] walls;
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

    private void Start()
    {
        isInWave = false;
    }

    private void Update()
    {
        if(enemyAmount > 1)
        {
            for(int i = 0;i <= walls.Length ; i++)
            {
                walls[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i <= walls.Length; i++)
            {
                walls[i].SetActive(false);
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }

    private IEnumerator SpawnWave2()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            int randomSpawnPoint = Random.Range(0, spawnPoints2.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints2[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }

    private IEnumerator SpawnWave3()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < maxEnemyAmount; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            int randomSpawnPoint = Random.Range(0, spawnPoints3.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints3[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;
        isInWave = false;
        maxEnemyAmount += 3;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            StartCoroutine(SpawnWave());
            isInWave = true;
        }

        if (other.gameObject.layer == 17)
        {
            StartCoroutine(SpawnWave2());
            isInWave = true;
        }

        if (other.gameObject.layer == 18)
        {
            StartCoroutine(SpawnWave3());
            isInWave = true;
        }
    }
}
