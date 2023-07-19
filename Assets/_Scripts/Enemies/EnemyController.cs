using System.Collections;
using UnityEngine;

/// <summary>
/// Esta clase esta dedicada a ser una plantilla para las demas clases de enemigos
/// </summary>
public abstract class EnemyController : MonoBehaviour
{
    // Variable para almacenar el jugador y sus datos
    private PlayerController playerController;
    private float playerDamage;

    // Estadisticas generales
    public float Health;
    public float MelleDamage;

    public float HorizontalSpeed;
    public float VerticalSpeed;

    // variables sobre las balas
    private GameObject bulletPrefab;
    public float BulletSpeed;
    public float BulletDamage;
    public float FireRate;
    // Variables especificas del disparo multiple
    private int actualMultipleShoot;
    public int MultipleShoot;

    // Variable de los puntos al morir
    public float OnDeathScore;

    // Prefab de la explosion
    private GameObject explosionPrefab;

    // Componentes del enemigo
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Inicializamos variables
        actualMultipleShoot = -1; // Es la forma de poder setear la variable por primera vez en Shoot()

        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Obtenemos el prefab de la explosion
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
    }

    /// <summary>
    /// Metodo protegido que se encarga de conseguir el prefab de bala que va a utilizar el enemigo
    /// mediante una ruta por String
    /// </summary>
    /// <param name="prefabRute"></param>
    protected void GetBullet(string prefabRute)
    {
        // Obtenemos la bala especifica
        bulletPrefab = Resources.Load<GameObject>(prefabRute);
    }

    /// <summary>
    /// Metodo privado para obtener el daño de un GameObjetc bala en concreto
    /// </summary>
    /// <param name="bullet"></param>
    private void GetPlayerBullet(GameObject bullet)
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
    }

    protected void Die()
    {
        // Añadimos los puntos al jugador por la muerte
        playerController.PlayerEconomy.AddScore(OnDeathScore);

        // Instanciamos una explosion y destruimos el gameObject
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void TakeDamage(GameObject bullet)
    {
        GetPlayerBullet(bullet);

        // Recibe daño del jugador
        Health -= playerDamage;

        // Si no tiene mas vida, muere
        if (Health <= 0)
        {
            Die();
        }

        // Mostramos un indicador visual del daño
        StartCoroutine(HitIndicator());
    }


    /// <summary>
    /// Una funcion de muerte mas sofisticada donde se puede tener mas flexibilidad para añadir ciertos comportamientos adicionales
    /// </summary>
    /// <param name="explosionRadius"></param>
    protected void BossDie(float explosionRadius)
    {
        // Añadimos los puntos al jugador por la muerte
        playerController.PlayerEconomy.AddScore(OnDeathScore);

        // Instanciamos una explosion y destruimos el gameObject
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale *= explosionRadius;
        Destroy(gameObject);
    }

    protected void TakeDamageBoss(GameObject bullet, float explosionRadius)
    {
        GetPlayerBullet(bullet);

        // Recibe daño del jugador
        Health -= playerDamage;

        // Si no tiene mas vida, muere
        if (Health <= 0)
        {
            BossDie(explosionRadius);
        }

        // Mostramos un indicador visual del daño
        StartCoroutine(HitIndicator());
    }

    /// <summary>
    /// Corutina simple que se encarga que dar un indicador visual del daño recibido en el enemigo
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitIndicator()
    {
        // Si no tenemos el SpriteRenderer, lo conseguimos
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Corutina protegida que permite al resto de clases enemigas disparar
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    protected IEnumerator Shoot(Vector3 position, Quaternion rotation)
    {
        if (actualMultipleShoot < 0)
        {
            // La primera vez, seteamos actualMultipleShoot
            actualMultipleShoot = MultipleShoot;
        }

        // Ciclo para controlar la cantidad de veces que se llamara Shooting()
        for (int i = 0; i < actualMultipleShoot; i++)
        {
            // Llamada a Shooting()
            yield return StartCoroutine(Shooting(position, rotation));

            // Espera entre llamadas a Shooting()
            yield return new WaitForSeconds(FireRate);
        }
    }

    /// <summary>
    /// Corutina necesaria para que Shoot() funcione correctamente
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <returns></returns>
    private IEnumerator Shooting(Vector3 pos, Quaternion rot)
    {
        // Instanciamos la bala en la posicion y la rotacion pasada por parametro
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        actualMultipleShoot--;

        if (bullet.CompareTag("EnemyBullet"))
        {
            // Pasar la referencia del objeto padre a la bala
            bullet.GetComponent<EnemyBulletController>().SetParentObject(this);
        }
        else if (bullet.CompareTag("Rocket"))
        {
            // No hacer nada
        }

        // Esperamos el CoolDown del arma para volver a disparar
        yield return new WaitForSeconds(FireRate);
        actualMultipleShoot = MultipleShoot;
    }
}