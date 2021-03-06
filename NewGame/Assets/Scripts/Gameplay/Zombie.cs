﻿using System.Collections;
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
	private Animator anim;
	protected override void Spawn(){
		base.Spawn();
		player = GameObject.Find("Player");
		state = ZombieState.Idle;
		dmgModifier = 0;
		anim = GetComponent<Animator>();
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
					anim.SetBool("Follow", true);
				}
				break;
			case ZombieState.Following:
				if(distance < range){
					state = ZombieState.Aiming;
					anim.SetBool("Follow", false);
				}
				else{
					targetPos = player.transform.position;
				}
				Move(targetPos);
				break;
			case ZombieState.Aiming:
				anim.SetTrigger("Aim");
				state = ZombieState.WaitToAttack;
				atkWaitTime = 0f;
				break;
			case ZombieState.WaitToAttack:
				atkWaitTime += Time.deltaTime;
				if (atkWaitTime >= atkDelay) {
					state = ZombieState.Attacking;
					anim.SetTrigger("Atk");
					curAtkDistance = 0;
					targetPos = player.transform.position;
					atkV = targetPos - transform.position;
					SetAnimDir(atkV);
				}
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
				if (coolingT <= 0f) {
					state = ZombieState.Idle;
				}
				break;
		}
	}
	private void SetAnimDir(Vector3 dir) {
		if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
			anim.SetFloat("y", 0f);
			anim.SetFloat("x", Norm(dir.x));
        }
        else {
			anim.SetFloat("x", 0f);
			anim.SetFloat("y", Norm(dir.y));
        }
    }
	protected override void Move(Vector3 target) {
		Vector3 diff = target - transform.position;
		transform.position += diff.normalized * moveSpeed * Time.deltaTime;
		SetAnimDir(diff);
	}
	private float Norm(float x) {
		if (x == 0f) return 0f;
		return x / Mathf.Abs(x);
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