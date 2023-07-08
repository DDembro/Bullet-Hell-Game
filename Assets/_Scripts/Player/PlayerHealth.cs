using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Registro del controller del player y otros componentes
    private GameObject player;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Script UI del juego (Para llamar al metodo ShowDeathMenu)
    private GamePlayUI gamePlayUI;

    // Obtenemos Prefabs utiles
    private GameObject explosionPrefab;

    // Variables
    public float Health;
    private float inmuneTime;

    private void Awake()
    {
        // Obtenemos el jugador para simplificar la busqueda mas adelante
        player = GameObject.FindWithTag("Player");

        // Obtenemos la referencia al controller del jugador
        playerController = player.GetComponent<PlayerController>();
        // Obtenemos el SpriteRenderer y su RB
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        rb = player.GetComponent<Rigidbody2D>();

        // Obtenemos el prefab de la explosion
        explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
    }

    private void Start()
    {
        // Inicializamos las estadisticas
        Health = playerController._Health;
        inmuneTime = playerController._inmuneTime;

        // Obtenemos el UI
        gamePlayUI = GameObject.Find("GamePlayUI").GetComponent<GamePlayUI>();
    }

    private IEnumerator Die()
    {
        // Mostramos una explosion y desactivamos tanto visual como fisicamente al jugador
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        spriteRenderer.enabled = false;
        rb.simulated = false;
        player.GetComponent<PlayerShooting>().enabled = false;

        // Esperamos un poco antes de mostrar la pantalla de muerte
        yield return new WaitForSeconds(1.5f);

        // Llamamos al metodo encargado de mostrar la pantalla de muerte
        gamePlayUI.ShowDeathMenu();
    }

    private void TakeDamage(float damage)
    {
        // recibimos da�o
        Health -= damage;

        if(Health > 0)
        {
            StartCoroutine(ImmunityTime());
        }
        else
        {
            // Si nos quedamos sin vida, morimos
            StartCoroutine(Die());
        }
    }

    private IEnumerator ImmunityTime()
    {
        // Desactivamos su simulacion para que no pueda ser da�ado por proyectiles (ni activarlos)
        // Y cambiamos su color para generar una respuesta visual del efecto
        rb.simulated = false;

        // Primero lo pasamos a rojo, y lo hacemos transparente
        Color hitColor = Color.red;
        hitColor.a = 0.5f;
        spriteRenderer.color = hitColor;

        // Luego lo pasamos a su color original pero transparente
        yield return new WaitForSeconds(0.2f);
        hitColor = Color.white;
        hitColor.a = 0.5f;
        spriteRenderer.color = hitColor;

        // Al finalizar lo volvemos a la normalidad
        yield return new WaitForSeconds(inmuneTime - 0.2f);
        rb.simulated = true;
        spriteRenderer.color = Color.white;
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