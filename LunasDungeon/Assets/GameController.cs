using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Creamos una instancia estatica de esta clase porque queremos que sea la misma siempre, no queremos replicarla
    public static GameController instance;

    // Variables que vamos a necesitar (tienen que ser estaticas)
    private static int health = 10;
    private static int maxHealth = 10;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;

    // Variables de la UI
    public Text healthText;

    // Propiedades
    public static int Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }

    // Uso del patron Singleton: despues de que se inicialice en el metodo Awake(), vamos a poder llamarlo desde cualquier parte del juego
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(int healAmount)
    {
        // De esta manera el jugador no supera su vida maxima
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    private static void KillPlayer()
    {

    }
}
