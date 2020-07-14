using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static bool _shuttingDown = false;
	private static object _lock = new object();
	private static T _instance;
	public static T Instance{
		get{
			lock(_lock){
				if(_instance == null){
					_instance = (T)FindObjectOfType(typeof(T));
					if(_instance == null){
						Debug.Log("Created " + typeof(T).ToString());
						var singletonObject = new GameObject();
						_instance = singletonObject.AddComponent<T>();
						singletonObject.name = typeof(T).ToString();
						DontDestroyOnLoad(singletonObject);
					}
				}
				return _instance;
			}
		}
	}
	void OnApplicationQuit(){
		_shuttingDown = true;
	}
	void OnDestroy(){
		_shuttingDown = true;
	}
}
