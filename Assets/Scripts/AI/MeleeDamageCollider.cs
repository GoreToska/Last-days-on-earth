using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeDamageCollider : MonoBehaviour
{
    private float _damage;

    private List<GameObject> _charactersToDamage = new List<GameObject>();
    private Collider _damageCollider;

    void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.enabled = false;
        _damageCollider.isTrigger = true;
    }

    public void EnableCollider(float damage)
    {
        this._damage = damage;
        _damageCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _damageCollider.enabled = false;
        _charactersToDamage.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_charactersToDamage.Contains(other.transform.root.gameObject) || other.transform.root.gameObject.layer == this.transform.root.gameObject.layer)
        {
            return;
        }

        if (other.transform.root.gameObject.TryGetComponent<IDamagable>(out var component))
        {
            AddCharacterToDamageList(other.transform.root.gameObject);
            component.TakeDamage(_damage, this.gameObject);

            return;
        }
    }

    private void AddCharacterToDamageList(GameObject character)
    {
        _charactersToDamage.Add(character);
        ImpactManager.Instance.HandleImpact(character, character.transform.position + Vector3.up, Vector3.forward, ImpactType.Shot);
        // play sfx, impact, effect etc
    }
}
