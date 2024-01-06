using InventorySystem;
using UnityEngine;
namespace InventorySampleScene
{
    public class FashionApplicator : MonoBehaviour
    {
        [SerializeField] GameObject hatFlippedFalse;
        [SerializeField] GameObject hatFlippedTrue;

        [SerializeField] Sprite turqoiseHat;
        [SerializeField] Sprite boringHat;
        [SerializeField] Sprite redHat;
        [SerializeField] Sprite blackHat;

        [SerializeField] GameObject redHatobj;
        SpriteRenderer srHatFlippedFalse;
        SpriteRenderer srHatFlippedTrue;

        private void Start()
        {
            srHatFlippedFalse = hatFlippedFalse.GetComponent<SpriteRenderer>();
            srHatFlippedTrue = hatFlippedTrue.GetComponent<SpriteRenderer>();
        }
        public void SetTurqoiseHat()
        {
            srHatFlippedFalse.sprite = turqoiseHat;
            srHatFlippedTrue.sprite = turqoiseHat;
        }
        public void SetBoringHat()
        {
            srHatFlippedFalse.sprite = boringHat;
            srHatFlippedTrue.sprite = boringHat;
        }
        public void SetRedHat()
        {
            srHatFlippedFalse.sprite = redHat;
            srHatFlippedTrue.sprite = redHat;
        }
        public void SetBlackHat()
        {
            srHatFlippedFalse.sprite = blackHat;
            srHatFlippedTrue.sprite = blackHat;
        }
        public void swap(InventoryItem item1, InventoryItem inSlot)
        {
            string item1inv = item1.GetInventory();
            string inSLotInv = inSlot.GetInventory();

            int positem1 = item1.GetPosition();
            int posinslotinv = inSlot.GetPosition();
            InventoryController.instance.RemoveItemPos(inSLotInv, inSlot.GetPosition(), inSlot.GetAmount());

            InventoryController.instance.AddItemPos(item1inv, inSlot.GetItemType(), positem1, inSlot.GetAmount());

            InventoryController.instance.AddItemPos(inSLotInv, item1.GetItemType(), posinslotinv, item1.GetAmount());


        }
        public void DropItem(Vector3 pos, InventoryItem item)
        {
            for (int i = 0; i < item.GetAmount(); i++)
            {
                Instantiate(item.GetRelatedGameObject(), pos, Quaternion.identity);
            }
        }
    }
}
