using UnityEngine;
public interface ICharacter
{
    public void Move();
    public void TakeDamage(Vector3 attackPos, float damage);
    public void Attack();
    public void Die();
}
