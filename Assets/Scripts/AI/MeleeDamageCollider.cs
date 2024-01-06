using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeDamageCollider : MonoBehaviour
{
    [SerializeField] private string playerMaskName = "Player";
    [SerializeField] private string raiderMaskName = "Raider";

    private float damage;

    private List<GameObject> charactersToDamage = new List<GameObject>();

    private Collider damageCollider;
    void Start()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.enabled = false;
    }

    public void EnableCollider(float damage)
    {
        this.damage = damage;
        damageCollider.enabled = true;
    }

    public void DisableCollider()
    {
        damageCollider.enabled = false;
        charactersToDamage.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (charactersToDamage.Contains(other.transform.root.gameObject))
        {
            return;
        }

        if (other.transform.root.gameObject.layer == LayerMask.NameToLayer(playerMaskName))
        {
            AddCharacterToDamageList(other.transform.root.gameObject);
            other.transform.root.gameObject.GetComponent<PlayerStatusManager>().TakeDamage(damage);

            return;
        }
        else if(other.transform.root.gameObject.layer != LayerMask.NameToLayer(raiderMaskName))
        {
            AddCharacterToDamageList(other.transform.root.gameObject);
            // apply damage to raider

            return;
        }
    }

    private void AddCharacterToDamageList(GameObject character)
    {
        charactersToDamage.Add(character);
        // play sfx, impact, effect etc
    }
}
