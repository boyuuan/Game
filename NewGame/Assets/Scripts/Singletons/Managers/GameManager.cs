using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject player = null;
	[SerializeField]
	private GameObject zombie = null;
	public bool InputEnabled = true;
	void Start(){
		Application.targetFrameRate = 120;
	}
	void Awake(){
		Debug.Log("GameManager Awake");
		DontDestroyOnLoad(gameObject);
		Rules.Instance.Init();
		MobController.Instance.Init();
		var obj = (GameObject)Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
		obj.name = player.name;
		obj = (GameObject)Instantiate(zombie, new Vector3(-4, 0, 0), Quaternion.identity);
		obj.name = zombie.name;
		//StartCoroutine(Temp());
	}
	private IEnumerator Temp(){
		yield return new WaitForSeconds(.5f);
		Instantiate(zombie, new Vector3(-4, 0, 0), Quaternion.identity);
	}
	public void EndGame(){
		print("Game Over");
	}
}
