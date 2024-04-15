using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractablesPromptManager : MonoBehaviour
{
	[SerializeField] private GameObject _promptObject;
	[SerializeField] private TMP_Text _interactableName;
	[SerializeField] private Vector2 _offset;

	private Camera _camera;
	private Transform _interactableTransform;

	private void Awake()
	{
		_camera = Camera.main;
	}

	private void Start()
	{
		_promptObject.SetActive(false);
	}

	private void Update()
	{
		if (_promptObject.activeSelf)
		{
			UpdatePromptPosition();
		}
	}

	public void ShowPrompt(Transform transform, string name)
	{
		_interactableName.text = name;
		_interactableTransform = transform;
		UpdatePromptPosition();
		_promptObject.SetActive(true);
	}

	public void HidePrompt()
	{
		_promptObject.SetActive(false);
	}

	private void UpdatePromptPosition()
	{
		if (_interactableTransform == null)
			return;

		_promptObject.transform.position = _camera.WorldToScreenPoint(
			new Vector3(_interactableTransform.position.x + _offset.x,
			_interactableTransform.position.y + _offset.y,
			_interactableTransform.position.z));
	}
}
