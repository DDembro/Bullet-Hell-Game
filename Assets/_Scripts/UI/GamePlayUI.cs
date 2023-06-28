using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayUI : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;

    // Obtenemos los elementos que queremos modificar del UI
    private Label healthLabel;
    private Label scoreLabel;

    // Referencia a PlayerController
    private PlayerController playerController;

    // Variable del jugador
    private float playerHealth;
    private float playerScore;

    private void OnEnable()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos las etiquetas
        healthLabel = root.Q<Label>("health-count");
        scoreLabel = root.Q<Label>("score-count");
    }

    private void Update()
    {
        // Obtenemos en tiempo real las estadisticas que queremos
        playerHealth = playerController.PlayerHealth.Health;
        playerScore = playerController.PlayerEconomy.PlayerScore;

        // Actualizamos el texto que muestra la vida para mostrar el valor actual
        healthLabel.text = "LIFE: " + playerHealth;
        // Lo mismo con el puntaje
        scoreLabel.text = "Score: " + playerScore;
    }
}