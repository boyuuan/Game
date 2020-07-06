using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private Transform player = null;
	public bool InputEnabled = true;
	void Awake(){
		Debug.Log("GameManager Awake");
		DontDestroyOnLoad(gameObject);
		Rules.Instance.Init();
		Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
	}
	public void EndGame(){
		print("Game Over");
	}
}
