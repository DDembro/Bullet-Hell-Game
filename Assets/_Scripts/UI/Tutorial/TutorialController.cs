using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialController : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;
    // Referencia al jugador
    private GameObject player;
    // Referencia al Spawner de enemigos
    private EnemySpawnManager enemySpawnManager;

    // Contenedor de enemigos
    private GameObject enemiesInScene;

    // Los diferentes VisualElements de cada pantalla del tutorial
    private VisualElement TutorialBox1;
    private VisualElement TutorialBox2;
    private VisualElement TutorialBox3;
    private VisualElement TutorialBox4;


    // Variables para controlar el flujo del tutorial
    private bool isTutorialBox1 = true;
    private bool isTutorialBox2 = false;
    private bool isTutorialBox3 = false;
    private bool isTutorialBox4 = false;

    private bool isTutorialEnd = false;
    private bool auxiliary = false;

    // Variables de la Caja Tutorial 2
    private bool MoveUp = false;
    private bool MoveDown = false;
    private bool MoveRight = false;
    private bool MoveLeft = false;

    // Variables de la Caja Tutorial 3
    private GameObject hunterDummy1;
    private GameObject hunterDummy2;
    private GameObject hunterDummy3;

    private void OnEnable()
    {
        // Obtenemos las referencias
        player = GameObject.FindWithTag("Player");
        enemySpawnManager = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnManager>();

        // Obtenemos el contenedor
        enemiesInScene = GameObject.Find("EnemiesInScene");

        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos cada TutorialBox
        TutorialBox1 = root.Q<VisualElement>("TutorialBox1");
        TutorialBox2 = root.Q<VisualElement>("TutorialBox2");
        TutorialBox3 = root.Q<VisualElement>("TutorialBox3");
        TutorialBox4 = root.Q<VisualElement>("TutorialBox4");
    }

    private void Start()
    {
        // Deshabilitamos la capacidad de moverse y disparar al jugador al inicio del tutorial
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerShooting>().enabled = false;

        // Buscamos los Dummys de la caja tutorial 3
        hunterDummy1 = GameObject.Find("HunterDummy1");
        hunterDummy2 = GameObject.Find("HunterDummy2");
        hunterDummy3 = GameObject.Find("HunterDummy3");
        // Inmediatamente despues de obtener su referencia los ocultamos
        hunterDummy1.SetActive(false);
        hunterDummy2.SetActive(false);
        hunterDummy3.SetActive(false);
    }

    private void Update()
    {
        // Si todavia estamos en el tutorial:
        if (!isTutorialEnd)
        {
            // Caja del Tutorial 1
            if (isTutorialBox1)
            {
                // Al precionar Espacio pasamos a la siguiente caja
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // Ocultamos esta caja y hacemos visible la siguiente
                    isTutorialBox1 = false;
                    isTutorialBox2 = true;

                    TutorialBox1.style.display = DisplayStyle.None;
                    TutorialBox2.style.display = DisplayStyle.Flex;
                }
            }
            // Caja del Tutorial 2
            else if (isTutorialBox2)
            {
                // Hacemos que el jugador pueda moverse
                player.GetComponent<PlayerMovement>().enabled = true;

                // Registramos el uso de cada tecla de movimiento
                if (Input.GetAxisRaw("Vertical") > 0) { MoveUp = true; };
                if (Input.GetAxisRaw("Vertical") < 0) { MoveDown = true; };
                if (Input.GetAxisRaw("Horizontal") > 0) { MoveRight = true; };
                if (Input.GetAxisRaw("Horizontal") < 0) { MoveLeft = true; };

                // Una vez que el jugador se movio en todas las direcciones, pasamos a la siguiente caja de tutorial
                if(MoveUp && MoveDown && MoveRight && MoveLeft)
                {
                    // Ocultamos esta caja y hacemos visible la siguiente
                    isTutorialBox2 = false;
                    isTutorialBox3 = true;

                    TutorialBox2.style.display = DisplayStyle.None;
                    TutorialBox3.style.display = DisplayStyle.Flex;
                }
            }
            // Caja del Tutorial 3
            else if (isTutorialBox3)
            {
                // Hacemos que el jugador pueda disparar
                player.GetComponent<PlayerShooting>().enabled = true;

                // Activamos los enemigos preparados en la escena
                ActivateDummys();

                // Si el jugador ya los mato cambiamos de escena
                if(enemiesInScene.transform.childCount <= 1)
                {
                    // Ocultamos esta caja y hacemos visible la siguiente
                    isTutorialBox3 = false;
                    isTutorialBox4 = true;

                    TutorialBox3.style.display = DisplayStyle.None;
                    TutorialBox4.style.display = DisplayStyle.Flex;
                }
            }
            // Caja del Tutorial 4
            else if (isTutorialBox4)
            {
                // Deshabilitamos la capacidad de moverse y disparar al jugador nuevamente
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<PlayerShooting>().enabled = false;

                // Al precionar Espacio terminamos el tutorial y volvemos a habilitar todas las acciones del jugador
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // Ocultamos esta caja y finalizamos el tutorial
                    isTutorialBox4 = false;
                    isTutorialEnd = true;

                    TutorialBox4.style.display = DisplayStyle.None;

                     // Habilitamos al jugador
                    player.GetComponent<PlayerMovement>().enabled = true;
                    player.GetComponent<PlayerShooting>().enabled = true;
                }
            }
        }
        // Si el tutorial termino:
        else
        {
            // Con el auxiliar nos aseguramos de que esto ocurre una sola vez
            if (!auxiliary)
            {
                // Le damos tiempo al nivel para empezar el tutorial
                enemySpawnManager.LevelTimer = 15f;
                auxiliary = true;

                // Borramos el objeto empty que cumplia la funcion de no terminar el nivel por victoria (no haber mas enemigos)
                Destroy(GameObject.Find("Empty"));
            }
        }

    }

    /// <summary>
    /// Funcion encargada de activar los dummys de forma eficiente para evitar generar errores
    /// </summary>
    private void ActivateDummys()
    {
        // Activamos los Dummys
        hunterDummy1.SetActive(true);
        hunterDummy2.SetActive(true);
        hunterDummy3.SetActive(true);
        // Instantaneamente despues cambiamos sus referencias a cualquier otro objeto para evitar errores
        hunterDummy1 = GameObject.Find("Walls");
        hunterDummy2 = GameObject.Find("Walls");
        hunterDummy3 = GameObject.Find("Walls");
    }
}