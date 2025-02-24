using UnityEngine;
public interface ICharacter
{
    public void Move(Vector3 direction);
    public void TakeDamage(Vector3 attackPos, float damage);
    public void Attack();
    public void Die();
}
