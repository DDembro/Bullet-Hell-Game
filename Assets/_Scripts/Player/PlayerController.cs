using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Este sistema esta planteado para que todos los componentes que necesiten obtener informacion del jugador
/// tengan que comunicarse con PlayerController, como una forma de intentar centralizar todos los vinculos en vez de tenerlos
/// dispersos entre varios Scripts
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Referencia al jugador
    private GameObject playerGO;
    // Referencia a la bala del jugador
    private GameObject pBulletPrefab;

    // Referencias a todos los Scripts vinculados al jugador
    public PlayerMovement PlayerMovement { private set; get; }
    public PlayerShooting PlayerShooting { private set; get; }
    public PlayerBulletController PlayerBulletController { private set; get; }
    public PlayerHealth PlayerHealth { private set; get; }
    public PlayerEconomy PlayerEconomy { private set; get; }

    // Variables del movimiento
    [Header("Movimiento")]
    public float _horizontalSpeed;
    public float _verticalSpeed;

    public float _maxHorizontalOffSet;
    public float _minHorizontalOffSet;

    public float _maxVerticalOffSet;
    public float _minVerticalOffSet;

    // Variables del disparo
    [Header("Disparo")]
    public float _fireRate;

    // Variables de las balas
    [Header("Balas")]
    public float _BulletSpeed;
    public float _BulletDamage;

    // Variables de la vida
    public float _Health;
    public float _inmuneTime;

    private void Awake()
    {
        // Inicializamos las variables
        _horizontalSpeed = 7f;
        _verticalSpeed = 5f;

        _maxHorizontalOffSet = 11f;
        _minHorizontalOffSet = -11f;

        _maxVerticalOffSet = 4f;
        _minVerticalOffSet = -11f;

        _fireRate = 0.2f;

        _BulletSpeed = 10f;
        _BulletDamage = 1f;

        _Health = 10f;
        _inmuneTime = 0.5f;

        // Obtenemos la referencia al jugador
        playerGO = GameObject.FindWithTag("Player");

        // Obtenemos la referencia a la bala
        pBulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/PlayerBullet");

        // Cargamos todas las referencias
        LoadScriptsReferences();
    }

    private void Update()
    {

    }

    private void LoadScriptsReferences()
    {
        PlayerMovement = playerGO.GetComponent<PlayerMovement>();
        PlayerShooting = playerGO.GetComponent<PlayerShooting>();
        PlayerBulletController = pBulletPrefab.GetComponent<PlayerBulletController>();
        PlayerHealth = playerGO.GetComponent<PlayerHealth>();
        PlayerEconomy = playerGO.GetComponent<PlayerEconomy>();
    }
}