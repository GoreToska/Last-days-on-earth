using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New AI Agent Configuration", menuName = "Epidemic/Data/AI/Agent Config")]
public class AIAgentConfig : ScriptableObject
{
    public float maxSightDistance = 18f;
}
