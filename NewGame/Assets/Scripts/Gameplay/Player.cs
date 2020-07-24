using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Entity
{
	[SerializeField]
	private float dragOrSprint = .1f;
	private float minDistanceThreshold = .1f;
	private float tunex = .64f;
	private float tuney = .55f;
	private float depth = 10f;
	//[SerializeField]
	private PlayerState state;
	private Vector3 atkV;
	private Vector3 targetPos;
	private Vector2 screen;
	private Vector2 edgeV;
	private float timeSinceBtnDown;
	private bool btnDown;
	private Animator anim;
	private float deltaX;
	private bool coolingDown = false;
	private float animX;
	private float animY;
	private GameObject _light = null;
	private GameObject trail = null;
	[SerializeField]
	private GameObject blackLight = null;
	[SerializeField]
	private GameObject redLight = null;
	[SerializeField]
	private float scale = 0f;
	protected override void Spawn(){
		base.Spawn();
		screen = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		atkV = Vector3.zero;
		state = PlayerState.Spawning;
		timeSinceBtnDown = 0f;
		anim = GetComponent<Animator>();
		_light = transform.Find("Light").gameObject;
		trail = blackLight;
	}
	
	protected override void Die(){
		GameManager.Instance.LoseGame();
	}
	private IEnumerator SpawnWait(float s){
		yield return new WaitForSeconds(s);
		state = PlayerState.Idle;
	}
	private Vector3 CalcActualDest(Vector3 start, float d, Vector3 v, float w, float h){
		Vector3 target = start + d * v;
		Vector3 temp = new Vector3(Mathf.Clamp(target.x, -w, w), Mathf.Clamp(target.y, -h, h), 0);
		float actualD;
		if(temp.x != target.x)
			actualD = Mathf.Abs((start.x - temp.x) / (start.x - target.x)) * d;
		else if(temp.y != target.y)
			actualD = Mathf.Abs((start.y - temp.y) / (start.y - target.y)) * d;
		else
			actualD = d;
		return start + actualD * v;
	}
	private void UpdateState(){
		switch(state){
			case PlayerState.Spawning:
				StartCoroutine(SpawnWait(.1f));
				break;
			case PlayerState.Idle:
				//TODO #IF UNITY_IOS, GetTouch(0)
				_light.gameObject.SetActive(false);
				if (btnDown){
					timeSinceBtnDown += Time.deltaTime;
					if(timeSinceBtnDown > dragOrSprint){
						state = PlayerState.Running;
						anim.SetTrigger("Running");
						anim.SetBool("Idle", false);
					}
				}
				if(!btnDown && GameManager.Instance.InputEnabled && Input.GetMouseButtonDown(0)){
					timeSinceBtnDown = 0f;
					btnDown = true;
				}
				if(GameManager.Instance.InputEnabled && Input.GetMouseButtonUp(0)){
					//Sprinting and Dragging have the same targetPos, the difference is
					//Sprinting won't stop until targetPos is reached while Drag will immediately stop while the button is released.
					var mousePos = Input.mousePosition;
					var wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, depth));
					atkV = (wantedPos - transform.position).normalized;
					edgeV = new Vector2(screen.x - tunex, screen.y - tuney);
					targetPos = CalcActualDest(transform.position, atkDistance, atkV, edgeV.x, edgeV.y);
					state = PlayerState.PrepareToAttack;
					btnDown = false;
				}
				break;
			case PlayerState.Running:
				if(Input.GetMouseButtonUp(0)){  //Stop Running Immediately
					btnDown = false;
					state = PlayerState.Idle;
					anim.SetBool("Idle", true);
					anim.SetFloat("deltaX", Norm(deltaX));
				}
				else{	//Keep running
					if(!btnDown) Debug.LogWarning("error");
					var mousePos = Input.mousePosition;
					var targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
					var delta = targetPos - transform.position;
					var distance = Vector3.Distance(targetPos, transform.position);
					if(distance > minDistanceThreshold)
						transform.position += delta.normalized * moveSpeed * Time.deltaTime;
					deltaX = delta.x;
					animX = delta.x;
					animY = delta.y;
					if(Mathf.Abs(animX) < Mathf.Abs(animY)) {
						animX = 0f;
						animY = animY / Mathf.Abs(animY);
                    }
                    else {
						animY = 0f;
						animX = animX / Mathf.Abs(animX);
                    }
					anim.SetFloat("x", animX);
					anim.SetFloat("y", animY);
				}
				break;
			case PlayerState.PrepareToAttack:
				state = PlayerState.Attacking;
				anim.SetBool("Idle", false);
				anim.SetTrigger("Attack");
				_light.gameObject.SetActive(true);
				trail.GetComponent<TrailRenderer>().time = Vector3.Distance(targetPos, transform.position) / atkSpeed + atkCoolDown;
				animX = atkV.x;
				animY = atkV.y;
				if(Mathf.Abs(atkV.x) > Mathf.Abs(atkV.y)) {
					animY = 0f;
					animX = animX / Mathf.Abs(animX);
                }
                else {
					animX = 0f;
					animY = animY / Mathf.Abs(animY);
                }
				anim.SetFloat("x", animX);
				anim.SetFloat("y", animY);
				break;
			case PlayerState.Attacking:
				if(Vector3.Dot(atkV, transform.position - targetPos) > 0){
					state = PlayerState.Cooling;
				}
				else{
					transform.position += atkV.normalized * atkSpeed * Time.deltaTime;
					deltaX = atkV.x;
				}
				break;
			case PlayerState.Cooling:
				if (!coolingDown)
					StartCoroutine(AtkCoolDown());
				break;
			case PlayerState.Hurt:
				break;
			case PlayerState.Dead:
				break;
		}
	}
	private IEnumerator AtkCoolDown() {
		coolingDown = true;
		yield return new WaitForSeconds(atkCoolDown);
		state = PlayerState.Idle;
		anim.SetBool("Idle", true);
		anim.SetFloat("deltaX", Norm(deltaX));
		coolingDown = false;
	}
	private float Norm(float x) {
		if (x == 0) return 1;
		return x / Mathf.Abs(x);
    }
	
	void Update(){
		if(GameManager.Instance.GameState != EGameState.Running)
			return;
		UpdateState();
	}
	
	void OnTriggerEnter2D(Collider2D other){
		Entity e = other.gameObject.GetComponent<Entity>();
		if(state == PlayerState.Attacking){
			e.TakeDmg(damage);
		}
		else{
			TakeDmg(e.Dmg);
		}
	}
}
public enum PlayerState{
	Spawning,
	Idle,
	Running,
	PrepareToAttack,
	Attacking,
	Cooling,
	Hurt,
	Dead
}
