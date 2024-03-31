using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
	[Header("Status Bars")]
	[SerializeField] private StatusBar _hpBar;
	[SerializeField] private StatusBar _staminaBar;

    private int currentBullets = 0;

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
