using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Esta clase es la encargada de dar la orden de Spawn del jefe especificado, de controlar el HUD correspondiente a dicho jefe
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


        // En caso de haber jefe, lo spawneamos (desactivado) y obtenemos sus datos
        if(bossPrefab != null)
        {
            // Spawneamos el jefe en cuestion
            GameObject boss = enemySpawnManager.SpawnBoss(bossPrefab);
            bossGameObject = boss;

            // Obtenemos su EnemyController
            bossEnemyController = bossGameObject.GetComponent<EnemyController>();

            // Seteamos la barra de vida del jefe
            bossBar.highValue = bossEnemyController.Health;
            bossBar.value = bossEnemyController.Health;

            bossBar.title = bossName;
        }
    }

    private void Start()
    {
        // Obtenemos el contenedor de enemigos
        enemiesInScene = GameObject.Find("EnemiesInScene");
    }

    private void Update()
    {
        // Al terminarse el tiempo del nivel
        if(enemySpawnManager.LevelTimer <= 0)
        {
            // Verificamos que en este nivel haya un jefe
            if(bossPrefab != null && !isInBossFight)
            {
                // Si hay un jefe, y todavia no aparecio, Comenzamos la pelea
                isInBossFight = true;
                bossGameObject.SetActive(true);

                // Activamos la barra del jefe
                bossBar.visible = true;
            }
            // Si no hay jefe (o ya murio) entonces verificamos si quedan enemigos en pantalla
            else if(enemiesInScene.transform.childCount <= 0)
            {
                // Llamamos a la funcion de ganar partida en GamePlayUI
                gamePlayUI.ShowWinMenu();
            }
        }

        // Si estamos en pelea:
        if (isInBossFight)
        {
            // Actualizamos la barra de vida del jefe para mostrar la vida actual
            bossBar.value = bossEnemyController.Health;
        }
    }
}