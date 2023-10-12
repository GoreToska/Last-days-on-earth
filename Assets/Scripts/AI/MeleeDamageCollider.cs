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
        Debug.Log(damage);
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

        if (other.transform.root.gameObject.layer != LayerMask.NameToLayer(playerMaskName) &&
            other.transform.root.gameObject.layer != LayerMask.NameToLayer(raiderMaskName))
        {
            return;
        }

        charactersToDamage.Add(other.transform.root.gameObject);

        Debug.Log(damage);
        other.transform.root.gameObject.GetComponent<PlayerStatusManager>().TakeDamage(damage);
    }
}
