using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Arreglo con los prefabs de los enemigos
    public Transform[] spawnPoints; // Arreglo con los puntos de aparición de los enemigos
    public float timeBetweenWaves = 5f; // Tiempo entre cada oleada
    public int numberOfEnemiesPerWave = 5; // Cantidad de enemigos por oleada
    public float enemyIncreaseFactor = 1.1f; // Factor de aumento de dificultad de los enemigos
    public int enemyAmount;
    public int maxEnemyAmount = 5;
    public bool isInWave = false;
    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start()
    {
        isInWave = true;
    }

    private IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        isSpawning = true;
        currentWave++;

        for (int i = 0; i < numberOfEnemiesPerWave; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(enemyPrefabs[randomIndex], spawnPoints[randomSpawnPoint].position, Quaternion.identity);

            enemyAmount += 1;

            yield return new WaitForSeconds(1f); // Tiempo entre cada aparición de enemigo
        }

        isSpawning = false;

        // Aumentar la dificultad para la siguiente oleada
        numberOfEnemiesPerWave = Mathf.RoundToInt(numberOfEnemiesPerWave * enemyIncreaseFactor);

    }

    private void Update()
    {
        if(enemyAmount <= maxEnemyAmount & isInWave == true)
        {
            StartCoroutine(SpawnWave());
            isInWave = false;
        }

        // Comprobar si se han destruido todos los enemigos y no está ocurriendo una oleada actualmente
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !isSpawning)
        {
            // Finalizar el juego o realizar alguna acción adicional
        }
    }
}
