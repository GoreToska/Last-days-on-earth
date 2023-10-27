using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatusManager : MonoBehaviour
{
    public abstract bool IsDead { get; set; }

    public abstract void Start();

    public abstract void OnDisable();

    public abstract void SetHP(float value);

    public abstract void SetStamina(float value);

    public abstract void RegenHP(float value);

    public abstract void RegenStamina(float value);

    public abstract void TakeDamage(float value);

    public abstract void TakeStaminaDamage(float value);
}
