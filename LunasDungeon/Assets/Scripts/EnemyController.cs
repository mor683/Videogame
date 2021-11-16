using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enumerado con los posibles estados en los que se puede encontrar un enemigo
public enum EnemyState
{
    Wander,
    Follow,
    Die,
    Attack
}; 

public enum EnemyType
{
    Melee,
    Ranged
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currentState = EnemyState.Wander;
    public EnemyType enemyType;

    // Animaciones
    public Animator animator;
    private float oldPos = 0.0f;

    public float range; // rango en el que el enemigo nos puede ver
    public float speed;
    public float coolDown;
    private bool coolDownAttack = false;

    public LayerMask whatIsPlayer;
    public float attackRange;
    public int baseDamage = 10;

    private Vector3 randomDirection;    // Direccion aleatoria a la que avanza el enemigo en Wander
    public GameObject spellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPos = transform.position.x;  // Actualiza la ultima posicion
        PositionChange();               // Elige una nueva direccion a la que ir
    }

    // Update is called once per frame
    void Update()
    {
        Animate();  // Animacion del enemigo

        switch (currentState)
        {
            case(EnemyState.Wander):
                Wander();
            break;
            case(EnemyState.Follow):
                Follow();
            break;
            case(EnemyState.Die):
                Death();
            break;
            case (EnemyState.Attack):
                Attack();
            break;
        }

        // Si el jugador esta en el rango y el enemigo no esta muerto, le persigue, sino sigue pululando
        if(IsPlayerInRange(range) && (currentState != EnemyState.Die))
        {
            currentState = EnemyState.Follow;
        } 
        else if(!IsPlayerInRange(range) && (currentState != EnemyState.Die))
        {
            currentState = EnemyState.Wander;
        }
        if(Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    void Wander()
    {
        // Si ya ha alcanzado la posicion elegida, elige una nueva
        if (Vector2.Distance(transform.position, randomDirection) < 1)
        {
            PositionChange();
        }

        // Actualizamos la posicion
        transform.position = Vector3.MoveTowards(transform.position, randomDirection, speed * Time.deltaTime);

        // Si el jugador esta en el rango le persigue
        if (IsPlayerInRange(range)) 
        {
            currentState = EnemyState.Follow;
        }
    }

    // Toma una nueva posicion destino a la que deambular
    void PositionChange()
    {
        randomDirection = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
    }

    // Calcula la distancia entre el enemigo y el jugador y devuelve si esta en rango o no
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void Follow() 
    {
        // El enemigo se mueve desde su posicion hacia la del jugador a una velocidad determinada
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime * 2);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Attack()
    {
        if (!coolDownAttack)
        {
            switch(enemyType)
            {
                case (EnemyType.Melee):
                    Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsPlayer);
                    for (int i = 0; i < playerToDamage.Length; i++)
                    {
                        playerToDamage[i].GetComponent<PlayerController>().TakeDamage(baseDamage);
                    }
                    StartCoroutine(CoolDown());
                    break;

                case (EnemyType.Ranged):
                    // Crea un hechizo en la posicion del enemigo
                    GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity) as GameObject;
                    spell.GetComponent<SpellController>().GetPlayer(player.transform);
                    spell.AddComponent<Rigidbody2D>().gravityScale = 0;
                    spell.GetComponent<SpellController>().isEnemySpell = true;
                    StartCoroutine(CoolDown());
                    break;
            }
        }
    }

    // Se espera un tiempo entre un ataque y otro
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    // Animacion del personaje
    void Animate()
    {
        float currentPosition = transform.position.x;   // Lee la posicion actual
        if (oldPos < currentPosition)
        {
            animator.SetFloat("Horizontal", 1);         // Se mueve a la derecha
        } else
        {
            animator.SetFloat("Horizontal", 0);         // Se mueve a la izquierda
        }
        oldPos = currentPosition;                       // Actualiza la ultima posicion
    }
}
