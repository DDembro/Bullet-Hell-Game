using System.Collections;
using UnityEngine;

public class TurretController : EnemyController
{
    // Objeto del Cañon
    private Transform Cannon;

    // Referencia al jugador
    private GameObject player;

    // Otras variables
    private int bulletSpray = 3;
    private float sprayCoolDown = 1.5f;
    private bool canSpray = true;

    private int actualBullets = 3;

    private void Start()
    {
        // Inicializamos las variables
        this.Health = 5f;
        this.MelleDamage = 1f;
        this.HorizontalSpeed = 0f;
        this.VerticalSpeed = 0f;

        this.BulletSpeed = 8f;
        this.BulletDamage = 1f;
        this.FireRate = 0.2f;
        this.MultipleShoot = 1;

        this.OnDeathScore = 100;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/Bullets/EnemyBullet");

        // Obtenemos el cañon
        Cannon = transform.Find("Cannon");

        // Obtenemos al jugador
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        // Hacemos a la torreta mirar siempre al jugador
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, angle + 90);
        transform.rotation = desiredRotation;

        // Logica de disparo: disparamos una rafaba de balas y esperamos un enfriamiento

        if (canSpray)
        {
            // Disparamos y restamos una bala
            actualBullets--;
            StartCoroutine(Shoot(Cannon.position, Cannon.rotation));

            if (actualBullets <= 0)
            {
                // Si no quedan balas esperamos mas
                StartCoroutine(WaitSprayCoolDown(sprayCoolDown));
            }
            else
            {
                // Si no esperamos el tiempo normal
                StartCoroutine(WaitSprayCoolDown(FireRate));
            }
        }
    }

    private IEnumerator WaitSprayCoolDown(float time)
    {
        // Esperamos y reestablecemos las balas
        canSpray = false;
        yield return new WaitForSeconds(time);

        // Si no quedan mas balas las reestablecemos
        if(actualBullets <= 0)
        {
            actualBullets = bulletSpray;
        }
        canSpray = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con una bala
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("NeutralBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;

            // Efectuamos el daño
            TakeDamage(bullet);
        }

        // Al entrar en contacto con el jugador
        if (collision.CompareTag("Player"))
        {
            // Matamos al enemigo instantaneamente
            Die();
        }
    }
}