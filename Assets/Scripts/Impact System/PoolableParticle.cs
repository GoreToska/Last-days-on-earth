using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableParticle : MonoBehaviour
{
    public ParticlePool Parent;

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}
