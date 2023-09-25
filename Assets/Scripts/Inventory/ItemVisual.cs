using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

//  Summary
//  Handle all of the logic for registering, unregistering and processing click events
//  Summary
public class ItemVisual : VisualElement
{
    private readonly ItemData item;
    private readonly StoredItem storedItem;

    private Vector2 m_OriginalPosition;
    private bool m_IsDragging;
    private (bool canPlace, Vector2 position) m_PlacementResults;
    private (StyleLength left, StyleLength top) m_Placement;
    public bool isRotated = false;
    public bool wasRotated = false;

    private Label count = new Label();

    public ItemVisual(StoredItem item)
    {
        this.item = item.Data;
        this.storedItem = item;

        //  setting the properties of the root VisualElement (the parent in the reference above)
        name = $"{this.item.name}";
        style.height = this.item.itemCharacteristics.Height *
            PlayerInventory.SlotDimension.Height;
        style.width = this.item.itemCharacteristics.Width *
            PlayerInventory.SlotDimension.Width;
        //style.visibility = Visibility.Visible;

        //   creating a new child VisualElement called Icon
        //   and setting the background image to the same one set in the ItemDefinition asset
        VisualElement icon = new VisualElement
        {
            style = { backgroundImage = this.item.image.texture }
        };

        Add(icon);

        if (item.Data.count > 0)
        {
            count.text = item.Count.ToString();
            icon.Add(count);
            count.AddToClassList("visual-icon-count");
        }


        //  adding two new styles to the parent and children.
        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");

        RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }

    //  Unregister events
    ~ItemVisual()
    {
        UnregisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        UnregisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }

    //  set position of item
    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.top = pos.y;
    }

    public void SetPosition(StyleLength left, StyleLength top)
    {
        style.left = left;
        style.top = top;
    }

    public void SetCount(int value)
    {
        count.text = value.ToString();
    }

    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if (!m_IsDragging && mouseEvent.button == 1)
        {
            PlayerInventory.Instance.UpdateItemDetails(storedItem);
            PlayerInventory.ItemDetailsVisibility(Visibility.Visible);
            return;
        }

        if (!m_IsDragging && mouseEvent.button == 0)
        {
            StartDrag();
            //PlayerInputManager.Instance.RotateItem += RotateItem;
            return;
        }
        m_IsDragging = false;

        if (m_PlacementResults.canPlace && mouseEvent.button == 0)
        {
            //SetRotation(isRotated);
            //PlayerInputManager.Instance.RotateItem -= RotateItem;

            m_Placement = PlayerInventory.Instance.PositionToPlace();
            SetPosition(m_Placement.left, m_Placement.top);

            //if (isRotated)
            //{
            //    SetPosition(new Vector2(targetSlot.layout.position.x, targetSlot.layout.position.y));

            //    //SetPosition(new Vector2(
            //    //m_PlacementResults.position.x - parent.worldBound.position.x - (1 - m_Item.slotDimension.Width) * 25,
            //    //m_PlacementResults.position.y - parent.worldBound.position.y + (1 - m_Item.slotDimension.Height) * 25)); // sell size = 100
            //}
            //else
            //{
            //    SetPosition(new Vector2(
            //    m_PlacementResults.position.x - parent.worldBound.position.x,
            //    m_PlacementResults.position.y - parent.worldBound.position.y));
            //}

            return;
        }

        //SetRotation(wasRotated);
        //PlayerInputManager.Instance.RotateItem -= RotateItem;
        SetPosition(new Vector2(m_OriginalPosition.x, m_OriginalPosition.y));
    }

    public void StartDrag()
    {
        m_IsDragging = true;
        m_OriginalPosition = worldBound.position - parent.worldBound.position;
        BringToFront();
    }

    private void OnMouseMoveEvent(MouseMoveEvent mouseEvent)
    {
        if (!m_IsDragging)
        {
            return;
        }

        SetPosition(GetMousePosition(mouseEvent.mousePosition));

        m_PlacementResults = PlayerInventory.Instance.ShowPlacementTarget(this);
    }

    public Vector2 GetMousePosition(Vector2 mousePosition)
    {
        return new Vector2(mousePosition.x - (layout.width / 2) -
         parent.worldBound.position.x, mousePosition.y - (layout.height / 2) -
         parent.worldBound.position.y);
    }

    //public void RotateItem()
    //{
    //    if (resolvedStyle.width != m_Item.slotDimension.Width * 100) // cell size
    //    {
    //        style.width = m_Item.slotDimension.Width * 100;
    //        style.height = m_Item.slotDimension.Height * 100;
    //        PlayerInventory.Instance.UpdateTelegraphSize(this);
    //    }
    //    else
    //    {
    //        style.width = m_Item.slotDimension.Height * 100;
    //        style.height = m_Item.slotDimension.Width * 100;
    //        PlayerInventory.Instance.UpdateTelegraphSize(this);
    //    }

    //    //if (style.rotate == new Rotate(90))
    //    //{
    //    //    isRotated = false;
    //    //    style.rotate = StyleKeyword.Initial;
    //    //    PlayerInventory.Instance.UpdateTelegraphSize(this);
    //    //}
    //    //else
    //    //{
    //    //    isRotated = true;
    //    //    style.rotate = new Rotate(90);
    //    //    PlayerInventory.Instance.UpdateTelegraphSize(this);
    //    //}
    //}

    private void SetRotation(bool wasRotated)
    {
        if (!wasRotated)
        {
            isRotated = false;
            style.rotate = StyleKeyword.Initial;
            PlayerInventory.Instance.UpdateTelegraphSize(this);
        }
        else
        {
            isRotated = true;
            style.rotate = new Rotate(90);
            PlayerInventory.Instance.UpdateTelegraphSize(this);
        }
    }
}