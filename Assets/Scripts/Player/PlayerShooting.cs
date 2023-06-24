using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Input que registra si el jugador esta disparando
    private bool shootingInput;

    // Prefab de la bala
    public GameObject BulletPrefab;
    public Transform GunCannon;

    // Estadisticas
    private float fireRate;

    // Variable para ver si esta disponible el proximo disparo
    private bool canShoot;

    private void Awake()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Inicializamos las estadisticas
        fireRate = playerController._fireRate;

        // Inicializamos poder disparar en true
        canShoot = true;
    }

    private void Update()
    {
        // Leemos el input del espacio para saber si esta disparando
        shootingInput = Input.GetKey(KeyCode.Space);

        // Al precionar espacio disparamos
        if (shootingInput && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        // Generamos la bala
        GameObject bullet = Instantiate(BulletPrefab, GunCannon.position, GunCannon.rotation);
        // Hacemos falso canShoot
        canShoot = false;

        // Esperamos a que pase el CoolDown del anterior disparo
        yield return new WaitForSeconds(fireRate);
        // Volvemos a habilitar el disparo
        canShoot = true;

    }
}
