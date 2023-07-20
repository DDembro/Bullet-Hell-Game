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
    private Button backToMenuButton;

    private Button shopButton;

    private Button level1Button;
    private Button level2Button;

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

        level1Button = root.Q<Button>("level-1-button");
        level2Button = root.Q<Button>("level-2-button");

        // Callbacks
        backToMenuButton.RegisterCallback<ClickEvent>(LoadMainMenu);

        shopButton.RegisterCallback<ClickEvent>(ShowShop);

        level1Button.RegisterCallback<ClickEvent>(LoadLevel1);
        level2Button.RegisterCallback<ClickEvent>(LoadLevel2);

    }

    private void LoadMainMenu(ClickEvent click)
    {
        // Cargamos el menu principal
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void ShowShop(ClickEvent click)
    {
        // FALTA IMPLEMENTAR BIEN LA TIENDA
        Debug.Log("Esta es la tienda!");
    }

    private void LoadLevel1(ClickEvent click)
    {
        // Cargamos el nivel 1
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void LoadLevel2(ClickEvent click)
    {
        // Cargamos el nivel 2
        // SceneManager.LoadScene(1, LoadSceneMode.Single); A FUTURO.....
        Debug.Log("Este es el nivel 2!");
    }
}
