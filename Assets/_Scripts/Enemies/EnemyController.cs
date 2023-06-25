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
    public float Damage;
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


    private void Awake()
    {
        // Inicializamos variables
        actualMultipleShoot = -1; // Es la forma de poder setear la variable por primera vez en Shoot()

        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    protected void GetBullet(string prefabRute)
    {
        // Obtenemos la bala especifica
        bulletPrefab = Resources.Load<GameObject>(prefabRute);
    }

    private void GetPlayerBullet(GameObject bullet)
    {
        // Obtenemos el daño de la bala
        playerDamage = bullet.GetComponent<PlayerBulletController>().BulletDamage;
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
    }

    protected void Die()
    {
        // Destruimos el gameObject
        Destroy(gameObject);
    }



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


    private IEnumerator Shooting(Vector3 pos, Quaternion rot)
    {
        // Instanciamos la bala en la posicion y la rotacion pasada por parametro
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        actualMultipleShoot--;

        // Pasar la referencia al objeto padre al objeto bullet
        bullet.GetComponent<EnemyBulletController>().SetParentObject(this);

        // Esperamos el CoolDown del arma para volver a disparar
        yield return new WaitForSeconds(FireRate);
        actualMultipleShoot = MultipleShoot;
    }
}