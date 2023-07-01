using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanController : EnemyController
{
    // Vectores de movimiento
    private Vector3 horizontalMovement;
    private Vector3 verticalMovement;
    private int moveRight = 1;

    // Cañones de donde se instancia la bala
    private Transform leftCannon;
    private Transform rightCannon;

    // Otras variables
    private float horizontalRange = 6f;
    private float verticalTarget = 8.5f;

    private void Start()
    {
        // Inicializamos las variables
        this.Health = 10f;
        this.Damage = 1f; // No afecta en este enemigo
        this.MelleDamage = 0f;
        this.HorizontalSpeed = 3f;
        this.VerticalSpeed = 2f;

        this.BulletSpeed = 0f; // No afecta en este enemigo
        this.BulletDamage = 1f; // No afecta en este enemigo
        this.FireRate = 5f;
        this.MultipleShoot = 2;

        this.OnDeathScore = 1500;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/Bullets/BigRocket");

        // Obtenemos los cañones del arma
        leftCannon = transform.Find("LeftCannon");
        rightCannon = transform.Find("RightCannon");
    }

    private void Update()
    {
        // Calculamos el movimiento

        // Movimiento horizontal
        if (transform.position.x < -horizontalRange)
        {
            moveRight = 1; // Moverse a la derecha
        }
        else if(transform.position.x > horizontalRange)
        {
            moveRight = -1; // Moverse a la izquierda
        }
        horizontalMovement = new Vector3(moveRight * HorizontalSpeed * Time.deltaTime, 0, 0);

        // Movimiento vertical
        if (transform.position.y > verticalTarget)
        {
            verticalMovement = new Vector3(0, -VerticalSpeed * Time.deltaTime, 0);
        }
        else
        {
            verticalMovement = Vector3.zero;
        }
        // Vector final del movimiento
        Vector3 movement = horizontalMovement + verticalMovement;

        // Aplicamos el movimiento
        transform.Translate(movement);

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
            TakeDamageBoss(bullet, 4f);
        }
    }
}