using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo: Read from config.csv
public class Rules : Singleton<Rules>
{
	public Dictionary<EType, EntityData> EntityData = new Dictionary<EType, EntityData>();
	
	public void Init(){
		EntityData z = new EntityData(EType.Zombie);
		z.MaxHP = 1;
		z.MoveSpeed = 2f;
		z.AtkSpeed = 15f;
		z.Dmg = 1;
		z.Range = 2f;
		z.SpawnTimer = 3f;
		z.AtkDistance = 2.2f;
		z.MaxCount = 10;
		z.AtkCoolDown = .5f;
		
		EntityData[EType.Zombie] = z;
		
		EntityData p = new EntityData(EType.Player);
		p.MaxHP = 5;
		p.MoveSpeed = 5f;
		p.AtkSpeed = 20f;
		p.Dmg = 1;
		p.AtkDistance = 3f;
		
		EntityData[EType.Player] = p;
		
		EntityData a = new EntityData(EType.Archer);
		a.MaxHP = 2;
		a.Dmg = 1;
		a.Range = 5f;
		a.AtkSpeed = 5f;
		a.AtkCoolDown = .5f;
		
		EntityData[EType.Archer] = a;
	}
}
