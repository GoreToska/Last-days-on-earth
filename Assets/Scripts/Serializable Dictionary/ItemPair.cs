using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPair : ISerializationCallbackReceiver
{
	[field: SerializeField] public InventoryItemData Item { get; private set; }
	[field: SerializeField] public int Count { get; private set; }

	public void OnAfterDeserialize()
	{

	}

	public void OnBeforeSerialize()
	{
		if (Item == null)
			return;

		if (Count > Item.MaxStackSize)
			Count = Item.MaxStackSize;

		if (Count <= 0)
			Count = 1;
	}
}
