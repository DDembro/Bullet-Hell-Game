using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBulletController : MonoBehaviour
{
    // Referencia al objeto padre que instancio la bala
    private RocketController parentRocket;

    // Atributos de la bala
    private float bulletSpeed;
    public float BulletDamage;
    private float bulletDuration;

    // RigidBody de la bala
    private Rigidbody2D rb;

    public void SetParentRocket(RocketController parent)
    {
        // Definimos quien fue el enemigo que invoco la bala
        parentRocket = parent;
    }

    private void Start()
    {
        // Definimos la velocidad y el daño segun las estadisticas del enemigo que la disparo
        bulletSpeed = parentRocket.BulletSpeed;
        BulletDamage = parentRocket.BulletDamage;
        bulletDuration = parentRocket.BulletDuration;

        // En caso de que la bala viaje hacia el infinito, la destruimos luego de unos segundos
        Destroy(gameObject, bulletDuration);

        // Obtenemos el RB de la bala
        rb = GetComponent<Rigidbody2D>();

        // Aplicamos fuerza a la bala para desplazarla
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al colisionar con un enemigo la bala se destruye
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
