using UnityEngine;
using UnityEngine.UIElements;

//  Summary
//  Handle all of the logic for registering, unregistering and processing click events
//  Summary
public class ItemVisual : VisualElement
{
    private readonly ItemDefinition m_Item;

    private Vector2 m_OriginalPosition;
    private bool m_IsDragging;
    private (bool canPlace, Vector2 position) m_PlacementResults;

    public ItemVisual(ItemDefinition item)
    {
        m_Item = item;

        //  setting the properties of the root VisualElement (the parent in the reference above)
        name = $"{m_Item.friendlyName}";
        style.height = m_Item.slotDimension.Height *
            PlayerInventory.SlotDimension.Height;
        style.width = m_Item.slotDimension.Width *
            PlayerInventory.SlotDimension.Width;
        style.visibility = Visibility.Hidden;

        //   creating a new child VisualElement called Icon
        //   and setting the background image to the same one set in the ItemDefinition asset
        VisualElement icon = new VisualElement
        {
            style = { backgroundImage = m_Item.icon.texture }
        };

        Add(icon);

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

    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if (!m_IsDragging && mouseEvent.button == 1)
        {
            PlayerInventory.UpdateItemDetails(m_Item);
            return;
        }

        if (!m_IsDragging && mouseEvent.button == 0)
        {
            StartDrag();
            return;
        }
        m_IsDragging = false;

        if (m_PlacementResults.canPlace && mouseEvent.button == 0)
        {
            SetPosition(new Vector2(
                m_PlacementResults.position.x - parent.worldBound.position.x,
                m_PlacementResults.position.y - parent.worldBound.position.y));
            return;
        }

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

}