using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
	[SerializeField]
	private ZombieState state;
	[SerializeField]
	private GameObject player;
	private float distance;
	private Vector3 targetPos;
	private Vector3 atkV;
	private float atkDelay = 1f;
	private float atkWaitTime = 0f;
	private float coolingT = 0f;
	private float curAtkDistance = 0f;
	protected override void Awake(){
		base.Awake();
		player = GameObject.Find("Player");
		state = ZombieState.Idle;
	}
	protected override void Die(){
		Destroy(gameObject);
		GameManager.Instance.WinGame();
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
				if(curAtkDistance < atkDistance && Vector3.Dot(targetPos - transform.position, atkV) > 0){
					transform.position += atkV.normalized * atkSpeed * Time.deltaTime;
					curAtkDistance += atkSpeed * Time.deltaTime;
				}
				else{
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