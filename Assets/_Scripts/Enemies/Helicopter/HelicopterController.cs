using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : EnemyController
{
    // Cañones de donde se instancia la bala
    private Transform cannon;

    // rb del helicoptero
    private Rigidbody2D rb;

    // Otras variables

    // La posicion objetivo del helicoptero
    private float targetPositionX; // Posicion del jugador
    private float randomOffSetRange = 1.5f;

    private float targetPositionY; // Generada de forma aleatoria
    private float maxPositionY = 9f;
    private float minPositionY = 5f;
    // Referencia al jugador
    private GameObject player;

    private void Start()
    {
        // Inicializamos las variables
        this.Health = 5f;
        this.Damage = 1f;
        this.MelleDamage = 1f;
        this.HorizontalSpeed = 15f;
        this.VerticalSpeed = 10f;

        this.BulletSpeed = 10f;
        this.BulletDamage = 1f;
        this.FireRate = 0.8f;
        this.MultipleShoot = 1;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/EnemyBullet");

        // Obtenemos los cañones del arma
        cannon = transform.Find("Cannon");

        // Obtenemos el RB
        rb = GetComponent<Rigidbody2D>();

        // Obtenemos al jugador
        player = GameObject.FindWithTag("Player");

        // Generamos aleatoriamente una posicion en Y target
        InvokeRepeating("RandomPositionY", 0f, 2.5f);
        // Generamos aleatoriamente un offset para X target
        InvokeRepeating("RandomPositionX", 0f, 1f);
    }

    private void RandomPositionY()
    {
        targetPositionY = Random.Range(minPositionY, maxPositionY);
    }

    private void RandomPositionX()
    {
        // Obtenemos la posicion en X del jugador, y le añadimos un error para aleatoriezarlo
        targetPositionX = player.transform.position.x + Random.Range(-randomOffSetRange, randomOffSetRange);
    }

    private void Update()
    {
        // Logica del movimiento Horizontal

        // Aplicamos fuerza para desplazar al helicoptero a la posicion del jugador para ambas direcciones, tambien añadimos una tolerancia
        if (transform.position.x > targetPositionX + 1f)
        {
            rb.AddForce(transform.right * HorizontalSpeed * -1 * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (transform.position.x < targetPositionX - 1f)
        {
            rb.AddForce(transform.right * HorizontalSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Logica del movimiento Vertical

        // Aplicamos fuerza hasta que se encuentre a la altura deseada
        if (transform.position.y > targetPositionY + 0.5f)
        {
            rb.AddForce(transform.up * VerticalSpeed * -1 * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (transform.position.y < targetPositionY - 0.5f)
        {
            rb.AddForce(transform.up * VerticalSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Disparamos
        StartCoroutine(Shoot(cannon.position, cannon.rotation));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con una bala   
        if (collision.CompareTag("PlayerBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;

            // Efectuamos el daño
            TakeDamage(bullet);
        }
    }
}
