using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{

    public float lifeTime;
    public bool isEnemySpell = false;
    private Vector2 lastPos;
    private Vector2 currentPos;
    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        // Destruyo el objeto pasado un tiempo para que no se este moviendo infinitamente
        StartCoroutine(DeathDelay());
        if (!isEnemySpell)
        {
            transform.localScale = new Vector2(GameController.SpellSize, GameController.SpellSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemySpell)
        {
            currentPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if (currentPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = currentPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }
        
    // Corrutina: funcion que tiene la habilidad de pausar su ejecucion y devolver el control a
    // Unity para luego continuar donde lo dejo en el siguiente frame.
    IEnumerator DeathDelay()
    {
        // Espera un tiempo y luego destruye el objeto tipo spell
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    // Si el hechizo da a un enemigo, se destruye al enemigo y el hechizo desaparece tambien
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy" && !isEnemySpell)
        {
            col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if (col.tag == "Player" && isEnemySpell)
        {
            GameController.DamagePlayer(20);
            Destroy(gameObject);
        }
    }
}