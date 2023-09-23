using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [HideInInspector] public static PlayerEquipmentManager Instance;

    [SerializeField] public List<Item> itemsToPickUp;

    //  Handle weapon change
    [SerializeField] private MainWeapon mainWeapon;

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
    }

    private void OnEnable()
    {

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
}
