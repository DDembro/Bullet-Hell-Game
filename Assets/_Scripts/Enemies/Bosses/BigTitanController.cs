using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTitanController : EnemyController
{
    // Vectores de movimiento
    private Vector3 verticalMovement;

    // Cañones de donde se instancia la bala
    private Transform leftCannon;
    private Transform rightCannon;

    // Otras variables
    private float verticalTarget = 6f;

    private void Start()
    {
        // Inicializamos las variables
        this.Health = 50f;
        this.MelleDamage = 3f;
        this.HorizontalSpeed = 3f;
        this.VerticalSpeed = 1f;

        this.BulletSpeed = 0f; // No afecta en este enemigo
        this.BulletDamage = 0f; // No afecta en este enemigo
        this.FireRate = 5f;
        this.MultipleShoot = 2;

        this.OnDeathScore = 20000;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/Bullets/BigRocket");

        // Obtenemos los cañones del arma
        leftCannon = transform.Find("LeftCannon");
        rightCannon = transform.Find("RightCannon");
    }

    private void Update()
    {
        // Movimiento vertical
        if (transform.position.y > verticalTarget)
        {
            verticalMovement = new Vector3(0, -VerticalSpeed * Time.deltaTime, 0);
        }
        else
        {
            verticalMovement = Vector3.zero;
        }
        // Aplicamos el movimiento
        transform.Translate(verticalMovement);

        // Disparamos con ambas armas
        StartCoroutine(Shoot(leftCannon.position, leftCannon.rotation));
        StartCoroutine(Shoot(rightCannon.position, rightCannon.rotation));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con una bala
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("NeutralBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;

            // Efectuamos el daño
            TakeDamageBoss(bullet, 8f);
        }
    }
}