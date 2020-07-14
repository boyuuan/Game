using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
	//[SerializeField]
	private ZombieState state;
	//[SerializeField]
	private GameObject player;
	private float distance;
	private Vector3 targetPos;
	private Vector3 atkV;
	private float atkDelay = 1f;
	private float atkWaitTime = 0f;
	private float coolingT = 0f;
	private float curAtkDistance = 0f;
	protected override void Spawn(){
		base.Spawn();
		player = GameObject.Find("Player");
		state = ZombieState.Idle;
		dmgModifier = 0;
	}
	protected override void Die(){
		hp = 0;
		gameObject.SetActive(false);
	}
	public override int Dmg{
		get{
			return damage * dmgModifier;
		}
	}
	private void UpdateZombieState(){
		switch(state){
			case ZombieState.Idle:
				if(distance < range){
					state = ZombieState.Aiming;
				}
				else{
					state = ZombieState.Following;
				}
				break;
			case ZombieState.Following:
				if(distance < range){
					state = ZombieState.Aiming;
				}
				else{
					targetPos = player.transform.position;
				}
				Move(targetPos);
				break;
			case ZombieState.Aiming:
				state = ZombieState.WaitToAttack;
				atkWaitTime = 0f;
				break;
			case ZombieState.WaitToAttack:
				targetPos = player.transform.position;
				atkV = targetPos - transform.position;
				atkWaitTime += Time.deltaTime;
				if(atkWaitTime >= atkDelay) state = ZombieState.Attacking;
				curAtkDistance = 0;
				break;
			case ZombieState.Attacking:
				if(curAtkDistance < atkDistance){
					transform.position += atkV.normalized * atkSpeed * Time.deltaTime;
					curAtkDistance += atkSpeed * Time.deltaTime;
					dmgModifier = 1;
				}
				else{
					dmgModifier = 0;
					state = ZombieState.Cooling;
					coolingT = atkCoolDown;
				}
				break;
			case ZombieState.Cooling:
				coolingT -= Time.deltaTime;
				if(coolingT <= 0f) state = ZombieState.Idle;
				break;
		}
	}
	
    void Update()
    {
		if(GameManager.Instance.GameState != EGameState.Running)
			return;
		if(state != ZombieState.Attacking){
			if(player == null)
				Debug.LogWarning("can't find player");
			distance = Vector3.Distance(transform.position, player.transform.position);
		}
        UpdateZombieState();
    }
}
public enum ZombieState{
	Idle,
	Following,
	Aiming,
	WaitToAttack,
	Attacking,
	Cooling
}