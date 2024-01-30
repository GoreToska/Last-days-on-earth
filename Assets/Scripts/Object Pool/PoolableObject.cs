using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoreToska
{
    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] public ObjectPool Parent;

        public virtual void Awake()
        {

        }

        public virtual void OnDisable()
        {
            Parent.ReturnObjectToPool(this);
        }

        protected IEnumerator DisableOnEndCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            this.gameObject.SetActive(false);
        }
    }
}
