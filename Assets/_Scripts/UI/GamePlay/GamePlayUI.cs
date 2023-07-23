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
    private VisualElement winView;

    // Etiquetas:
        // GamePlay:
    private Label healthLabel;
    private Label scoreLabel;
    private Label levelLabel;
    private Label timerLabel;
        // Pantalla de Victoria:
    private Label winScoreValue;
    private Label winMoneyValue;

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
    private bool isWinActive = false;

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
        winView = root.Q<VisualElement>("WinView");

        // Obtenemos las etiquetas
        healthLabel = root.Q<Label>("health-count");
        scoreLabel = root.Q<Label>("score-count");
        levelLabel = root.Q<Label>("level-count");
        timerLabel = root.Q<Label>("level-timer-count");

        winScoreValue = root.Q<Label>("win-score-value");
        winMoneyValue = root.Q<Label>("win-money-value");

        // Obtenemos los botones
        exitToMenuButtons = root.Query<Button>("exit-to-menu-button").ToList();
        resumeButton = root.Q<Button>("resume-button");
        restartButton = root.Q<Button>("restart-button");


        // CallBacks de los botones interactuables (Pausa, Muerte y Victoria)
        foreach(Button button in exitToMenuButtons)
        {
            // Este boton esta en comun entre varias de las vistas
            button.RegisterCallback<ClickEvent>(LoadLevelSelector);
        }
        resumeButton.RegisterCallback<ClickEvent>(ShowMenu);
        restartButton.RegisterCallback<ClickEvent>(RestartLevel);
    }

    private void Start()
    {
        // Obtenemos el total de escenas
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // Si no estamos en el tutorial:
        if (SceneManager.GetActiveScene().buildIndex != totalScenes - 2)
        {
            // Actualizamos el display del nivel obteniendo el nivel actual
            levelLabel.text = "Level " + SceneManager.GetActiveScene().buildIndex;
        }
        // En caso contrario el nivel es 0
        else
        {
            levelLabel.text = "Level 0";
        }

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

        // Al precionar Esc, abrimos el menu, ademas lo impedimos si el jugador esta muerto o ya gano
        if (Input.GetKeyDown(KeyCode.Escape) && !isDeathActive && !isWinActive)
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

        // Por si acaso, nos aseguramos que el menu y la victoria estan desactivados
        menuView.style.display = DisplayStyle.None;
        winView.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Metodo publico que es llamado desde BossUIController
    /// </summary>
    public IEnumerator ShowWinMenu()
    {
        // Esperamos un tiempo antes de mostrar la pantalla de victoria
        yield return new WaitForSeconds(3);

        // En caso de que el jugador haya muerto justo despues de terminar el nivel, lo hacemos perder igualmente
        if(playerHealth <= 0)
        {
            ShowDeathMenu();
            yield return null;
        }
        // En caso contrario:
        else
        {
            // Paramos el tiempo y hacemos true haber ganado el nivel
            Time.timeScale = 0f;
            isWinActive = true;

            // Le damos al jugador dinero equivalente al 10% de su puntaje
            float money = playerScore/10;
            playerController.PlayerEconomy.AddMoney(money);

            // Pasamos los valores de las estadisticas a los Labels correspondientes
            winScoreValue.text = playerScore.ToString();
            winMoneyValue.text = money.ToString() + "$";

            // Activamos la Vista de Victoria
            winView.style.display = DisplayStyle.Flex;
        }
    }

    private void LoadLevelSelector(ClickEvent click)
    {
        // Cargamos el menu de seleccion de niveles y activamos el tiempo otra vez
        Time.timeScale = 1f;
        // El menu de seleccion de niveles siempre esta al final en el indice de Builds
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(totalScenes - 1, LoadSceneMode.Single);
    }

    private void RestartLevel(ClickEvent click)
    {
        // Reiniciamos el nivel en el que estamos y activamos el tiempo otra vez
        Time.timeScale = 1f;
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
    }
}