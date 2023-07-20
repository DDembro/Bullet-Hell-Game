using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    // Registro del controller del player
    private PlayerController playerController;

    // Variables de la clase
    public float PlayerScore;
    public float PlayerMoney;

    private void Awake()
    {
        // Obtenemos la referencia al controller del jugador
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void AddScore(float score)
    {
        // Aumentamos el puntaje
        PlayerScore += score;
    }

    /// <summary>
    /// Esta funcion se encarga de añadir dinero al jugador. Es llamada por GamePlayUI luego de haber ganado un nivel
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(float money)
    {
        // Aumentamos el dinero del jugador
        PlayerMoney += money;
    }

    public void ResetScore()
    {
        PlayerScore = 0;
    }

    /// <summary>
    /// El metodo Paid Retorna TRUE si fue posible comprar, y disminuye el dinero
    /// En caso contrario retorna FALSE y no hace nada
    /// </summary>
    /// <param name="cost"></param>
    public bool Paid(float cost)
    {
        if (PlayerMoney >= cost)
        {
            PlayerMoney -= cost;
            return true;
        }

        return false;
    }

}