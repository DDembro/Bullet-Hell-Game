using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Atributos de la bala
    public float BulletSpeed;
    public float BulletDamage;

    // RigidBody de la bala
    private Rigidbody2D rb;

    private void Start()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Inicializamos las estadisticas
        BulletSpeed = playerController._BulletSpeed;
        BulletDamage = playerController._BulletDamage;

        // En caso de que la bala viaje hacia el infinito, la destruimos luego de unos segundos
        Destroy(gameObject, 3.5f);

        // Obtenemos el RB de la bala
        rb = GetComponent<Rigidbody2D>();

        // Aplicamos fuerza a la bala para desplazarla
        rb.AddForce(transform.up * BulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al colisionar con un enemigo la bala se destruye
        if (collision.CompareTag("Enemy") || collision.CompareTag("Rocket"))
        {
            Destroy(gameObject);
        }
    }
}