using UnityEngine;

public interface IAIRangeWeapon
{
	public int Bullets { get; set; }

	public void Reload();

	public void Attack(GameObject target, AIAnimation animator, float damage = 10);
}