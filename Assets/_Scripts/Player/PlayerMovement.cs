using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Movimiento
    // Input
    private float horizontalInput;
    private float verticalInput;
    // Velocidad
    private float horizontalSpeed;
    private float verticalSpeed;
    // Vector
    private Vector3 movement;

    // Restricciones del movimiento
    // Maximo OffSet
    private float maxHorizontalOffSet;
    private float minHorizontalOffSet;
    // Minimo OffSet
    private float maxVerticalOffSet;
    private float minVerticalOffSet;

    private void Awake()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Inicializamos las estadisticas
        horizontalSpeed = playerController._horizontalSpeed;
        verticalSpeed = playerController._verticalSpeed;

        maxHorizontalOffSet = playerController._maxHorizontalOffSet;
        minHorizontalOffSet = playerController._minHorizontalOffSet;

        maxVerticalOffSet = playerController._maxVerticalOffSet;
        minVerticalOffSet = playerController._minVerticalOffSet;

        // En el caso de estar en el tutorial, cambiamos el maxOffset vertival para nuestra convenencia
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (SceneManager.GetActiveScene().buildIndex == totalScenes - 2)
        {
            maxVerticalOffSet = -4f;
        }
    }

    private void Update()
    {
        // Leemos el Input horizontal y vertical
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Definimos el vector de Movimiento
        movement = new Vector3(horizontalInput * horizontalSpeed * Time.deltaTime, verticalInput * verticalSpeed * Time.deltaTime, 0);
        // Aplicamos el movimiento al jugador
        transform.Translate(movement);

        // Impedimos que el jugador se mueva mas alla del offset vertical establecido
        if (transform.position.y >= maxVerticalOffSet)
        {
            transform.position = new Vector3(transform.position.x, maxVerticalOffSet, 0);
        }
        if (transform.position.y <= minVerticalOffSet)
        {
            transform.position = new Vector3(transform.position.x, minVerticalOffSet, 0);
        }

        // Impedimos que el jugador se mueva mas alla del offset horizontal establecido
        if (transform.position.x >= maxHorizontalOffSet)
        {
            transform.position = new Vector3(maxHorizontalOffSet, transform.position.y, 0);
        }
        if (transform.position.x <= minHorizontalOffSet)
        {
            transform.position = new Vector3(minHorizontalOffSet, transform.position.y, 0);
        }

    }
}