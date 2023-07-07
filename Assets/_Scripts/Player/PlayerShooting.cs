using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Input que registra si el jugador esta disparando
    private bool shootingInput;

    // Prefab de la bala
    private GameObject bulletPrefab;
    private Transform gunCannon;

    // Estadisticas
    private float fireRate;

    // Variable para ver si esta disponible el proximo disparo
    private bool canShoot;

    // AudioSource Del efecto de disparo
    private AudioSource shootingAudio;

    private void Awake()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GetComponent<PlayerController>();

        // Obtenemos el efecto de disparo
        shootingAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Inicializamos las estadisticas
        fireRate = playerController._fireRate;

        // Inicializamos poder disparar en true
        canShoot = true;

        // Obtenemos el cañon
        gunCannon = transform.Find("GunCannon");

        // Definimos cual es la bala a utilizar
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/PlayerBullet");
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
        GameObject bullet = Instantiate(bulletPrefab, gunCannon.position, gunCannon.rotation);
        // Hacemos el sonido de disparar
        shootingAudio.enabled = true;
        shootingAudio.Play();
        // Hacemos falso canShoot
        canShoot = false;

        // Esperamos a que pase el CoolDown del anterior disparo
        yield return new WaitForSeconds(fireRate);
        // Volvemos a habilitar el disparo
        canShoot = true;

    }
}
