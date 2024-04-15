using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemDescriptionManager : MonoBehaviour
{
	[SerializeField] private RectTransform _descriptionObject;
	[SerializeField] private TMP_Text _itemName;
	[SerializeField] private TMP_Text _description;
	[SerializeField] private Vector2 _offset;

	private void Awake()
	{
		HideDescriptionPanel();
	}

	public void ConfigureDescriptionPanel(string itemName, string description, Vector2 position)
	{
		_descriptionObject.transform.position = SetPosition(position);
		_itemName.text = itemName;
		_description.text = description;

		ShowDescriptionPanel();
	}

	public void MoveDescriptionPanel(Vector2 position)
	{
		_descriptionObject.transform.position = SetPosition(position);
	}

	public void HideDescriptionPanel()
	{
		_descriptionObject.gameObject.SetActive(false);
	}

	public void ShowDescriptionPanel()
	{
		_descriptionObject.gameObject.SetActive(true);
	}

	private Vector2 SetPosition(Vector2 position)
	{
		Vector2 newPosition = position + _offset * _descriptionObject.lossyScale;

		if (newPosition.x + _descriptionObject.rect.width * _descriptionObject.lossyScale.x >= Screen.width)
		{
			newPosition.x -= (_descriptionObject.rect.width + _offset.x) * _descriptionObject.lossyScale.x;
			// object is too close to the right side of the screen
		}

		if (newPosition.y - _descriptionObject.rect.height * _descriptionObject.lossyScale.y <= 0)
		{
			// object is too close to the bottom side of the screen
			newPosition.y += (_descriptionObject.rect.height - _offset.y) * _descriptionObject.lossyScale.y;
		}

		return newPosition;
	}
}
