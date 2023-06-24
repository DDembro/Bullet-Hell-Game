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
        this.FireRate = 0.5f;
        this.MultipleShoot = 2;

        // Obtenemos el prefab de la bala que queremos utilizar y le asignamos las estadisticas que predefinimos arriba
        GetBullet("Prefabs/EnemyBullet");

        // Obtenemos los cañones del arma
        leftCannon = transform.Find("LeftCannon");
        rightCannon = transform.Find("RightCannon");
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con una bala
        if (collision.CompareTag("PlayerBullet"))
        {
            // Efectuamos el daño
            TakeDamage();
        }
    }
}