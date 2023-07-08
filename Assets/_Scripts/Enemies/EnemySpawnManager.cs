using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // Struct que guarda cada enemigo y su respectivos criterios para su spawn
    [Serializable]
    private struct enemyData
    {
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private float spawnDelay;
    }

    // Array de los enemigos a spawnear HAY QUE CARGARLOS A MANO EN CADA INSTANCIA DE ESTE SCRIPT**
    [SerializeField]
    private List<enemyData> enemies;

    // Posiciones donde se van a generar los enemigos
    [SerializeField]
    private float spawnPositionX = 0f;
    private float randomSpawnPositionX;
    [SerializeField]
    private float spawnPositionY = 16f;

    // Temporizador de la ronda actual
    public float LevelTimer {  get; private set; }

    private void Start()
    {
        // Inicializamos el contador
        LevelTimer = 120f;

        // Actualizamos constantemente el punto en el que aparecen los enemigos
        InvokeRepeating("GetRandomPositionX", 0f, 1f);
    }
    
    private void Update()
    {
        // Disminuimos al contador el tiempo transcurrido
        LevelTimer -= Time.deltaTime;
    }

    private void GetRandomPositionX()
    {
        // Obtenemos un numero aleatorio a una distancia de range
        float range = 11f;
        randomSpawnPositionX = UnityEngine.Random.Range(-range, range);
    }

    private void SpawnEnemy()
    {

    }

    private IEnumerator WaitSpawn(float time)
    {
        // Esperamos el tiempo indicado entre spawns
        yield return new WaitForSeconds(time);
    }
}