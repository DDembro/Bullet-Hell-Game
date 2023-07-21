using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MenuOptions : MonoBehaviour
{
    // Definimos el AudioMixer del juego
    [SerializeField] private AudioMixer audioMixer;

    // Definimos el elemento Root
    private VisualElement root;

    // Referencias a cada elemento interactuable de las opciones
    private DropdownField fpsDropdown;
    private DropdownField resolutionDropdown;

    private SliderInt audioSliderInt;
    private SliderInt musicSliderInt;

    private Toggle fullscreenToggle;


    // Audio de referencia al cambiar el nivel de volumen de SFX
    [SerializeField] private AudioSource testAudio;

    private void OnEnable()
    {
        // Obtenemos el elemento root
        root = GetComponent<UIDocument>().rootVisualElement;

        // Obtenemos cada elemento
        fpsDropdown = root.Q<DropdownField>("fps-dropdown");
        resolutionDropdown = root.Q<DropdownField>("resolution-dropdown");

        audioSliderInt = root.Q<SliderInt>("audio-slider-int");
        musicSliderInt = root.Q<SliderInt>("music-slider-int");

        fullscreenToggle = root.Q<Toggle>("fullscreen-toggle");

        // Callbacks
        fpsDropdown.RegisterCallback<ChangeEvent<string>>(SetFps);
        resolutionDropdown.RegisterCallback< ChangeEvent<string>>(SetResolution);

        audioSliderInt.RegisterCallback<ChangeEvent<int>>(SetSFXVolume);
        musicSliderInt.RegisterCallback<ChangeEvent<int>>(SetMusicVolume);

        fullscreenToggle.RegisterCallback<ChangeEvent<bool>>(SetFullscreen);
    }

    private void Start()
    {
        // Seteamos los ajustes
        SetFpsDropdown();

        SetResolutionDropdown();

        // Hacemos que el slider del audio y la musica esten sincronizados con el volumen actual del juego
        float sliderValue;
        audioMixer.GetFloat("SFXVolume", out sliderValue);
        audioSliderInt.value = Mathf.RoundToInt(sliderValue);

        audioMixer.GetFloat("MusicVolume", out sliderValue);
        musicSliderInt.value = Mathf.RoundToInt(sliderValue);

        // Controlamos que el CheckBox de fullscreen tenga la opcion correcta seleccionada
        if (Screen.fullScreen)
        {
            fullscreenToggle.value = true;
        }
        else
        {
            fullscreenToggle.value = false;
        }

    }

    private void SetFpsDropdown()
    {
        // Borramos todas las opcciones del Dropdown
        fpsDropdown.choices.Clear();

        // Luego añadimos las opcciones a cada uno
        fpsDropdown.choices.Add("30");
        fpsDropdown.choices.Add("60");
        fpsDropdown.choices.Add("75");
        fpsDropdown.choices.Add("144");

        // Con un switch controlamos que tasa de FPS tenemos, en caso de ser distinta a cualquiera de las opcciones la configuramos a 60
        switch (Application.targetFrameRate)
        {
            case 30:
                fpsDropdown.index = 0;
                break;

            case 60:
                fpsDropdown.index = 1;
                break;

            case 75:
                fpsDropdown.index = 2;
                break;

            case 144:
                fpsDropdown.index = 3;
                break;

            default:
                Application.targetFrameRate = 60;
                fpsDropdown.index = 1;
                break;
        }
    }

    private void SetResolutionDropdown()
    {
        // Borramos todas las opcciones del Dropdown
        resolutionDropdown.choices.Clear();

        // Obtenemos todas las resoluciones de la PC actual y las asignamos una por una en cada opccion
        // Ademas, seteamos el valor actual de resolucion de esa PC en las opcciones

        int currentResolutionIndex = 0;
        var resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.choices.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Luego de cargar los valores, seteamos el por defecto que obtuvimos
        resolutionDropdown.index = currentResolutionIndex;
    }

    private void SetFps(ChangeEvent<string> evt)
    {
        // Convertimos el string a int y le ponemos ese valor a la tasa de FPS objetivo
        int selectedValue = int.Parse(evt.newValue);
        Application.targetFrameRate = selectedValue;
    }

    private void SetResolution(ChangeEvent<string> evt)
    {
        // Partimos el string en formato "1111 x 1111" para poder obtener cada numero por separado, y asi pasarlo a int
        string[] resolutionParts = evt.newValue.Split('x');

        int width = int.Parse(resolutionParts[0].Trim());
        int height = int.Parse(resolutionParts[1].Trim());

        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    private void SetSFXVolume(ChangeEvent<int> evt)
    {
        // Establecemos el nuevo valor de los efectos de sonido
        float volume = evt.newValue;
        audioMixer.SetFloat("SFXVolume", volume);
        // Reproducimos un sonido de referencia para que el usuario sepa que volumen tiene en el juego
        testAudio.enabled = true;
        testAudio.Play();
    }

    private void SetMusicVolume(ChangeEvent<int> evt)
    {
        // Establecemos el nuevo valor de la musica
        float volume = evt.newValue;
        audioMixer.SetFloat("MusicVolume", volume);
    }

    private void SetFullscreen(ChangeEvent<bool> isFullScreen)
    {
        // Cambiamos de modo pantalla completa dependiendo del valor del CheckBox
        Screen.fullScreen = isFullScreen.newValue;
    }
}