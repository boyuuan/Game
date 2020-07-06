using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : Singleton<MobController>
{
	public List<GameObject> mobs = new List<GameObject>();
	private Dictionary<EType, float> timers = new Dictionary<EType, float>();
	private float zombieTimer;
	public void Init(){
		timers[EType.Zombie] = Rules.Instance.EntityData[EType.Zombie].SpawnTimer;
	}
	void Update(){
		foreach(KeyValuePair<EType, float> pair in timers){
			
		}
	}
}

public enum MobType{
	Zombie,
	Archer
}
