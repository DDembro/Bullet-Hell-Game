using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GamePlayUI : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;
    // Referencia a PlayerController
    private PlayerController playerController;
    // Referencia al Spawner de enemigos
    private EnemySpawnManager enemySpawnManager;

    // Obtenemos los elementos que queremos modificar del UI
    // Las vistas:
    private VisualElement gamePlayView;
    private VisualElement menuView;
    private VisualElement deathView;
    // Etiquetas:
    private Label healthLabel;
    private Label scoreLabel;
    private Label levelLabel;
    private Label timerLabel;
    // Botones:
    private List<Button> exitToMenuButtons;
    private Button resumeButton;
    private Button restartButton;


    // Variable del jugador
    private float playerHealth;
    private float playerScore;

    // Variables adicionales
    private bool isMenuActive = false;
    private bool isDeathActive = false;

    private void OnEnable()
    {
        // Obtenemos las referencias
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        enemySpawnManager = GameObject.Find("EnemySpawner").GetComponent<EnemySpawnManager>();

        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos las vistas
        gamePlayView = root.Q<VisualElement>("GamePlayView");
        menuView = root.Q<VisualElement>("MenuView");
        deathView = root.Q<VisualElement>("DeathView");

        // Obtenemos las etiquetas
        healthLabel = root.Q<Label>("health-count");
        scoreLabel = root.Q<Label>("score-count");
        levelLabel = root.Q<Label>("level-count");
        timerLabel = root.Q<Label>("level-timer-count");

        // Obtenemos los botones
        exitToMenuButtons = root.Query<Button>("exit-to-menu-button").ToList();
        resumeButton = root.Q<Button>("resume-button");
        restartButton = root.Q<Button>("restart-button");


        // CallBacks
        foreach(Button button in exitToMenuButtons)
        {
            button.RegisterCallback<ClickEvent>(GoToMainMenu);
        }
        resumeButton.RegisterCallback<ClickEvent>(ShowMenu);
        restartButton.RegisterCallback<ClickEvent>(RestartLevel);
    }

    private void Start()
    {
        // Actualizamos el display del nivel obteniendo el nivel actual
        levelLabel.text = "Level " + SceneManager.GetActiveScene().buildIndex;

        // Obtenemos en tiempo real las estadisticas que queremos
        playerHealth = playerController.PlayerHealth.Health;
        playerScore = playerController.PlayerEconomy.PlayerScore;
    }

    private void Update()
    {
        // Obtenemos en tiempo real las estadisticas que queremos
        playerHealth = playerController.PlayerHealth.Health;
        playerScore = playerController.PlayerEconomy.PlayerScore;

        // Actualizamos el texto que muestran las etiquetas que cambian
        healthLabel.text = "Life: " + playerHealth;
        scoreLabel.text = "Score: " + playerScore;
        timerLabel.text = "Time: " + Mathf.CeilToInt(enemySpawnManager.LevelTimer);

        // Al precionar Esc, abrimos el menu, ademas lo impedimos si el jugador esta muerto
        if (Input.GetKeyDown(KeyCode.Escape) && !isDeathActive)
        {
            ShowMenu(null);
        }
    }

    private void ShowMenu(ClickEvent click)
    {
        // Activamos la pantalla del menu si no lo estaba, y la desactivamos en caso contrario
        if(!isMenuActive)
        {
            // Detenemos el tiempo
            Time.timeScale = 0f;
            menuView.style.display = DisplayStyle.Flex;
            isMenuActive = true;
        }
        else if (isMenuActive)
        {
            // Reanudamos el tiempo
            Time.timeScale = 1f;
            menuView.style.display = DisplayStyle.None;
            isMenuActive = false;
        }
    }

    /// <summary>
    /// ShowDeathMenu() Es un metodo publico, que se llama desde PlayerHealth
    /// </summary>
    public void ShowDeathMenu()
    {
        // Detenemos el tiempo
        Time.timeScale = 0f;

        // Activamos la pantalla de muerte
        deathView.style.display = DisplayStyle.Flex;
        isDeathActive = true;

        // Por si acaso, nos aseguramos que el menu esta desactivado
        menuView.style.display = DisplayStyle.None;
    }

    private void GoToMainMenu(ClickEvent click)
    {
        // Cargamos el menu principal y activamos el tiempo otra vez
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void RestartLevel(ClickEvent click)
    {
        // Reiniciamos el nivel en el que estamos y activamos el tiempo otra vez
        Time.timeScale = 1f;
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}