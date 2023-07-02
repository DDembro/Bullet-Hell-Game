using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;

    // Referencias a cada boton
    private Button playButton;
    private Button optionButton;
    private Button exitButton;

    private void OnEnable()
    {
        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos cada boton
        playButton = root.Q<Button>("play-button");
        optionButton = root.Q<Button>("option-button");
        exitButton = root.Q<Button>("exit-button");

        // Callbacks
        playButton.RegisterCallback<ClickEvent>(StartLevel);
        optionButton.RegisterCallback<ClickEvent>(ShowOptions);
        exitButton.RegisterCallback<ClickEvent>(ExitGame);
    }
    
    private void StartLevel(ClickEvent click)
    {
        // Al precionar click en el boton play, cargamos el primer nivel
        SceneManager.LoadScene(1 , LoadSceneMode.Single);
    }

    private void ShowOptions(ClickEvent click)
    {
        // Falta implementar una vista para las opciones (y pensar que opciones cambiar lol)
    }

    private void ExitGame(ClickEvent click)
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

// Faltan funciones para los botones de opciones
// Estaria bueno incorporar una pantalla de seleccion de niveles
// Tambien ver donde carajo meto la tienda