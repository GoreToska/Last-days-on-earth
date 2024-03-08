using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionManager : MonoBehaviour
{
	[HideInInspector] public static DescriptionManager Instance;

	[SerializeField] private RectTransform _descriptionObject;
	[SerializeField] private TMP_Text _itemName;
	[SerializeField] private TMP_Text _description;

	private float _objectWidth;
	private float _objectHeight;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		HideDescriptionPanel();

		StoreDescriptionPanelSize();
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
		Vector2 newPosition = position;

		if (position.x + _objectWidth >= Screen.width)
		{
			Debug.Log("Width");
			newPosition.x -= _objectWidth;
			// object is too close to the right side of the screen
		}

		if (position.y - _objectHeight <= 0)
		{
			// object is too close to the bottom side of the screen
			newPosition.y += _objectHeight;
		}

		return newPosition;
	}

	private void StoreDescriptionPanelSize()
	{
		_objectWidth = _descriptionObject.rect.width;
		_objectHeight = _descriptionObject.rect.height;
	}
}
