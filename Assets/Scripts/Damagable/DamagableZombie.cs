using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseAIAgent))]
public class DamagableZombie : DamagableCharacter, IDamagable
{
	protected override void Awake()
    {
        base.Awake();
    }
}
