using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Esta clase es la encargada de Spawnear del jefe especificado, de controlar el HUD correspondiente a dicho jefe
/// y de terminar el nivel cuando el tiempo haya terminado Y no queden enemigos
/// </summary>
public class BossUIController : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;
    // Obtenemos el GamePlayUI
    private GamePlayUI gamePlayUI;

    // Referencia al Spawner de enemigos
    private EnemySpawnManager enemySpawnManager;

    // Obtenemos los elementos que queremos modificar del UI
    private ProgressBar bossBar;


    // Variables del jefe a instanciar
    [SerializeField] private GameObject bossPrefab; // Prefab (Cargado a mano)
    private GameObject bossGameObject; // El autentico jefe del nivel

    [SerializeField] private string bossName; // Nombre (Cargado a mano)
    private EnemyController bossEnemyController; // Script EnemyController del jefe
    private bool isInBossFight = false; // Indicador de inicio de combate contra el jefe


    // Variables adicionales:
    private bool win = false;

    // Contenedor de enemigos del nivel
    private GameObject enemiesInScene;

    private void OnEnable()
    {
        // Obtenemos las referencias
        enemySpawnManager = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnManager>();

        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;
        // Obtenemos el GamePlayUI
        gamePlayUI = GetComponent<GamePlayUI>();

        // Obtenemos la barra del boss en el HUD
        bossBar = root.Q<ProgressBar>("boss-bar");

        // Obtenemos el contenedor de enemigos
        enemiesInScene = GameObject.Find("EnemiesInScene");

        // En caso de haber jefe, lo spawneamos (desactivado) y obtenemos sus datos
        if (bossPrefab != null)
        {
            SetBoss();
        }
    }

    private void Update()
    {
        // Al terminarse el tiempo del nivel
        if(enemySpawnManager.LevelTimer <= 0)
        {
            // Verificamos que en este nivel haya un jefe y no queden mas enemigos en pantalla (A excepcion del mismo jefe que ya esta en escena)
            if(bossPrefab != null && !isInBossFight && enemiesInScene.transform.childCount <= 1)
            {
                StartCoroutine(StartBossFight());
            }
            // Si no hay jefe (o ya murio) entonces verificamos si quedan enemigos en pantalla
            else if(enemiesInScene.transform.childCount <= 0 && !win)
            {
                // Llamamos a la funcion de ganar partida en GamePlayUI
                StartCoroutine(gamePlayUI.ShowWinMenu());
                win = true;
            }
        }

        // Si estamos en pelea:
        if (isInBossFight)
        {
            // Actualizamos la barra de vida del jefe para mostrar la vida actual
            bossBar.value = bossEnemyController.Health;
        }
    }

    private void SetBoss()
    {
        // Instanciamos el jefe en la posicion objetivo
        GameObject bossInstance = Instantiate(bossPrefab, new Vector3(0, 18, 0f), bossPrefab.transform.rotation);
        bossGameObject = bossInstance;

        // Hacemos que sea hijo del contenedor de enemigos
        bossGameObject.transform.parent = enemiesInScene.transform;

        // Obtenemos su EnemyController
        bossEnemyController = bossGameObject.GetComponent<EnemyController>();

        // Le colocamos el nombre a la barra
        bossBar.title = bossName;

        // Lo desactivamos
        bossGameObject.SetActive(false);
    }

    private IEnumerator StartBossFight()
    {
        // Esperamos un tiempo luego de terminado el contador (y que no hayan enemigos)
        yield return new WaitForSeconds(3);

        // Comenzamos la pelea activando el jefe
        isInBossFight = true;
        bossGameObject.SetActive(true);
        // Activamos la barra del jefe
        bossBar.visible = true;
        bossBar.highValue = bossEnemyController.Health;
    }
}