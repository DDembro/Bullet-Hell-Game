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
    public float _horizontalSpeed = 7f;
    public float _verticalSpeed = 5f;

    public float _maxHorizontalOffSet = 9f;
    public float _minHorizontalOffSet = -9f;

    public float _maxVerticalOffSet = 4f;
    public float _minVerticalOffSet = -9f;

    // Variables del disparo
    [Header("Disparo")]
    public float _fireRate = 0.2f;

    // Variables de las balas
    [Header("Balas")]
    public float _BulletSpeed = 10f;
    public float _BulletDamage = 1f;

    // Variables de la vida
    public float _Health = 10f;

    private void Awake()
    {
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