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

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currentState = EnemyState.Wander;
    
    public float range; // rango en el que el enemigo nos puede ver
    public float speed;
    public float coolDown;
    private bool coolDownAttack = false;

    public LayerMask whatIsPlayer;
    public float attackRange;
    public int baseDamage = 10;

    // Variables que sirven para que el enemigo avance hacia una direccion aleatoria (pulule)
    private bool chooseDir = false;
    private Vector3 randomDirection;

    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
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

    // Calcula la distancia entre el enemigo y el jugador y devuelve si esta en rango o no
    private bool IsPlayerInRange(float range) 
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    // Corrutina: funcion que tiene la habilidad de pausar su ejecucion y devolver el control a
    // Unity para luego continuar donde lo dejo en el siguiente frame.
    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        // cambiara la direccion en la que se mueve en un tiempo aleatorio entre 2 y 5 segundos
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        // Escoge una nueva direccion 
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        // Los cuarteniones sirven para representar las rotaciones en Unity
        // Quaternion.Euler retorna una rotacion basada en la direccion aleatoria que hemos calculado previamente
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        // Realiza la rotacion desde el punto de partida hasta la proxima rotacion a una velocidad, en este caso, aleatoria
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        // Si no ha elegido una direccion, la elige
        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        // Actualizamos la posicion
        transform.position += -transform.right * speed * Time.deltaTime;

        // Si el jugador esta en el rango le persigue
        if (IsPlayerInRange(range)) 
        {
            currentState = EnemyState.Follow;
        }
    }

    void Follow() 
    {
        // El enemigo se mueve desde su posicion hacia la del jugador a una velocidad determinada
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Attack()
    {
        if (!coolDownAttack)
        {
            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsPlayer);
            for (int i = 0; i < playerToDamage.Length; i++)
            {
                playerToDamage[i].GetComponent<PlayerController>().TakeDamage(baseDamage);
            }
            StartCoroutine(CoolDown());
        }
    }

    // Se espera un tiempo entre un ataque y otro
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
}
