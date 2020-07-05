using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	private float depth = 10.0f;
	protected override void Die(){
		GameManager.Instance.EndGame();
	}
	public override void TakeDamage(int dmg){
		hp -= dmg;
		if(hp <= 0) Die();
	}
	void Update(){
		if(Input.GetMouseButtonUp(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
				Debug.Log("dkslj");
			Debug.Log("MousePos = " + Input.mousePosition);
			var mousePos = Input.mousePosition;
			var wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, depth));
			transform.position = wantedPos;
		}
	}
}
