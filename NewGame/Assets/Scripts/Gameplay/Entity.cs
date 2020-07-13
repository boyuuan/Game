using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EType{
	None = 0,
	Player,
	Zombie,
	Archer
}
public abstract class Entity : MonoBehaviour
{
	public EType EntityType;
	protected int hp;
	protected int damage;
	protected int dmgModifier;
	public virtual int Dmg{
		get{
			return damage * dmgModifier;
		}
	}
	protected float moveSpeed;
	protected float atkSpeed;
	protected float range;
	protected float atkDistance;
	protected float spawnTimer;
	protected int maxCount;
	protected float atkCoolDown;
	
	protected virtual void Spawn(){
		if(!Rules.Instance.EntityData.ContainsKey(EntityType)){
			Debug.Log("Can't find entityType = " + EntityType + " in rules");
			return;
		}
		hp = Rules.Instance.EntityData[EntityType].MaxHP;
		damage = Rules.Instance.EntityData[EntityType].Dmg;
		moveSpeed = Rules.Instance.EntityData[EntityType].MoveSpeed;
		atkSpeed = Rules.Instance.EntityData[EntityType].AtkSpeed;
		range = Rules.Instance.EntityData[EntityType].Range;
		atkDistance = Rules.Instance.EntityData[EntityType].AtkDistance;
		spawnTimer = Rules.Instance.EntityData[EntityType].SpawnTimer;
		maxCount = Rules.Instance.EntityData[EntityType].MaxCount;
		atkCoolDown = Rules.Instance.EntityData[EntityType].AtkCoolDown;
		dmgModifier = 1;
	}
	protected abstract void Die();
	public void TakeDmg(int dmg){
		hp -= dmg;
		if(hp <= 0) Die();
	}
	public bool IsDead(){
		return hp == 0;
	}
    protected void Awake()
    {
        Spawn();
    }
	protected virtual void Move(Vector3 target){
		Vector3 diff = target - transform.position;
		transform.position += diff.normalized * moveSpeed * Time.deltaTime;
	}
    void Update()
    {
    }
}

public class EntityData{
	public EType EntityType;
	public int MaxHP;
	public float MoveSpeed;
	public float AtkSpeed;
	public float Range;
	public int Dmg;
	public float AtkDistance;
	public float SpawnTimer;
	public int MaxCount;
	public float AtkCoolDown;
	public EntityData(EType type){
		EntityType = type;
		MaxHP = 0;
		MoveSpeed = 0f;
		AtkSpeed = 0f;
		Range = 0f;
		Dmg = 0;
		AtkDistance = 0f;
		SpawnTimer = 0f;
		MaxCount = 0;
	}
}
