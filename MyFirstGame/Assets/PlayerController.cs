using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Personaje
    public float speed;
    Rigidbody2D rigidbody;

    // Ataque
    public GameObject spellPrefab;
    public float spellSpeed;
    private float lastFire;
    public float fireDelay;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 

        float shootHorizontal = Input.GetAxis("shootHorizontal");
        float shootVertical = Input.GetAxis("shootVertical"); 
        if ((shootHorizontal != 0 || shootVertical != 0) && (Time.time > lastFire + fireDelay)) 
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        rigidbody.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void Shoot(float x, float y)
    {
        GameObject spell = Instantiate(spellPrefab, transform.position, transform.rotation) as GameObject;
        spell.AddComponent<Rigidbody2D>().gravityScale = 0;
        spell.GetComponent<Rigidbody2D>().velocity = new Vector2(
            // si x < 0 hacemos que se mueva en el eje negativo, sino en el eje positivo
            (x < 0) ? Mathf.Floor(x) * spellSpeed : Mathf.Ceil(x) * spellSpeed,
            // si y < 0 hacemos que se mueva en el eje negativo, sino en el eje positivo
            (y < 0) ? Mathf.Floor(y) * spellSpeed : Mathf.Ceil(y) * spellSpeed
        );
    }
}
