using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Personaje
    public float speed;
    Rigidbody2D rigidbody;

    // Animaciones
    public Animator animator;
    private Vector2 ultimaDireccion;    // Guarda la ultima direccion para elegir el idle
    private Vector2 movimiento;
    private Vector2 ataque;

    // Ataque
    public GameObject spellPrefab;
    public float spellSpeed;
    private float lastFire;
    public float fireDelay;

    // Vida
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento del personaje: datos
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Comprueba si debe guardar la ultima direccion de movimiento
        if ((horizontal == 0 && vertical == 0) && movimiento.x != 0 || movimiento.y != 0)
        {
            ultimaDireccion = movimiento;
        }

        // Guarda el movimiento en un vector
        movimiento = new Vector2(horizontal, vertical);

        // Ataque del personaje: datos
        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");
        ataque = new Vector2(shootHorizontal, shootVertical);

        // Controla los disparos del personaje para que transcurra el tiempo adecuado entre disparo y disparo
        if ((shootHorizontal != 0 || shootVertical != 0) && (Time.time > lastFire + fireDelay)) 
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        // Animacion del personaje
        Animate();

        // Movimiento del personaje: accion
        rigidbody.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void Shoot(float x, float y)
    {
        // Crea un hechizo en la posicion del jugador
        GameObject spell = Instantiate(spellPrefab, transform.position, transform.rotation) as GameObject;
        // Definicion del movimiento y las propiedades del hechizo
        spell.AddComponent<Rigidbody2D>().gravityScale = 0;
        spell.GetComponent<Rigidbody2D>().velocity = new Vector2(
            // si x < 0 hacemos que se mueva en el eje negativo, sino en el eje positivo
            (x < 0) ? Mathf.Floor(x) * spellSpeed : Mathf.Ceil(x) * spellSpeed,
            // si y < 0 hacemos que se mueva en el eje negativo, sino en el eje positivo
            (y < 0) ? Mathf.Floor(y) * spellSpeed : Mathf.Ceil(y) * spellSpeed
        );
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void Heal(int healAmount)
    {
        // De esta manera el jugador no supera su vida maxima
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        healthBar.SetHealth(currentHealth);
    }

    void Animate()
    {
        // Animacion del personaje
        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Vertical", movimiento.y);
        animator.SetFloat("Magnitude", movimiento.magnitude);
        animator.SetFloat("UltimaDirHorizontal", ultimaDireccion.x);
        animator.SetFloat("UltimaDirVertical", ultimaDireccion.y);
        animator.SetFloat("AttackHorizontal", ataque.x);
        animator.SetFloat("AttackVertical", ataque.y);
        animator.SetFloat("AttackMagnitude", ataque.magnitude);
    }

}
