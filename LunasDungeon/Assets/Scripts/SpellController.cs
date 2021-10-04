using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{

    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        // Destruyo el objeto pasado un tiempo para que no se este moviendo infinitamente
        StartCoroutine(DeathDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }
    }
}