using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float damage = 3f;

    public float knockbackforce = 50f;

    public float movementSpeed = 500f;

    public DetectionZone detectionZone;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (detectionZone.detectedObjects.Count > 0)
        {
            Vector2 direction = (detectionZone.detectedObjects[0].transform.position - transform.position).normalized;

            rb.AddForce(direction * movementSpeed * Time.deltaTime);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.OnHit(damage);

            Vector3 parentPosition = transform.position;
            
            Vector2 direction = (collider.transform.position - parentPosition ).normalized;
            Vector2 knockback = direction * knockbackforce;

            damagable.OnHit(damage, knockback);
        }
    }
}
