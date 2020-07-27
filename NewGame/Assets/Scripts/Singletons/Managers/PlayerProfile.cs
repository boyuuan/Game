using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Something that goes with each individual user, such as player's current level and obtained items, etc.
//These data normally change through time because of upgrades.
public class PlayerProfile : Singleton<PlayerProfile>
{
    private int playerMaxHP;
    public int PlayerMaxHP {
        get {
            return playerMaxHP;
        }
    }
    private void Awake() {
    }
    public void Init() {

        playerMaxHP = 5;
    }
}
