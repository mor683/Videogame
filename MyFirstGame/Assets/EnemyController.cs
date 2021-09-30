using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Follow,
    Die
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currentState = EnemyState.Wander;
    
    public float range; // rango en el que el enemigo nos puede ver
    public float speed;

    private bool chooseDir = false;
    private bool dead = false;
    private Vector3 randomDirection;
    

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
        }

        if(IsPlayerInRange(range) && (currentState != EnemyState.Die))
        {
            currentState = EnemyState.Follow;
        } 
        else if(!IsPlayerInRange(range) && (currentState != EnemyState.Die))
        {
            currentState = EnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range) 
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        // cambiara la direccion en la que se mueve en un tiempo aleatorio entre 2 y 8 segundos
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        
        if(IsPlayerInRange(range)) 
        {
            currentState = EnemyState.Follow;
        }
    }

    void Follow() 
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
