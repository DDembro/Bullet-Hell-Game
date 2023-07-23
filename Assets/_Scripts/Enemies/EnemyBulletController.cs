using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // Referencia al objeto padre que instancio la bala
    private EnemyController parentObject;

    // Atributos de la bala
    private float bulletSpeed;
    public float BulletDamage;

    // RigidBody de la bala
    private Rigidbody2D rb;

    public void SetParentObject(EnemyController parent)
    {
        // Definimos quien fue el enemigo que invoco la bala
        parentObject = parent;
    }

    private void Start()
    {
        // Generamos el sonido de la bala al ser disparada
        AudioSource shootSound = GetComponent<AudioSource>();
        shootSound.Play();

        // Definimos la velocidad y el daño segun las estadisticas del enemigo que la disparo
        bulletSpeed = parentObject.BulletSpeed;
        BulletDamage = parentObject.BulletDamage;

        // En caso de que la bala viaje hacia el infinito, la destruimos luego de unos segundos
        Destroy(gameObject, 3.5f);

        // Obtenemos el RB de la bala
        rb = GetComponent<Rigidbody2D>();

        // Aplicamos fuerza a la bala para desplazarla
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
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