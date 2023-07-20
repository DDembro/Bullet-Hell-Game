using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;

    // Referencia a cada vista
    private VisualElement mainMenuView;
    private VisualElement optionView;
    private VisualElement creditView;

    // Referencias a cada boton
    private Button playButton;
    private Button optionButton;
    private Button creditButton;
    private Button exitButton;

    private List<Button> goBackButtons;

    private void OnEnable()
    {
        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos cada vista
        mainMenuView = root.Q<VisualElement>("MainMenuView");
        optionView = root.Q<VisualElement>("OptionView");
        creditView = root.Q<VisualElement>("CreditView");

        // Obtenemos cada boton
        playButton = root.Q<Button>("play-button");
        optionButton = root.Q<Button>("option-button");
        creditButton = root.Q<Button>("credit-button");
        exitButton = root.Q<Button>("exit-button");

        goBackButtons = root.Query<Button>("go-back-button").ToList();

        // Callbacks
        playButton.RegisterCallback<ClickEvent>(LoadLevelSelector);
        optionButton.RegisterCallback<ClickEvent>(ShowOptions);
        creditButton.RegisterCallback<ClickEvent>(ShowCredits);
        exitButton.RegisterCallback<ClickEvent>(ExitGame);

        foreach (Button button in goBackButtons)
        {
            button.RegisterCallback<ClickEvent>(ShowMainMenu);
        }
    }
    
    private void LoadLevelSelector(ClickEvent click)
    {
        // Cargamos el menu de seleccion de niveles
        // El menu de seleccion de niveles siempre esta al final en el indice de Builds
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(totalScenes - 1, LoadSceneMode.Single);
    }

    private void ShowMainMenu(ClickEvent click)
    {
        // Ocultamos el resto de vistas y habilitamos solo el menu principal
        mainMenuView.style.display = DisplayStyle.Flex;

        optionView.style.display = DisplayStyle.None;
        creditView.style.display = DisplayStyle.None;
    }

    private void ShowOptions(ClickEvent click)
    {
        // Ocultamos el resto de vistas y habilitamos solo las opciones
        optionView.style.display = DisplayStyle.Flex;

        mainMenuView.style.display = DisplayStyle.None;
        creditView.style.display = DisplayStyle.None;
    }

    private void ShowCredits(ClickEvent click)
    {
        // Ocultamos el resto de vistas y habilitamos solo los creditos
        creditView.style.display = DisplayStyle.Flex;

        mainMenuView.style.display = DisplayStyle.None;
        optionView.style.display = DisplayStyle.None;
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