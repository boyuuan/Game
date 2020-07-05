using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private Transform player;
	void Awake(){
		//Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
		DontDestroyOnLoad(gameObject);
		Rules.Instance.Init();
	}
	public void EndGame(){
		print("Game Over");
	}
}
