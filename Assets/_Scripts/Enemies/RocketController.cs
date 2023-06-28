using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class RocketController : MonoBehaviour
{
    // Referencia al jugador
    private GameObject player;

    // Prefab utilizados
    private GameObject bulletPrefab;
    private GameObject explosionPrefab;

    // Variables sobre el jugador
    private float playerDamage;

    // Estadisticas de las balas
    public float BulletSpeed = 6f;
    public float BulletDamage = 2f;

    // Estadisticas del cohete
    public float RocketDamage = 4f;
    private float rocketSpeed = 10f;
    private float rocketRotateSpeed = 100f;
    private float rocketHealth = 5f;

    // RB del cohete
    private Rigidbody2D rb;

    private void Start()
    {
        // Cargamos los prefabs
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/NeutralBullet");
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");

        // Obtenemos la referencia al jugador
        player = GameObject.FindWithTag("Player");

        // Obtenemos el RB
        rb = GetComponent<Rigidbody2D>();

        // Despues de unos segundos, si todavia no exploto, lo hacemos explotar
        Invoke("Explode", 4.5f);
    }

    private void Update()
    {
        // Calcula la dirección hacia el objetivo
        Vector2 direction = player.transform.position - transform.position;

        // Calcula el ángulo hacia el objetivo
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // Obtiene la diferencia de ángulos entre la rotación actual y la rotación objetivo en el rango [-180, 180]
        float angleDifference = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle);

        // Calcula la velocidad de rotación basada en la diferencia de ángulos
        float rotationSpeed = Mathf.Clamp(angleDifference / 180f, -1f, 1f) * rocketRotateSpeed;

        // Aplica fuerza para rotar el cohete hacia la dirección objetivo
        rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);


        // Lo hacemos desplazarce hacia adelante
        rb.AddForce(transform.up * rocketSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void Explode()
    {
        // Al explotar, creamos un aro de balas
        for(int i = 0; i < 12; i++)
        {
            // Instanciamos la bala en la posicion y la rotacion del cohete, y por cada bala rotamos el angulo un poco
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 360 / 12 * i));

            // Pasar la referencia al objeto padre al objeto bullet
            bullet.GetComponent<NeutralBulletController>().SetParentRocket(this);
        }

        // Instanciamos una explosion y destruimos el gameObject
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void TakeDamage(GameObject bullet)
    {
        // Comprobamos que tipo de bala es y obtenemos su daño
        if (bullet.CompareTag("PlayerBullet"))
        {
            playerDamage = bullet.GetComponent<PlayerBulletController>().BulletDamage;
        }
        else if (bullet.CompareTag("NeutralBullet"))
        {
            playerDamage = bullet.GetComponent<NeutralBulletController>().BulletDamage;
        }

        // Recibe daño del jugador
        rocketHealth -= playerDamage;

        // Si no tiene mas vida, muere
        if (rocketHealth <= 0)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al colisionar con el jugador el cohete explota
        if (collision.CompareTag("Player"))
        {
            Explode();
        }

        // Al entrar en contacto con una bala
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("NeutralBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;

            // Efectuamos el daño
            TakeDamage(bullet);
        }
    }
}
