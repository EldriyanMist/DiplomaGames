using UnityEngine;

public interface IDamagable
{
    public float Health { get; set; }
    public bool Targatable { get; set; }
    public void OnHit(float damage, Vector2 knockbackDirection);
    public void OnHit(float damage);
    public void OnObjectDestroyed();
}