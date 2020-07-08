using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : Singleton<GameManager>
{
	public bool InputEnabled = false;
	[SerializeField]
	private GameObject resultPG = null;
	[SerializeField]
	private List<GameObject> inspectorPrefabs;
	public Dictionary<EType, GameObject> Prefabs = new Dictionary<EType, GameObject>();
	void Start(){
		Application.targetFrameRate = 120;
	}
	void Awake(){
		DontDestroyOnLoad(gameObject);
		Rules.Instance.Init();
		resultPG = transform.Find("ResultPage").gameObject;
		resultPG.SetActive(false);
		InputEnabled = false;
		for(int i = 0; i < inspectorPrefabs.Count; i ++){
			Prefabs[EType.None + 1 + i] = inspectorPrefabs[i];
		}
		StartCoroutine(DelayGameStart());
	}
	public void StartGame(){
		
	}
	private IEnumerator DelayGameStart(){
		InputEnabled = false;
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Game Start";
		resultPG.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		resultPG.SetActive(false);
		InputEnabled = true;
		var obj = (GameObject)Instantiate(Prefabs[EType.Player], new Vector3(0, 0, 0), Quaternion.identity);
		obj.name = Prefabs[EType.Player].name;
		MobController.Instance.Init();
	}
	public void LoseGame(){
		MobController.Instance.StopMobSpawn();
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Lost All Hope";
		InputEnabled = false;
		resultPG.SetActive(true);
	}
	public void WinGame(){
		MobController.Instance.StopMobSpawn();
		resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Victory";
		InputEnabled = false;
		resultPG.SetActive(true);
	}
}
