using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    // Definimos el elemento Root
    private VisualElement root;

    // Referencia a cada vista
    private VisualElement levelSelectorView;
    private VisualElement shopView;

    // Referencias a cada boton
        // Selector de niveles:
    private Button backToMenuButton;

    private Button shopButton;

    private Button tutorialButton;
    private Button level1Button;
    private Button level2Button;
    private Button level3Button;
    // Tienda:
    private Button returnToSelectorButton;

    private void OnEnable()
    {
        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos cada vista
        levelSelectorView = root.Q<VisualElement>("LevelSelectorView");
        shopView = root.Q<VisualElement>("ShopView");

        // Obtenemos cada boton
        backToMenuButton = root.Q<Button>("back-to-menu-button");

        shopButton = root.Q<Button>("shop-button");

        tutorialButton = root.Q<Button>("tutorial-button");
        level1Button = root.Q<Button>("level-1-button");
        level2Button = root.Q<Button>("level-2-button");
        level3Button = root.Q<Button>("level-3-button");


        returnToSelectorButton = root.Q<Button>("return-to-selector-button");

        // Callbacks
        backToMenuButton.RegisterCallback<ClickEvent>(LoadMainMenu);

        shopButton.RegisterCallback<ClickEvent>(ShowShop);

        tutorialButton.RegisterCallback<ClickEvent>(LoadTutorial);
        level1Button.RegisterCallback<ClickEvent>(LoadLevel1);
        level2Button.RegisterCallback<ClickEvent>(LoadLevel2);
        level3Button.RegisterCallback<ClickEvent>(LoadLevel3);


        returnToSelectorButton.RegisterCallback<ClickEvent>(ShowLevelSelector);
    }

    private void LoadMainMenu(ClickEvent click)
    {
        // Cargamos el menu principal
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void ShowShop(ClickEvent click)
    {
        // Desactivamos la vista de la seleccion de niveles y activamos la tienda
        levelSelectorView.style.display = DisplayStyle.None;
        shopView.style.display = DisplayStyle.Flex;
    }

    private void ShowLevelSelector(ClickEvent click)
    {
        // Desactivamos la vista de la tienda y activamos la de seleccion de niveles
        shopView.style.display = DisplayStyle.None;
        levelSelectorView.style.display = DisplayStyle.Flex;
    }

    private void LoadTutorial(ClickEvent click)
    {
        // Cargamos el tutorial
        // El tutorial siempre es el anteultimo en el indice de Builds
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(totalScenes - 2, LoadSceneMode.Single);
    }

    private void LoadLevel1(ClickEvent click)
    {
        // Cargamos el nivel 1
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void LoadLevel2(ClickEvent click)
    {
        // Cargamos el nivel 2
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    private void LoadLevel3(ClickEvent click)
    {
        // Cargamos el nivel 3
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
