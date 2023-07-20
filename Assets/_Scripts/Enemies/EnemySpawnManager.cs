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
        [SerializeField] public GameObject enemyPrefab;
        [SerializeField] public float spawnDelay;
        [SerializeField] public int maxSpawn; // Maxima cantidad de enemigos de ese tipo que pueden estar a la vez
        [SerializeField] public float spawnChance; // Posibilidad de que el enemigo spawnee entre las demas opciones
    }

    // Array de los enemigos a spawnear HAY QUE CARGARLOS A MANO EN CADA INSTANCIA DE ESTE SCRIPT**
    [SerializeField] private List<enemyData> enemies;

    // Posiciones donde se van a generar los enemigos
    private float randomSpawnPositionX;
    [SerializeField] private float spawnPositionY;

    // Variables relacionadas con el spawn de enemigos
    private bool canSpawn = true;
    [SerializeField] private int maxEnemiesAtTime;

    // Contenedor de enemigos
    private GameObject enemiesInScene;

    // Temporizador de la ronda actual
    public float LevelTimer; // El contador se inicia de manera MANUAL


    // Otras variables
    private float totalProbability = 0f; // Suma de todas las probabilidades de cada enemigo diferente

    private void Start()
    {
        // Obtenemos el contenedor
        enemiesInScene = GameObject.Find("EnemiesInScene");

        // Actualizamos constantemente el punto en el que aparecen los enemigos
        InvokeRepeating("GetRandomPositionX", 0f, 1f);

        // Obtenemos la probabilidad de todos los enemigos
        GetProbability();
    }
    
    private void Update()
    {
        // Disminuimos al contador el tiempo transcurrido (Y lo truncamos en 0)
        if (LevelTimer >= 0f)
        {
            LevelTimer -= Time.deltaTime;
        }

        // Si la cantidad de enemigos no supera el limite generamos nuevos YY podemos spawnear YY el tiempo no se termino: spawneamos enemigos
        if (enemiesInScene.transform.childCount < maxEnemiesAtTime && canSpawn && LevelTimer > 0)
        {
            // Spawneamos un enemigo
            StartCoroutine(SpawnEnemy());
        }
    }

    private void GetRandomPositionX()
    {
        // Obtenemos un numero aleatorio a una distancia de range
        float range = 11f;
        randomSpawnPositionX = UnityEngine.Random.Range(-range, range);
    }

    private void GetProbability()
    {
        // Por cada enemigo cargado, leemos y almacenamos su posibilidad de spawnear en un total
        totalProbability = 0f;
        foreach (enemyData item in enemies)
        {
            totalProbability += item.spawnChance;
        }
    }

    private T RandomEnemyIndex<T>(List<T> list)
    {
        // En caso de estar vacia o no existir la lista, generamos un error
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("La lista no puede estar vacia");
        }

        // Generamos un número aleatorio entre 0 y la suma total de las probabilidades
        float randomValue = UnityEngine.Random.Range(0f, totalProbability);

        // Iteramos a través de los elementos y seleccionamos el primero cuya probabilidad acumulada sea mayor que el número aleatorio
        float accumulatedProbability = 0f;
        for (int i = 0; i < list.Count; i++)
        {
            float probability = enemies[i].spawnChance;
            accumulatedProbability += probability;
            if (randomValue <= accumulatedProbability)
            {
                return list[i];
            }
        }

        // Si no se seleccionó ningún elemento, se devuelve el primero de la lista
        return list[0];
    }

    /// <summary>
    /// Este metodo se encarga de contar todos los enemigos que tienen el mismo nombre dentro de la escena
    /// </summary>
    /// <param name="targetEnemy"></param>
    /// <returns>Cantidad de enemigos con el mismo nombre</returns>
    private int EnemyCount(GameObject targetEnemy)
    {
        int count = 0;
        // Obtenemos todos los enemigos mediante su tag
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject sceneEnemy in allEnemies)
        {
            // Si los nombres coinciden, agregamos 1 al contador
            if (sceneEnemy.name == targetEnemy.name + "(Clone)")
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Metodo publico encargado de spawnear los enemigos con cierta logica, es publico ya que
    /// ciertos enemigos especiales tienen la capacidad de aparecer enemigos
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnEnemy()
    {
        // Hacemos que no pueda spawnear hasta esperar el delay
        canSpawn = false;

        // Obtenemos un enemigo aleatorio

        enemyData enemy;
        // Si en la escena hay mas o igual enemigos que los maximos de ese tipo permitidos, buscamos otro
        int count = 0; // Variable auxiliar
        do
        {
            enemy = RandomEnemyIndex(enemies);
            // Si por algun motivo no paramos de buscar, devolver nada
            count++;
            if (count >= 3)
            {
                yield return null;
            }
        }
        while (enemy.maxSpawn <= EnemyCount(enemy.enemyPrefab));

        // Lo instanciamos
        GameObject newEnemy = Instantiate(enemy.enemyPrefab, new Vector3(randomSpawnPositionX, spawnPositionY, 0f), enemy.enemyPrefab.transform.rotation);
        // Lo hacemos hijo del contenedor de enemigos
        newEnemy.transform.parent = enemiesInScene.transform;

        // Esperamos el tiempo indicado entre spawns
        yield return new WaitForSeconds(enemy.spawnDelay);
        canSpawn = true;
    }
}