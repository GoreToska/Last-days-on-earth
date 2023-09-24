using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerEquipmentManager : MonoBehaviour
{
    [HideInInspector] public static PlayerEquipmentManager Instance;

    [SerializeField] private Transform MainWeaponSocket;

    public List<Item> itemsToPickUp;

    //  Handle weapon change
    [SerializeField] public MainWeapon mainWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        PlayerInputManager.Instance.PickUpEvent += TryToPickUp;
        PlayerInputManager.Instance.AttackEvent += TryToPerformAttack;
    }

    private void OnEnable()
    {
        //PlayerInputManager.Instance.PickUpEvent += TryToPickUp;
        //PlayerInputManager.Instance.AttackEvent += TryToPerformAttack;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.AttackEvent -= TryToPerformAttack;
        PlayerInputManager.Instance.PickUpEvent -= TryToPickUp;
    }

    private void TryToPerformAttack()
    {
        if (mainWeapon && PlayerInputManager.Instance.IsAiming)
        {
            mainWeapon.PerformAttack();
        }
    }

    public async void TryToPickUp()
    {
        if (itemsToPickUp.Count == 0)
        {
            Debug.Log("Nothing to pickup");
            return;
        }

        if (await itemsToPickUp[0].PickUpItem())
        {
            itemsToPickUp.RemoveAt(0);
            return;
        }
    }

    public void OnMainWeaponEquip(Item weapon, StoredItem item)
    {
        Debug.Log("Drop");

        if (mainWeapon != null)
        {
            //  just do nothing in future
            Debug.Log(weapon.ToString());
            DropCurrentMainWeapon(mainWeapon);
        }

        mainWeapon = Instantiate(weapon.itemPrefab, MainWeaponSocket).GetComponent<MainWeapon>();
        mainWeapon.storedItem = item;
        PlayerAnimationManager.Instance.SetWeaponAnimationPattern(weapon.weaponData.weaponType);
    }

    private void DropCurrentMainWeapon(MainWeapon item)
    {
        PlayerInventory.Instance.RemoveItemFromInventoryGrid(item.storedItem);
        Instantiate(item.itemPrefab, this.transform.position, Quaternion.Euler(0, 0, 90), null);
        PlayerAnimationManager.Instance.SetWeaponAnimationPattern(WeaponType.None);
        Destroy(item.gameObject);
    }

    public void DropStoredItem(StoredItem item)
    {
        if (mainWeapon != null && mainWeapon.storedItem == item)
        {
            DropCurrentMainWeapon(mainWeapon);
        }
        else
        {
            Debug.Log(item.Details.itemCharacteristics.itemPrefab);
            Instantiate(item.Details.itemCharacteristics.itemPrefab, this.transform.position, Quaternion.Euler(0, 0, 0), null);
            PlayerInventory.Instance.RemoveItemFromInventoryGrid(item);
        }

        PlayerInventory.ItemDetailsVisibility(Visibility.Hidden);
    }
}
