using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Read from config.xml
public class Rules : Singleton<Rules>
{
	public Dictionary<EntityType, int> maxHP = new Dictionary<EntityType, int>();
	public float PlayerSpeed;
	public float PlayerAtkDistance;
	public void Init(){
		Debug.Log("Rules Init.");
		PlayerSpeed = 15f;
		PlayerAtkDistance = 2f;
		maxHP[EntityType.Player] = 5;
		maxHP[EntityType.Zombie] = 1;
		maxHP[EntityType.Archer] = 2;
	}
}
