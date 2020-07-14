using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum EGameState{
	None = 0,
	Title,
	Initing,
	Running,
	Pausing,
	Stop
}

public class GameManager : Singleton<GameManager>
{
	public EGameState GameState;
	public bool InputEnabled = false;
	private GameObject go_resultPG = null;
	private GameObject go_fps = null;
	private Text text_fps = null;
	private float timer;
	[SerializeField]
	private List<GameObject> inspectorPrefabs = null;
	public Dictionary<EType, GameObject> Prefabs = new Dictionary<EType, GameObject>();
	private Player player = null;
	void Start(){
	}
	void Awake(){
		if(Instance != this){
			Destroy(gameObject);
		}
		else{
			DontDestroyOnLoad(gameObject);
			Application.targetFrameRate = 120;
			timer = 1f;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}
	void Update(){
		if(GameState == EGameState.Title) return;
		timer -= Time.deltaTime;
		if(timer <= 0f){
			text_fps.text = "FPS " + (1f / Time.deltaTime).ToString("F1");
			timer = 1f;
		}
	}
	public void StartGame(){
		
	}
	private IEnumerator DelayGameStart(){
		InputEnabled = false;
		go_resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Game Start";
		go_resultPG.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		go_resultPG.SetActive(false);
		InputEnabled = true;
		var obj = (GameObject)Instantiate(Prefabs[EType.Player], new Vector3(0, 0, 0), Quaternion.identity);
		obj.name = Prefabs[EType.Player].name;
		player = obj.GetComponent<Player>();
		MobController.Instance.Init();
		GameState = EGameState.Running;
	}
	public void LoseGame(){
		GameState = EGameState.Stop;
		//Time.timeScale = 0f;
		MobController.Instance.StopMobSpawn();
		go_resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Lost All Hope";
		InputEnabled = false;
		go_resultPG.SetActive(true);
		StartCoroutine(DelayBackToTitle());
	}
	public void WinGame(){
		GameState = EGameState.Stop;
		//Time.timeScale = 0f;
		MobController.Instance.StopMobSpawn();
		go_resultPG.transform.Find("Canvas/Text").GetComponent<Text>().text = "Victory";
		InputEnabled = false;
		go_resultPG.SetActive(true);
		StartCoroutine(DelayBackToTitle());
	}
	IEnumerator DelayBackToTitle(){
		yield return new WaitForSeconds(2f);
		Destroy(player.gameObject);
		SceneManager.LoadScene("TitleScene");
	}
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if(scene.name == "TitleScene"){
			GameState = EGameState.Title;
			Rules.Instance.Init();
			for(int i = 0; i < inspectorPrefabs.Count; i ++){
				Prefabs[EType.None + 1 + i] = inspectorPrefabs[i];
			}
		}
		else if(scene.name == "MainScene"){
			GameState = EGameState.Initing;
			go_resultPG = GameObject.Find("/ResultPage");
			go_fps = GameObject.Find("/FPS");
			text_fps = go_fps.transform.Find("Text").GetComponent<Text>();
			StartCoroutine(DelayGameStart());
		}
    }
}
