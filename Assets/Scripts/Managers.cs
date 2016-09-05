using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour {
	public static PlayerManager Player { get; private set; }
	public static InventoryManager Inventory { get; private set; }
	public static MissionManager Mission { get; private set; }
	public static DataManager Data { get; private set; }

	private List<IGameManager> _startupSequence;

	void Awake()
	{
		DontDestroyOnLoad (gameObject);

		Data = GetComponent<DataManager> ();
		Player = GetComponent<PlayerManager> ();
		Inventory = GetComponent<InventoryManager> ();
		Mission = GetComponent<MissionManager> ();

		_startupSequence = new List<IGameManager> ();
		_startupSequence.Add (Player);
		_startupSequence.Add (Inventory);
		_startupSequence.Add (Mission);
		_startupSequence.Add (Data);

		StartCoroutine (StartupManagers ());
	}

	private IEnumerator StartupManagers()
	{
		NetworkService network = new NetworkService ();

		foreach (IGameManager manager in _startupSequence)
		{
			manager.Startup (network);
		}

		yield return null;

		int numModles = _startupSequence.Count;
		int numReady = 0;

		while (numReady < numModles)
		{
			int lastReady = numReady;
			numReady = 0;

			foreach (IGameManager manager in _startupSequence)
			{
				if (manager.status == ManagerStatus.Started)
				{
					numReady++;
				}
			}

			if (numReady > lastReady)
			{
				Debug.Log("Progress: " + numReady + "/" + numModles);
				Messenger<int, int>.Broadcast (StartupEvent.MANAGERS_PROGRESS, numReady, numModles);
				yield return null;
			}
		}

		Debug.Log ("All managers started up");
		Messenger.Broadcast (StartupEvent.MANAGERS_STARTED);
	}
}
