using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;

    public float damage = 3f;
    public float knockbackforce = 500f;

    public enum AttackDirection
    {
        Right,
        Left
    }

    public AttackDirection attackDirection;
    Vector2 rightAttackOffset;

    private void Start()
    {
        rightAttackOffset = transform.position;

        // Debugging if the collider is attached or not
        Debug.Log(swordCollider);
        
        // Check if swordCollider is assigned
        if (swordCollider == null)
        {
            Debug.LogError("Sword Collider is not assigned in the inspector.");
        }
    }

    public void AttackRight()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            transform.localPosition = rightAttackOffset;
        }
    }

    public void AttackLeft()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            transform.localPosition = new Vector2(-rightAttackOffset.x, rightAttackOffset.y);
        }
    }

    public void StopAttack()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDamagable damagable = (IDamagable) collider.GetComponent(typeof(IDamagable));

        if (damagable != null)
        {
            Vector3 parentPosition = transform.parent.position;
            
            Vector2 direction = (collider.transform.position - parentPosition ).normalized;
            Vector2 knockback = direction * knockbackforce;


            damagable.OnHit(damage, knockback);
        }
        else
        {
            Debug.Log("Does not implement IDamagable interface");
        }
    }

    Vector2 CalculateKnockback(Vector2 enemyPosition)
    {
        Vector2 knockback = new Vector2(enemyPosition.x - transform.position.x, enemyPosition.y - transform.position.y);
        return knockback.normalized;
    }
}
