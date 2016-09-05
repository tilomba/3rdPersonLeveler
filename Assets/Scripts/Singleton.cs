using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

	private static Singleton _instance = null;
	public static Singleton Instance
	{
		get { return _instance; }
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy (gameObject);
			return;
		}
		else
		{
			// just move it to the root
			this.transform.parent = null;	
			_instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
}
