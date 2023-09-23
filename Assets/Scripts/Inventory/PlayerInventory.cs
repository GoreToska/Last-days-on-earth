using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PlayerInventory : MonoBehaviour
{
    [HideInInspector] public static PlayerInventory Instance;

    private VisualElement m_Root;
    public VisualElement inventoryGrid;
    private static Label m_ItemDetailHeader;
    private static Label m_ItemDetailBody;
    private static Label m_ItemDetailPrice;
    private bool m_IsInventoryReady;
    public static Dimensions SlotDimension { get; private set; }

    public List<StoredItem> StoredItems = new List<StoredItem>();   // List of stored items
    public Dimensions InventoryDimensions;  // The number of columns and rows that the inventory has

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

    //  find all references and set any initial values needed
    private async void Configure()
    {
        //  Sets the references to key VisualElements
        m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        inventoryGrid = m_Root.Q<VisualElement>("Grid");
        VisualElement itemDetails = m_Root.Q<VisualElement>("ItemDetails");
        m_ItemDetailHeader = itemDetails.Q<Label>("FriendlyName");
        m_ItemDetailBody = itemDetails.Q<Label>("Body");
        m_ItemDetailPrice = itemDetails.Q<Label>("SellPrice");

        //  Pauses until the end of the frame
        await UniTask.WaitForEndOfFrame();

        //  Calculates the size of a single slot item. The calculated value was used by ItemVisual
        ConfigureSlotDimensions();

        //  Sets m_IsInventoryReady as true, which is a value used by the LOAD INVENTORY method
        m_IsInventoryReady = true;
    }

    //  Grabs the first child of m_InventoryGrid and sets SlotDimensions variable to the width/height of it
    private void ConfigureSlotDimensions()
    {
        VisualElement firstSlot = inventoryGrid.Children().First();

        SlotDimension = new Dimensions
        {
            Width = Mathf.RoundToInt(firstSlot.worldBound.width),
            Height = Mathf.RoundToInt(firstSlot.worldBound.height)
        };

        Debug.Log(SlotDimension.Width);
        Debug.Log(SlotDimension.Height);
    }

    private async Task<bool> GetPositionForItem(VisualElement newItem, StoredItem item)
    {
        for (int y = 0; y < InventoryDimensions.Height; y++)
        {
            for (int x = 0; x < InventoryDimensions.Width; x++)
            {
                //try position

                //check for overlap inventory size
                if (SlotDimension.Width * (x + item.Details.slotDimension.Width) >
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
        await UniTask.WaitUntil(() => m_IsInventoryReady);

        foreach (StoredItem loadedItem in StoredItems)
        {
            ItemVisual inventoryItemVisual = new ItemVisual(loadedItem.Details);

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

    public async Task<StoredItem> AddNewItem(ItemDefinition item)
    {
        ItemVisual inventoryItemVisual = new ItemVisual(item);
        StoredItem storedItem = new StoredItem(item);
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
        //m_InventoryGrid.hierarchy.Add(item);
    }

    public void RemoveItemFromInventoryGrid(StoredItem item)
    {
        inventoryGrid.Remove(item.RootVisual);
        StoredItems.Remove(item);
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
        if (!inventoryGrid.layout.Contains(new Vector2(draggedItem.localBound.xMax,
            draggedItem.localBound.yMax)))
        {
            m_Telegraph.style.visibility = Visibility.Hidden;
            return (canPlace: false, position: Vector2.zero);
        }

        VisualElement targetSlot = inventoryGrid.Children().Where(x =>
            x.layout.Overlaps(draggedItem.layout) && x != draggedItem).OrderBy(x =>
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

    public static void UpdateItemDetails(ItemDefinition item)
    {
        m_ItemDetailHeader.text = item.friendlyName;
        m_ItemDetailBody.text = item.description;
        m_ItemDetailPrice.text = item.sellPrice.ToString();
    }

    public void TelegraphVisibility(Visibility visibility)
    {
        m_Telegraph.style.visibility = visibility;
    }

    public void InventoryVisibility(Visibility visibility)
    {
        m_Root.style.visibility = visibility;
        //m_Root.style.opacity = 0.5f; <- it works
    }

    public (StyleLength, StyleLength) PositionToPlace()
    {
        return (m_Telegraph.style.left, m_Telegraph.style.top);
        //return new Vector2(m_Telegraph.resolvedStyle.left, m_Telegraph.resolvedStyle.top);
    }
}