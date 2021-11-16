using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{
    private float lastFire;
    private GameObject player;
    public FamiliarData familiar;
    private float lastOffsetX;      // Estos offsets se usan para que el familiar
    private float lastOffsetY;      // pueda moverse tomando al jugador como referencia

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Movimiento del familiar: datos
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Ataque del familiar: datos
        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        // Controla los disparos del familiar para que transcurra el tiempo adecuado entre disparo y disparo
        if ((shootHorizontal != 0 || shootVertical != 0) && (Time.time > lastFire + familiar.fireDelay))
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        // Si el jugador no se mueve, el familiar irá hacia la posición en la que este se encuentre
        if (horizontal != 0 || vertical != 0)
        {                                       /*si horizontal < 0*/    /*si horizontal >= 0*/
            float offsetX = (horizontal < 0) ? Mathf.Floor(horizontal) : Mathf.Ceil(horizontal);
            float offsetY = (vertical < 0) ? Mathf.Floor(vertical) : Mathf.Ceil(vertical);

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, familiar.speed * Time.deltaTime);
            lastOffsetX = offsetX;
            lastOffsetY = offsetY;
        }
        // Si el jugador se mueve, el familiar irá hacia la posición hacia la que el jugador se mueva
        else
        {
            if (!(transform.position.x < lastOffsetX + 0.5f) || !(transform.position.y < lastOffsetY + 0.5f))
            {
                transform.position = Vector2.MoveTowards(transform.position,
                                        new Vector2(player.transform.position.x - lastOffsetX, player.transform.position.y - lastOffsetY),
                                        familiar.speed * Time.deltaTime);

            }
        }
    }



    void Shoot(float x, float y)
    {
        // Crea un hechizo en la posicion del familiar
        GameObject spell = Instantiate(familiar.spellPrefab, transform.position, Quaternion.identity) as GameObject;
        float posX = (x < 0) ? Mathf.Floor(x) * familiar.speed : Mathf.Ceil(x) * familiar.speed;
        float posY = (y < 0) ? Mathf.Floor(y) * familiar.speed : Mathf.Ceil(y) * familiar.speed;

        // Definicion del movimiento y las propiedades del hechizo
        spell.AddComponent<Rigidbody2D>().gravityScale = 0;
        spell.GetComponent<Rigidbody2D>().velocity = new Vector2(posX, posY);
    }

}
