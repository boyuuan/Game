using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType{
	Player,
	Zombie,
	Archer
}
public abstract class Entity : MonoBehaviour
{
	public EntityType entityType;
	[SerializeField]
	protected int hp = 0;
	protected int damage;
	protected void Spawn(){
		if(!Rules.Instance.maxHP.ContainsKey(entityType)){
			print("Can't find entityType = " + entityType + " in rules");
			return;
		}
		hp = Rules.Instance.maxHP[entityType];
		print(entityType + " spawned with hp = " + hp);
	}
	protected abstract void Die();
	public abstract void TakeDamage(int dmg);
	protected abstract void Attack();
    protected virtual void Awake()
    {
		Debug.Log("entity awake");
        Spawn();
    }

    void Update()
    {
        
    }
}
