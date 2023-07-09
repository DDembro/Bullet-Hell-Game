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
        public GameObject enemyPrefab;
        [SerializeField]
        public float spawnDelay;
        [SerializeField]
        public int maxSpawn; // Maxima cantidad de enemigos de ese tipo que pueden estar a la vez
    }

    // Array de los enemigos a spawnear HAY QUE CARGARLOS A MANO EN CADA INSTANCIA DE ESTE SCRIPT**
    [SerializeField]
    private List<enemyData> enemies;

    // Posiciones donde se van a generar los enemigos
    private float randomSpawnPositionX;
    [SerializeField]
    private float spawnPositionY = 16f;

    // Variables relacionadas con el spawn de enemigos
    private bool canSpawn = true;
    private int maxEnemiesAtTime = 5;

    // Contenedor de enemigos
    private GameObject enemiesInScene;

    // Temporizador de la ronda actual
    public float LevelTimer {  get; private set; }

    private void Start()
    {
        // Inicializamos el contador
        LevelTimer = 60f;

        // Obtenemos el contenedor
        enemiesInScene = GameObject.Find("EnemiesInScene");

        // Actualizamos constantemente el punto en el que aparecen los enemigos
        InvokeRepeating("GetRandomPositionX", 0f, 1f);
    }
    
    private void Update()
    {
        // Disminuimos al contador el tiempo transcurrido (Y lo truncamos en 0)
        if (LevelTimer >= 0f)
        {
            LevelTimer -= Time.deltaTime;
        }

        // Si la cantidad de enemigos no supera el limite generamos nuevos, y podemos spawnear, y el tiempo no se termino: spawneamos enemigos
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

    private static T RandomEnemyIndex<T>(List<T> list)
    {
        // En caso de estar vacia o no existir la lista, generamos un error
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("La lista no puede estar vacia");
        }
        // Calculamos un index aleatorio
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

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

    private IEnumerator SpawnEnemy()
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