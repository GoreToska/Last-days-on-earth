using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventorySlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler
{
	[Inject] private DescriptionManager _descriptionManager;

	[SerializeField] private Image _itemImage;
	[SerializeField] private TMP_Text _itemCount;
	[SerializeField] private GameObject _slotHighlight;
	[field: SerializeField] public InventorySlot AssignedInventorySlot { get; private set; }
	public InventoryDisplay ParentDisplay { get; private set; }

	private Button _button;

	private void Awake()
	{
		ClearSlot();

		_itemImage.preserveAspect = true;

		_button = GetComponent<Button>();
		_button?.onClick.AddListener(OnUISlotClick);

		ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
	}

	public void Construct(DescriptionManager descriptionManager)
	{
		_descriptionManager = descriptionManager;
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

	public void ToggleHighlight()
	{
		_slotHighlight.SetActive(!_slotHighlight.activeInHierarchy);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (AssignedInventorySlot.ItemData == null)
			return;

		_descriptionManager.ConfigureDescriptionPanel(AssignedInventorySlot.ItemData.DisplayName, AssignedInventorySlot.ItemData.Description, eventData.position);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (AssignedInventorySlot.ItemData == null)
			return;

		_descriptionManager.HideDescriptionPanel();
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		if (AssignedInventorySlot.ItemData == null)
			return;

		_descriptionManager.MoveDescriptionPanel(eventData.position);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (AssignedInventorySlot.ItemData == null)
			return;

		_descriptionManager.HideDescriptionPanel();
	}
}
