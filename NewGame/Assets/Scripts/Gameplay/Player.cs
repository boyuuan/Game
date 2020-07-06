using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	//[SerializeField]
	private float depth = 10f;
	private float speed = 0f;
	private PlayerState state;
	private Vector3 atkV;
	private Vector3 targetPos;
	protected override void Awake(){
		base.Awake();
		Debug.Log("player awake");
		atkV = Vector3.zero;
		state = PlayerState.Spawning;
		speed = Rules.Instance.PlayerSpeed;
	}
	
	
	protected override void Die(){
		GameManager.Instance.EndGame();
	}
	public override void TakeDamage(int dmg){
		hp -= dmg;
		if(hp <= 0) Die();
	}
	protected override void Attack(){
		
	}
	private IEnumerator SpawnWait(float s){
		yield return new WaitForSeconds(s);
		state = PlayerState.Idle;
	}
	private void UpdateState(){
		//Debug.Log("current state = " + state);
		switch(state){
			case PlayerState.Spawning:
				StartCoroutine(SpawnWait(2f));
				break;
			case PlayerState.Idle:
				if(GameManager.Instance.InputEnabled && Input.GetMouseButtonUp(0)){
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray))
						Debug.Log("dkslj");
					Debug.Log("MousePos = " + Input.mousePosition);
					var mousePos = Input.mousePosition;
					var wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, depth));
					atkV = wantedPos - transform.position;
					targetPos = atkV.normalized * Rules.Instance.PlayerAtkDistance + transform.position;
					state = PlayerState.PrepareToAttack;
				}
				break;
			case PlayerState.PrepareToAttack:
				state = PlayerState.Attacking;
				break;
			case PlayerState.Attacking:
				if(Vector3.Dot(atkV, transform.position - targetPos) > 0){
					state = PlayerState.Idle;
				}
				else{
					transform.position += atkV.normalized * speed * Time.deltaTime;
				}
				break;
			case PlayerState.Hurt:
				break;
			case PlayerState.Dead:
				break;
		}
	}
	
	void Update(){
		UpdateState();
	}
}
public enum PlayerState{
	Spawning,
	Idle,
	PrepareToAttack,
	Attacking,
	Hurt,
	Dead
}
