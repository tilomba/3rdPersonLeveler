using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {
	[SerializeField] private string itemName;

	void OnTriggerEnter(Collider other)
	{
		Managers.Inventory.AddItem (name);
		Destroy (this.gameObject);
	}
}
