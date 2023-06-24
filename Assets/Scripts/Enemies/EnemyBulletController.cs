using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // Atributos de la bala
    public float BulletSpe;
    public float BulletDam;

    // RigidBody de la bala
    private Rigidbody2D rb;

    private void Start()
    {
        // En caso de que la bala viaje hacia el infinito, la destruimos luego de unos segundos
        Destroy(gameObject, 3.5f);

        // Obtenemos el RB de la bala
        rb = GetComponent<Rigidbody2D>();

        // Aplicamos fuerza a la bala para desplazarla
        rb.AddForce(transform.up * BulletSpe, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al colisionar con un enemigo la bala se destruye
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
