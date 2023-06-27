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

    // Referencia a PlayerController
    private PlayerController playerController;

    private void OnEnable()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos el contador de vida
        healthLabel = root.Q<Label>("health-count");
    }

    private void Update()
    {
        // Obtenemos la vida del jugador en cada momento
        float playerHealth = playerController.PlayerHealth.Health;

        // Actualizamos el texto que muestra la vida para mostrar el valor actual
        healthLabel.text = "LIFE: " + playerHealth;
    }

}
