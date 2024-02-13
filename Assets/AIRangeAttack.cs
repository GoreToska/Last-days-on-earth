using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRangeAttack : MonoBehaviour, IAIRangeAttack
{
	public void PerformShot(BaseAIAgent agent, float damage = 0)
	{
		agent.AIAnimation.PlayMediumRifleShot();
	}
}

public interface IAIRangeAttack
{
	public void PerformShot(BaseAIAgent agent, float damage = 0);
}
