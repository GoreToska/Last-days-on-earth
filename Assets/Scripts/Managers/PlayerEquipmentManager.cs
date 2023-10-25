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
    [SerializeField] public RangeWeapon rangeMainWeapon;
    [SerializeField] public MeleeWeapon meleeWeapon;

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

        PlayerInputManager.Instance.AttackEvent += TryToPerformAttack;
        PlayerInputManager.Instance.PickUpEvent += TryToPickUp;
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
        if (rangeMainWeapon && PlayerInputManager.Instance.IsAiming)
        {
            rangeMainWeapon.PerformAttack();
            return;
        }

        if(meleeWeapon && PlayerInputManager.Instance.IsAiming)
        {
            meleeWeapon.PerformAttack();
            return;
        }
    }

    private void TryToPerformReload()
    {
        if (rangeMainWeapon)
        {
            rangeMainWeapon.PerformReload();
        }
    }

    public async void TryToPickUp()
    {
        if (itemsToPickUp.Count < 1)
        {
            Debug.Log("Nothing to pickup");
            return;
        }

        if (itemsToPickUp[0].PickUpItem())
        {
            itemsToPickUp.RemoveAt(0);
            return;
        }
    }

    public void OnMainWeaponEquip(WeaponItem weapon, StoredItem storedItem)
    {
        if (rangeMainWeapon != null)
        {
            //  just do nothing in future
            DropCurrentMainWeapon(rangeMainWeapon, false);
        }
        if(meleeWeapon != null)
        {
            Debug.Log("Drop melee");
            DropCurrentMainWeapon(meleeWeapon, false);
        }

        if(weapon.data.weaponType == WeaponType.Range_Primary)
        {
            rangeMainWeapon = Instantiate(weapon.data.weaponModel, MainWeaponSocket).GetComponent<RangeWeapon>();
            rangeMainWeapon.storedItem = storedItem;
            rangeMainWeapon.SetBulletStatus();
        }
        if(weapon.data.weaponType == WeaponType.Melee_Primary)
        {
            meleeWeapon = Instantiate(weapon.data.weaponModel, MainWeaponSocket).GetComponent<MeleeWeapon>();
            meleeWeapon.storedItem = storedItem;
            //rangeMainWeapon.SetBulletStatus();
        }

        PlayerAnimationManager.Instance.SetWeaponAnimationPattern(weapon.data.weaponType);
    }

    private void DropCurrentMainWeapon(RangeWeapon item, bool setDefaultRig = true)
    {
        PlayerInventory.Instance.RemoveItemFromInventoryGrid(item.storedItem);
        Instantiate(item.itemPrefab, this.transform.position, Quaternion.Euler(0, 0, 90), null);
        Destroy(item.gameObject);

        if (setDefaultRig)
        {
            PlayerAnimationManager.Instance.SetWeaponAnimationPattern(WeaponType.None);
        }
    }

    private void DropCurrentMainWeapon(MeleeWeapon item, bool setDefaultRig = true)
    {
        PlayerInventory.Instance.RemoveItemFromInventoryGrid(item.storedItem);
        Instantiate(item.itemPrefab, this.transform.position, Quaternion.Euler(0, 0, 90), null);
        Destroy(item.gameObject);

        if (setDefaultRig)
        {
            PlayerAnimationManager.Instance.SetWeaponAnimationPattern(WeaponType.None);
        }
    }

    public void DropStoredItem(StoredItem item)
    {
        if (rangeMainWeapon != null && rangeMainWeapon.storedItem == item)
        {
            DropCurrentMainWeapon(rangeMainWeapon);
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
