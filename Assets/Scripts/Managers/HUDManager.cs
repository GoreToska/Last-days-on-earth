using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    [HideInInspector] public static HUDManager Instance;

    private VisualElement root;
    private static VisualElement bulletsStatusContainer;
    private static Label bulletsStatusLabel;
    private static ProgressBar hpBar;
    private static ProgressBar staminaBar;

    private int currentBullets = 0;

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

        Configure();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Configure()
    {
        root = GetComponentInChildren<UIDocument>().rootVisualElement;
        bulletsStatusContainer = root.Q<VisualElement>("Ammo_Info_Container");
        bulletsStatusLabel = bulletsStatusContainer.Q<Label>("Bullets_Status");
        hpBar = root.Q<VisualElement>("Status_Container").Q<ProgressBar>("Hp_bar");
        staminaBar = root.Q<VisualElement>("Status_Container").Q<ProgressBar>("Stamina_bar");
    }

    //public void UpdateBulletsStatus(int currentBullets, int remainingBullets)
    //{
    //    var bulletString = currentBullets.ToString() + "/" + remainingBullets.ToString();
    //    bulletsStatusLabel.text = bulletString;
    //    this.currentBullets = currentBullets;
    //}

    public void UpdateHP(float newValue)
    {
        hpBar.value = newValue;
    }

    public void UpdateStamina(float newValue)
    {
        staminaBar.value = newValue;
    }

    public void UpdateBulletsStatus(int currentBullets)
    {
        var bulletString = currentBullets.ToString();
        bulletsStatusLabel.text = bulletString;
    }

    public void BulletsStatusVisibility(Visibility visibility)
    {
        bulletsStatusContainer.style.visibility = visibility;
    }
}
