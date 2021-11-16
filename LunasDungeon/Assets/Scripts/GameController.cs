using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // Atributos del jugador
    private static float health;
    private static int maxHealth;
    private static float moveSpeed = 5f;
    private static float attackRate = 0.5f;
    public static float spellSize = 0.5f;

    //private bool cauldronCollected = false;
    //private bool wandCollected = false;

    //public List<string> collectedNames = new List<string>();

    // Propiedades
    public static float Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float AttackRate { get => attackRate; set => attackRate = value; }
    public static float SpellSize { get => spellSize; set => spellSize = value; }

    public Text healthText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (health <= 0)    // si la vida llega a 0, muere
        {
            KillPlayer();
        }
    }

    // Funciones para aumentar ciertos atributos en base a los items recolectados
    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount); //solo puede curarse hasta el máximo de vida
    }
    
    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void AttackRateChange(float rate)
    {
        attackRate -= rate;
    }

    public static void SpellSizeChange(float size)
    {
        spellSize += size;
    }

    /*public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach(string i in collectedNames)
        {
            switch (i)
            {
                case "Cauldron":
                    cauldronCollected = true;
                    break;
                case "Wand":
                    wandCollected = true;
                    break;
            }
        }

        if(wandCollected && cauldronCollected)
        {
            AttackRateChange(0.25f);
        }
    }*/

    private static void KillPlayer()
    {

    }
}
