using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    Animator anim;

    bool isAlive = true;
    public bool disableSimulation = false;

    Rigidbody2D rb;

    Collider2D physicsCollider;
    // Start is called before the first frame update

    public float Health {
        get { return health; }
        set {
            print(value < health);
            if (value < health)
            {
                anim.SetTrigger("hit");
            }
            health = value;



            if (health <= 0)
            {
                anim.SetBool("isAlive", false);
                Targatable = false;
            }
        }
    }

    public bool Targatable {
        get { return _tagatable; }
        set { _tagatable = value; 
            if(disableSimulation){
                rb.simulated = false;
            }
            physicsCollider.enabled = value;
        }
    }

    public float health = 1f;
    public bool _tagatable = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isAlive", isAlive);

        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
    }
    
    public void OnHit(float damage)
    {
        Health -= damage;
        anim.SetTrigger("hit");
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        anim.SetTrigger("hit");
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    public void Defeated()
    {
        anim.SetTrigger("hit");
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
}
