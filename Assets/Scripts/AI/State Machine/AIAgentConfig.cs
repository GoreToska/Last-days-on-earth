using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Agent Configuration", menuName = "Epidemic/Data/AI/Agent Config")]
public class AIAgentConfig : ScriptableObject
{
	public float MaxSightDistance = 18f;
	public float LightDamage = 10;
	public float HeavyDamage = 25;
	public float AttackDistance = 1.3f;
}
