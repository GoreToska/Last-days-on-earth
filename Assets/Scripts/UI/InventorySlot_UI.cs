using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemCount;
    [field: SerializeField] public InventorySlot AssignedInventorySlot { get; private set; }
    public InventoryDisplay ParentDisplay { get; private set; }

    private Button _button;

    private void Awake()
    {
        ClearSlot();

        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        AssignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            _itemImage.sprite = slot.ItemData.Icon;
            _itemImage.color = Color.white;

            if (slot.StackSize > 1)
            {
                _itemCount.text = slot.StackSize.ToString();
            }
            else
            {
                _itemCount.text = "";
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (AssignedInventorySlot != null)
        {
            UpdateUISlot(AssignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot?.ClearSlot();
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
