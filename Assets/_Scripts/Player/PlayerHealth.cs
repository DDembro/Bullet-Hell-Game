using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Variables
    public float Health;

    private void Awake()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Inicializamos las estadisticas
        Health = playerController._Health;
    }

    private void TakeDamage(float damage)
    {
        // recibimos da�o
        Health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con una bala enemiga
        if (collision.CompareTag("EnemyBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;
            
            // Obtenemos el da�o de esa bala
            float bulletDamage = bullet.GetComponent<EnemyBulletController>().BulletDamage;
            
            // Efectuamos el da�o
            TakeDamage(bulletDamage);
        }

        // Al entrar en contacto con una bala neutral
        if (collision.CompareTag("NeutralBullet"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject bullet = collision.gameObject;

            // Obtenemos el da�o de esa bala
            float bulletDamage = bullet.GetComponent<NeutralBulletController>().BulletDamage;

            // Efectuamos el da�o
            TakeDamage(bulletDamage);
        }

        // Al entrar en contacto con un cohete
        if (collision.CompareTag("Rocket"))
        {
            // Obtenemos la bala que nos choco, para poder usarla de parametro en TakeDamage
            GameObject rocket = collision.gameObject;

            // Obtenemos el da�o de ese cohete
            float rocketDamage = rocket.GetComponent<RocketController>().RocketDamage;

            // Efectuamos el da�o
            TakeDamage(rocketDamage);
        }

        // Al entrar en contacto con el jugador
        if (collision.CompareTag("Enemy"))
        {
            // Obtenemos al enemigo que nos choco
            GameObject enemy = collision.gameObject;

            // Obtenemos su da�o de melle
            float melleDamage = enemy.GetComponent<EnemyController>().MelleDamage;

            // Efectuamos el da�o
            TakeDamage(melleDamage);
        }
    }
}