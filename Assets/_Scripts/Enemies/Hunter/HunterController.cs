using UnityEngine;

public class HunterController : EnemyController
{
    // Vector de movimiento
    private Vector3 movement;

    // Cañones de donde se instancia la bala
    private Transform leftCannon;
    private Transform rightCannon;

    // Otras variables
    private float amplitude = 3;

    // Referencia al jugador
    private GameObject player;

    private void Start()
    {
        // Inicializamos las variables
        this.Health = 5f;
        this.Damage = 1f;
        this.MelleDamage = 1f;
        this.HorizontalSpeed = 5f;
        this.VerticalSpeed = 4f;
        
        this.BulletSpeed = 5f;
        this.BulletDamage = 1f;
        this.FireRate = 1f;
        this.MultipleShoot = 2;

        this.OnDeathScore = 100;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/Bullets/EnemyBullet");

        // Obtenemos los cañones del arma
        leftCannon = transform.Find("LeftCannon");
        rightCannon = transform.Find("RightCannon");

        // Obtenemos al jugador
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        // Calculamos el movimiento
        movement = new Vector3(amplitude * Mathf.Sin(Time.time * HorizontalSpeed) * Time.deltaTime, VerticalSpeed * Time.deltaTime, 0);

        // Aplicamos el movimiento
        transform.Translate(movement);

        // Disparamos con ambas armas
        StartCoroutine(Shoot(leftCannon.position, leftCannon.rotation));
        StartCoroutine(Shoot(rightCannon.position, rightCannon.rotation));

        // Si estamos a la altura del jugador, aumentamos la cadencia de disparo significantemente
        if(transform.position.y < player.transform.position.y + 1.5f && transform.position.y > player.transform.position.y - 1.5f)
        {
            FireRate = 0.25f;
        }
        else
        {
            FireRate = 1f;
        }

        // Si se alejo demasiado de la pantalla del jugador, lo destruimos
        if(transform.position.y < -15)
        {
            Destroy(gameObject);
        }
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