using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
	private Slider statusBar;

	private void Awake()
	{
		statusBar = GetComponent<Slider>();
	}

	public void SetStatus(float value)
	{
		statusBar.value = value / 100f;
	}
}
