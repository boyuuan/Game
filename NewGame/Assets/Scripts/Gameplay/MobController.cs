using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : Singleton<MobController>
{
	public List<GameObject> AllMobs = new List<GameObject>();
	private Dictionary<EType, float> timers = new Dictionary<EType, float>();
	private float spawnTimer;
	private Dictionary<EType, int> mobCount = new Dictionary<EType, int>();
	private Dictionary<EType, int> mobCountMax = new Dictionary<EType, int>();
	private bool mobSpawnStart = false;
	private GameObject mobs;
	public void Init(){
		mobSpawnStart = true;
		foreach(EType et in GameManager.Instance.Prefabs.Keys){
			if(et == EType.Player) continue;
			mobCountMax[et] = Rules.Instance.EntityData[et].MaxCount;
			timers[et] = Rules.Instance.EntityData[et].SpawnTimer;
			mobCount[et] = 0;
		}
		mobs = new GameObject("Mobs");
		spawnTimer = 2f;
	}
	private bool isLevelCleared(){
		foreach(EType et in mobCount.Keys){
			if(mobCount[et] < mobCountMax[et]) return false;
		}
		return true;
	}
	void Update(){
		UpdateMobs();
		if(!mobSpawnStart) return;
		if(isLevelCleared() && AllMobs.Count == 0){
			GameManager.Instance.WinGame();
		}
		foreach(KeyValuePair<EType, float> pair in timers){
			if(mobCount[pair.Key] >= mobCountMax[pair.Key])
				continue;
			if(spawnTimer >= pair.Value){
				SpawnMob(pair.Key);
				spawnTimer = 0f;
			}
			else{
				spawnTimer += Time.deltaTime;
			}
		}
	}
	private void SpawnMob(EType et){
		Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0));
		GameObject obj = (GameObject)Instantiate(GameManager.Instance.Prefabs[et], pos, Quaternion.identity, mobs.transform);
		obj.name = GameManager.Instance.Prefabs[et].name + AllMobs.Count.ToString();
		mobCount[et] ++;
		AllMobs.Add(obj);
	}
	public void StopMobSpawn(){
		mobSpawnStart = false;
	}
	void UpdateMobs(){
		Entity e;
		int i = 0;
		while(i < AllMobs.Count){
			e = AllMobs[i].GetComponent<Entity>();
			if(e.IsDead()){
				AllMobs.RemoveAt(i);
				Destroy(e.gameObject);
			}
			else i ++;
		}
	}
}

public enum MobType{
	Zombie,
	Archer
}
