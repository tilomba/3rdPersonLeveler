using UnityEngine;
using System.Collections;
using System;

public class NetworkService : MonoBehaviour {
	private const string apiKey = "7c5b5d3abc11a30e3799f7ef112b66a2";
	private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Prague,CZ&mode=xml&APPID=" + apiKey;
	private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Prague,CZ&APPID=" + apiKey;


	private bool IsResponseValid(WWW www)
	{
		if (www.error != null)
		{
			Debug.Log ("bad connection - error: " + www.error);
			return false;
		}
		else if (string.IsNullOrEmpty (www.text))
		{
			Debug.Log ("bad data");
			return false;
		}
		else
		{
			return true;
		}
	}

	private IEnumerator CallAPI(string url, Action<string> callback)
	{
		WWW www = new WWW (url);
		yield return www;

		if(!IsResponseValid(www))
		{
			yield break;
		}

		callback (www.text);
	}

	public IEnumerator GetWeatherXML(Action<string> callback)
	{
		return CallAPI (xmlApi, callback);
	}

	public IEnumerator GetWeatherJSON(Action<string> callback)
	{
		return CallAPI (jsonApi, callback);
	}
}
