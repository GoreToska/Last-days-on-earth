using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[RequireComponent(typeof(UniqueID))]
public abstract class InventoryHolder : MonoBehaviour
{
	[field: SerializeField] public int InventorySize { get; private set; }
	[field: SerializeField] public InventorySystem PrimaryInventorySystem { get; set; }
	[SerializeField] public List<ItemPair> PreparedItems;
	[field: SerializeField] public int Offset { get; protected set; }

	public static UnityAction<InventorySystem, int> OnDinamicInventoryDisplayRequested;// inventory system to display, amount to offset display by

	protected string id;

	protected virtual void Awake()
	{
		PrimaryInventorySystem = new InventorySystem(InventorySize, PreparedItems);
		SaveLoad.OnLoadGame += LoadInventory;
		id = GetComponent<UniqueID>().ID;
	}

	protected abstract void LoadInventory(SaveData data);
}

// for save system
[System.Serializable]
public struct InventorySaveData
{
	public InventorySystem InventorySystem;
	public Vector3 Position;
	public Quaternion Rotation;

	public InventorySaveData(InventorySystem invSystem, Vector3 position, Quaternion rotation)
	{
		InventorySystem = invSystem;
		Position = position;
		Rotation = rotation;
	}

	public InventorySaveData(InventorySystem invSystem)
	{
		InventorySystem = invSystem;
		Position = Vector3.zero;
		Rotation = Quaternion.identity;
	}
}
