﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {
	public List<GameObject> btns;
	private float interval = .25f;
	private float timer;
	private bool flag = true;
	private List<Vector3> oriPos = new List<Vector3>();
	[SerializeField]
	private float scale = .02f;
	public bool debug = false;
    private void Awake() {
		foreach (GameObject go in btns) {
			oriPos.Add(go.transform.localPosition);
		}
		timer = interval;
	}
    public void OnClickStart(){
		SceneManager.LoadScene("MainScene");
	}
	public void OnClickUpgrade(){
		
	}
	public void OnClickQuit(){
		Application.Quit();
	}
	void Update() {
		if (debug) return;
		timer -= Time.deltaTime;
		if(timer <= 0) {
			for(int i = 0; i < btns.Count; i ++) {
                if (flag) {
					btns[i].transform.localPosition += GetRandVec();
                }
                else {
					btns[i].transform.localPosition = oriPos[i];
                }
            }
			flag = !flag;
			timer = interval;
        }
    }
	Vector3 GetRandVec() {
		float x = Random.Range(-scale, scale);
		float y = Random.Range(-scale, scale) * .5f;
		return new Vector3(x, y, 0f);
    }
}
