using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	protected override void Die(){
		GameManager.Instance.EndGame();
		print("Game Over");
	}
	public override void TakeDamage(int dmg){
		hp -= dmg;
		if(hp <= 0) Die();
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.A)){
			TakeDamage(1);
		}
	}
}
