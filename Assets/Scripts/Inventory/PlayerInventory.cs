using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PlayerInventory : MonoBehaviour
{
    [HideInInspector] public static PlayerInventory Instance;

    private VisualElement root;
    private VisualElement inventoryRoot;
    private VisualElement inventoryGrid;
    private VisualElement equiped;
    private static Button buttonDrop;
    private static VisualElement details;
    private static VisualElement mainWeaponSlot;
    private static Label itemDetailHeader;
    private static Label itemDetailDescription;
    private static Label itemDetailPrice;
    private bool isInventoryReady;
    private StoredItem currentItem;

    //  Move to its own scriptable object
    [SerializeField] public int LightRifleAmmoCount = 0;
    [SerializeField] public int HeavyRifleAmmoCount = 0;
    [SerializeField] public int PistolAmmoCount = 0;
    [SerializeField] public int SniperAmmoCount = 0;
    [SerializeField] public int ShotgunAmmoCount = 0;

    public static ItemData.ItemCharacteristic SlotDimension { get; private set; }

    public List<StoredItem> StoredItems = new List<StoredItem>();   // List of stored items
    public ItemData.ItemCharacteristic InventoryDimensions;  // The number of columns and rows that the inventory has

    private VisualElement m_Telegraph; // Used for highlighting slots in wich you are dragging in

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Configure();
            ConfigureInventoryTelegraph();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        LoadInventory();
    }

    private void OnEnable()
    {
        buttonDrop.clicked += () => PlayerEquipmentManager.Instance.DropStoredItem(currentItem);
    }

    private void OnDisable()
    {
        buttonDrop.clicked -= () => PlayerEquipmentManager.Instance.DropStoredItem(currentItem);
    }

    //  find all references and set any initial values needed
    private async void Configure()
    {
        //  Sets the references to key VisualElements
        root = GetComponentInChildren<UIDocument>().rootVisualElement;
        inventoryGrid = root.Q<VisualElement>("Grid");
        inventoryRoot = root.Q<VisualElement>("Inventory");
        equiped = root.Q<VisualElement>("Equiped");
        VisualElement itemDetails = root.Q<VisualElement>("ItemDetails");
        details = itemDetails;
        itemDetailHeader = itemDetails.Q<Label>("FriendlyName");
        itemDetailDescription = itemDetails.Q<Label>("Body");
        itemDetailPrice = itemDetails.Q<Label>("SellPrice");
        buttonDrop = itemDetails.Q<Button>("btn_Drop");
        mainWeaponSlot = root.Q<VisualElement>("MainWeaponSlot");

        //  Pauses until the end of the frame
        await UniTask.WaitForEndOfFrame();

        //  Calculates the size of a single slot item. The calculated value was used by ItemVisual
        ConfigureSlotDimensions();
        ItemDetailsVisibility(Visibility.Hidden);
        //  Sets m_IsInventoryReady as true, which is a value used by the LOAD INVENTORY method
        isInventoryReady = true;
    }

    //  Grabs the first child of m_InventoryGrid and sets SlotDimensions variable to the width/height of it
    private void ConfigureSlotDimensions()
    {
        VisualElement firstSlot = inventoryGrid.Children().First();

        SlotDimension = new ItemData.ItemCharacteristic
        {
            Width = Mathf.RoundToInt(firstSlot.worldBound.width),
            Height = Mathf.RoundToInt(firstSlot.worldBound.height)
        };
    }

    private async Task<bool> GetPositionForItem(VisualElement newItem, StoredItem item)
    {
        for (int y = 0; y < InventoryDimensions.Height; y++)
        {
            for (int x = 0; x < InventoryDimensions.Width; x++)
            {
                //try position

                //check for overlap inventory size
                if (SlotDimension.Width * (x + item.Data.itemCharacteristics.Width) >
                    SlotDimension.Width * InventoryDimensions.Width)
                {
                    continue;
                }

                SetItemPosition(newItem, new Vector2(SlotDimension.Width * x, SlotDimension.Height * y));

                //new Vector2(SlotDimension.Width * (x + item.Details.slotDimension.Width), 
                //  SlotDimension.Height * (y + item.Details.slotDimension.Height)));

                await UniTask.WaitForEndOfFrame();

                StoredItem overlappingItem = StoredItems.FirstOrDefault
                    (s => s.RootVisual != null &&
                    s.RootVisual.layout.Overlaps(newItem.layout));  // Check for overlap

                //Nothing is here! Place the item.
                if (overlappingItem == null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async void LoadInventory()
    {
        await UniTask.WaitUntil(() => isInventoryReady);

        foreach (StoredItem loadedItem in StoredItems)
        {
            ItemVisual inventoryItemVisual = new ItemVisual(loadedItem);

            AddItemToInventoryGrid(inventoryItemVisual);

            //  Waits until m_IsInventory is true before proceeding
            bool inventoryHasSpace = await GetPositionForItem(inventoryItemVisual, loadedItem);

            if (!inventoryHasSpace)
            {
                Debug.Log("No space - Cannot pick up the item");
                RemoveItemFromInventoryGrid(loadedItem);
                continue;
            }

            //  Creates a new VisualElement of type ItemVisual and adds it as a child of InventoryGrid
            ConfigureInventoryItem(loadedItem, inventoryItemVisual);
        }
    }

    public async Task<StoredItem> AddNewAmmoItem(AmmoData item)
    {
        switch (item.ammoType)
        {
            case AmmoTypes.RifleLight:
                //validation on how many ammo you already have
                if (LightRifleAmmoCount != 0)
                {
                    LightRifleAmmoCount += item.count;
                    return FindStoredAmmo(item);
                }
                else
                {
                    var stored = await AddNewItem(item, ItemType.Ammo, AmmoTypes.RifleLight);
                    LightRifleAmmoCount += item.count;
                    stored.RootVisual.SetCount(LightRifleAmmoCount);
                    return stored;
                }
            case AmmoTypes.RifleHeavy:
                if (HeavyRifleAmmoCount != 0)
                {
                    HeavyRifleAmmoCount += item.count;
                    return FindStoredAmmo(item);
                }
                else
                {
                    var stored = await AddNewItem(item, ItemType.Ammo, AmmoTypes.RifleHeavy);
                    HeavyRifleAmmoCount += item.count;
                    stored.RootVisual.SetCount(HeavyRifleAmmoCount);
                    return stored;
                }
            case AmmoTypes.Pistol:
                return null;
            case AmmoTypes.Sniper:
                return null;
            case AmmoTypes.Shotgun:
                return null;
            case AmmoTypes.None:
                return null;
            default:
                return null;
        }
    }

    private StoredItem FindStoredAmmo(AmmoData item)
    {
        foreach (var i in StoredItems)
        {
            if (i.Data.ID == item.ID)
            {
                if (item.ammoType == AmmoTypes.RifleHeavy)
                {
                    i.Count = HeavyRifleAmmoCount;
                    i.RootVisual.SetCount(HeavyRifleAmmoCount);
                }

                if (item.ammoType == AmmoTypes.RifleLight)
                {
                    i.Count = LightRifleAmmoCount;
                    i.RootVisual.SetCount(LightRifleAmmoCount);
                }

                if (item.ammoType == AmmoTypes.Pistol)
                {
                    i.Count = PistolAmmoCount;
                    i.RootVisual.SetCount(PistolAmmoCount);
                }

                if (item.ammoType == AmmoTypes.Sniper)
                {
                    i.Count = SniperAmmoCount;
                    i.RootVisual.SetCount(SniperAmmoCount);
                }

                if (item.ammoType == AmmoTypes.Shotgun)
                {
                    i.Count = ShotgunAmmoCount;
                    i.RootVisual.SetCount(ShotgunAmmoCount);
                }

                return i;
            }
        }
        return null;
    }

    public async Task<StoredItem> AddNewWeaponItem(WeaponItem weapon)
    {
        var item = await AddNewItem(weapon.data, ItemType.Weapon, AmmoTypes.None);

        if (item != null)
        {
            //PlayerEquipmentManager.Instance.OnMainWeaponEquip(weapon, item);
            return item;
        }
        else
        {
            return null;
        }
    }

    public async Task<StoredItem> AddNewItem(ItemData item, ItemType itemType, AmmoTypes ammoType)
    {
        StoredItem storedItem = new StoredItem(item, itemType, ammoType);
        ItemVisual inventoryItemVisual = new ItemVisual(storedItem);
        AddItemToInventoryGrid(inventoryItemVisual);

        //  Waits until m_IsInventory is true before proceeding
        bool inventoryHasSpace = await GetPositionForItem(inventoryItemVisual, storedItem);

        if (!inventoryHasSpace)
        {
            Debug.Log("No space - Cannot pick up the item");
            RemoveItemFromInventoryGrid(storedItem);
            return null;
        }

        StoredItems.Add(storedItem);

        //  Creates a new VisualElement of type ItemVisual and adds it as a child of InventoryGrid
        ConfigureInventoryItem(storedItem, inventoryItemVisual);
        return storedItem;
    }

    private void AddItemToInventoryGrid(VisualElement item)
    {
        inventoryGrid.Add(item);
    }

    public void RemoveItemFromInventoryGrid(StoredItem item)
    {
        if (item.ammoType == AmmoTypes.RifleHeavy)
        {
            HeavyRifleAmmoCount = 0;
        }
        if (item.ammoType == AmmoTypes.RifleLight)
        {
            LightRifleAmmoCount = 0;
        }
        if (item.ammoType == AmmoTypes.Sniper)
        {
            SniperAmmoCount = 0;
        }
        if (item.ammoType == AmmoTypes.Pistol)
        {
            PistolAmmoCount = 0;
        }
        if (item.ammoType == AmmoTypes.Shotgun)
        {
            ShotgunAmmoCount = 0;
        }

        inventoryGrid.Remove(item.RootVisual);
        StoredItems.Remove(item);
    }

    public int SubtractHeavyRifleAmmo(int value)
    {
        HeavyRifleAmmoCount -= value;

        foreach (var item in StoredItems)
        {
            if (item.ammoType == AmmoTypes.RifleHeavy)
            {
                if (HeavyRifleAmmoCount == 0)
                {
                    RemoveItemFromInventoryGrid(item);
                    return HeavyRifleAmmoCount;
                }

                item.RootVisual.SetCount(HeavyRifleAmmoCount);
            }
        }

        return HeavyRifleAmmoCount;
    }

    public int SubtractLightRifleAmmo(int value)
    {
        LightRifleAmmoCount -= value;

        foreach (var item in StoredItems)
        {
            if (item.ammoType == AmmoTypes.RifleLight)
            {
                if (LightRifleAmmoCount == 0)
                {
                    RemoveItemFromInventoryGrid(item);
                    return LightRifleAmmoCount;
                }

                item.RootVisual.SetCount(LightRifleAmmoCount);
            }
        }

        return LightRifleAmmoCount;
    }

    public int SubtractPistolAmmo(int value)
    {
        PistolAmmoCount -= value;

        foreach (var item in StoredItems)
        {
            if (item.ammoType == AmmoTypes.Pistol)
            {
                if (PistolAmmoCount == 0)
                {
                    RemoveItemFromInventoryGrid(item);
                    return PistolAmmoCount;
                }

                item.RootVisual.SetCount(PistolAmmoCount);
            }
        }

        return PistolAmmoCount;
    }

    public int SubtractSniperAmmo(int value)
    {
        SniperAmmoCount -= value;

        foreach (var item in StoredItems)
        {
            if (item.ammoType == AmmoTypes.Sniper)
            {
                if (SniperAmmoCount == 0)
                {
                    RemoveItemFromInventoryGrid(item);
                    return SniperAmmoCount;
                }

                item.RootVisual.SetCount(SniperAmmoCount);
            }
        }

        return SniperAmmoCount;
    }

    public int SubtractShotgunAmmo(int value)
    {
        ShotgunAmmoCount -= value;

        foreach (var item in StoredItems)
        {
            if (item.ammoType == AmmoTypes.Shotgun)
            {
                if (ShotgunAmmoCount == 0)
                {
                    RemoveItemFromInventoryGrid(item);
                    return ShotgunAmmoCount;
                }

                item.RootVisual.SetCount(ShotgunAmmoCount);
            }
        }

        return ShotgunAmmoCount;
    }

    private static void ConfigureInventoryItem(StoredItem item, ItemVisual visual)
    {
        item.RootVisual = visual;
        //visual.style.visibility = Visibility.Hidden;
    }

    private static void SetItemPosition(VisualElement element, Vector2 start)
    {
        element.style.left = start.x;
        element.style.top = start.y;
    }

    private void ConfigureInventoryTelegraph()
    {
        m_Telegraph = new VisualElement
        {
            name = "Telegraph",
            style =
        {
            position = Position.Absolute,
            visibility = Visibility.Hidden
        }
        };

        m_Telegraph.AddToClassList("slot-icon-highlighted");
        AddItemToInventoryGrid(m_Telegraph);
    }

    public (bool canPlace, Vector2 position) ShowPlacementTarget(ItemVisual draggedItem)
    {
        if (!inventoryRoot.layout.Contains(new Vector2(draggedItem.localBound.xMax,
            draggedItem.localBound.yMax)))
        {
            m_Telegraph.style.visibility = Visibility.Hidden;
            return (canPlace: false, position: Vector2.zero);
        }

        var together = inventoryGrid.Children().Concat(equiped.Children());
        VisualElement targetSlot = together.Where(x =>
            //x.layout.Overlaps(draggedItem.layout) &&  // idk why is it working without this code
            x != draggedItem).OrderBy(x =>
            Vector2.Distance(x.worldBound.position,
            draggedItem.worldBound.position)).First();

        UpdateTelegraphSize(draggedItem);
        SetItemPosition(m_Telegraph, new Vector2(targetSlot.layout.position.x, targetSlot.layout.position.y));

        m_Telegraph.style.visibility = Visibility.Visible;
        var overlappingItems = StoredItems.Where(x => x.RootVisual != null &&
            x.RootVisual.layout.Overlaps(m_Telegraph.layout)).ToArray();

        if (overlappingItems.Length > 1)
        {
            m_Telegraph.style.visibility = Visibility.Hidden;

            return (canPlace: false, position: Vector2.zero);
        }

        return (canPlace: true, targetSlot.worldBound.position);
    }

    public void UpdateTelegraphSize(ItemVisual draggedItem)
    {
        if (draggedItem.isRotated)
        {
            m_Telegraph.style.width = draggedItem.style.height;
            m_Telegraph.style.height = draggedItem.style.width;
        }
        else
        {
            m_Telegraph.style.width = draggedItem.style.width;
            m_Telegraph.style.height = draggedItem.style.height;
        }

    }

    public void UpdateItemDetails(StoredItem item)
    {
        itemDetailHeader.text = item.Data.name;
        itemDetailDescription.text = item.Data.description;
        itemDetailPrice.text = item.Data.sellPrice.ToString();
        currentItem = item;
    }

    public static void ItemDetailsVisibility(Visibility visibility)
    {
        details.style.visibility = visibility;
    }

    public void TelegraphVisibility(Visibility visibility)
    {
        m_Telegraph.style.visibility = visibility;
    }

    public void InventoryVisibility(Visibility visibility)
    {
        root.style.visibility = visibility;

        if (visibility == Visibility.Hidden)
        {
            ItemDetailsVisibility(visibility);
        }
        //m_Root.style.opacity = 0.5f; <- it works
    }

    public (StyleLength, StyleLength) PositionToPlace()
    {
        return (m_Telegraph.style.left, m_Telegraph.style.top);
        //return new Vector2(m_Telegraph.resolvedStyle.left, m_Telegraph.resolvedStyle.top);
    }
}