using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Image statusBar;

    public void SetStatus(float value)
    {
        statusBar.fillAmount = value / 100f;
    }
}
