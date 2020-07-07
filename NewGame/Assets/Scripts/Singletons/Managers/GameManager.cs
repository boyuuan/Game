using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private GameObject player = null;
	[SerializeField]
	private GameObject zombie = null;
	public bool InputEnabled = false;
	[SerializeField]
	private GameObject resultPG = null;
	void Start(){
		Application.targetFrameRate = 120;
		StartCoroutine(DelayGameStart());
	}
	void Awake(){
		DontDestroyOnLoad(gameObject);
		Rules.Instance.Init();
		MobController.Instance.Init();
		resultPG = transform.Find("ResultPage").gameObject;
		resultPG.SetActive(false);
		InputEnabled = false;
	}
	private IEnumerator Temp(){
		yield return new WaitForSeconds(.5f);
		Instantiate(zombie, new Vector3(-4, 0, 0), Quaternion.identity);
	}
	public void StartGame(){
		
	}
	private IEnumerator DelayGameStart(){
		InputEnabled = false;
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Game Start";
		resultPG.SetActive(true);
		yield return new WaitForSeconds(1f);
		resultPG.SetActive(false);
		InputEnabled = true;
		var obj = (GameObject)Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
		obj.name = player.name;
		obj = (GameObject)Instantiate(zombie, new Vector3(-4, 0, 0), Quaternion.identity);
		obj.name = zombie.name;
	}
	public void LoseGame(){
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Lost All Hope";
		InputEnabled = false;
		resultPG.SetActive(true);
	}
	public void WinGame(){
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Victory";
		InputEnabled = false;
		resultPG.SetActive(true);
	}
}
