using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [field: SerializeField] public int InventorySize { get; private set; }
    [field: SerializeField] public InventorySystem InventorySystem { get; set; }

    public static UnityAction<InventorySystem> OnDinamicInventoryDisplayRequested;

    private void Awake()
    {
        InventorySystem = new InventorySystem(InventorySize);
    }
}
