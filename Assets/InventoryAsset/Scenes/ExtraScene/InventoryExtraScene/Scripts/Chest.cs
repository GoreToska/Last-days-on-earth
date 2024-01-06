using UnityEngine;
using InventorySystem;

namespace InventorySampleScene
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private RectTransform inventoryRectTransform; // Changed from GameObject to RectTransform
        [SerializeField] private float distance;
        [SerializeField] private Vector3 offset;
        [SerializeField] GameObject text;
        private Camera mainCamera;
        private Canvas canvas; // The canvas that the inventory is a child of

        private void Start()
        {
            text.SetActive(true);
            mainCamera = Camera.main;

            // Assuming the parent of the inventory is the canvas
            canvas = inventoryRectTransform.GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(transform.position);

            // Convert screen point to canvas space
            Vector2 canvasPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out canvasPoint);

            inventoryRectTransform.anchoredPosition = canvasPoint; // Use anchoredPosition for UI elements
            inventoryRectTransform.position = inventoryRectTransform.position + offset;

            if ((player.transform.position - transform.position).magnitude < distance)
            {
                InventoryController.instance.AddToggleKey("Chest", 'e');
                text.SetActive(true);

            }
            else
            {
                inventoryRectTransform.gameObject.SetActive(false);
                InventoryController.instance.RemoveToggleKey("Chest", 'e');
                text.SetActive(false);
            }
        }
    }

}
