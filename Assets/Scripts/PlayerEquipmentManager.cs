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
        PlayerInputManager.Instance.ReloadEvent += TryToPerformReload;
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
        PlayerInputManager.Instance.ReloadEvent -= TryToPerformReload;
    }

    private void TryToPerformAttack()
    {
        if (mainWeapon && PlayerInputManager.Instance.IsAiming)
        {
            mainWeapon.PerformAttack();
        }
    }

    private void TryToPerformReload()
    {
        if(mainWeapon)
        {
            mainWeapon.PerformReload();
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

    public void OnMainWeaponEquip(WeaponItem weapon, StoredItem storedItem)
    {
        if (mainWeapon != null)
        {
            //  just do nothing in future
            Debug.Log(weapon.ToString());
            DropCurrentMainWeapon(mainWeapon);
        }

        mainWeapon = Instantiate(weapon.data.weaponModel, MainWeaponSocket).GetComponent<MainWeapon>();
        PlayerAnimationManager.Instance.SetWeaponAnimationPattern(weapon.data.weaponType);
        mainWeapon.storedItem = storedItem;
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
            var droppedItem = Instantiate(item.Data.itemCharacteristics.itemPrefab, this.transform.position, Quaternion.Euler(0, 0, 0), null);

            if (item.ammoType == AmmoTypes.RifleLight)
            {
            }

            PlayerInventory.Instance.RemoveItemFromInventoryGrid(item);
        }

        PlayerInventory.ItemDetailsVisibility(Visibility.Hidden);
    }
}
