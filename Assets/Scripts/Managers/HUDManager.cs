using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class HUDManager : MonoBehaviour
{
    [HideInInspector] public static HUDManager Instance;

	[Header("Status Bars")]
	[SerializeField] private StatusBar _hpBar;
	[SerializeField] private StatusBar _staminaBar;

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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateHP(float newValue)
    {
		_hpBar.SetStatus(newValue);
	}

	public void UpdateStamina(float newValue)
    {
		_staminaBar.SetStatus(newValue);
	}

	public void UpdateBulletsStatus(int currentBullets)
    {
        var bulletString = currentBullets.ToString();
       // bulletsStatusLabel.text = bulletString;
    }

    public void BulletsStatusVisibility(Visibility visibility)
    {
       // bulletsStatusContainer.style.visibility = visibility;
    }
}
