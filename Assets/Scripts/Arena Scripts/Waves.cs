﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Arreglo con los prefabs de los enemigos
    public Transform[] spawnPoints; // Arreglo con los puntos de aparición de los enemigos
    public float timeBetweenWaves = 5f; // Tiempo entre cada oleada
    public float enemyIncreaseFactor = 1.1f; // Factor de aumento de dificultad de los enemigos
    public int enemyAmount;
    public int maxEnemyAmount;
    public bool isInWave = false;
    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start()
    {
        isInWave = false;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SpawnWave());
            isInWave = true;

        }
    }
}
