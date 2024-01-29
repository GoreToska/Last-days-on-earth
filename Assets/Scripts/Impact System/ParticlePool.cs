using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    private GameObject Parent;
    private PoolableParticle Prefab;
    private int Size;
    private List<PoolableParticle> AvailableObjectsPool;
    private static Dictionary<PoolableParticle, ParticlePool> ObjectPools = new Dictionary<PoolableParticle, ParticlePool>();

    private ParticlePool(PoolableParticle Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjectsPool = new List<PoolableParticle>(Size);
    }

    public static ParticlePool CreateInstance(PoolableParticle Prefab, int Size)
    {
        ParticlePool pool = null;

        if (ObjectPools.ContainsKey(Prefab))
        {
            pool = ObjectPools[Prefab];
        }
        else
        {
            pool = new ParticlePool(Prefab, Size);

            pool.Parent = new GameObject(Prefab + " Pool");
            pool.CreateObjects();

            ObjectPools.Add(Prefab, pool);
        }

        return pool;
    }

    private void CreateObjects()
    {
        for (int i = 0; i < Size; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        PoolableParticle poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, Parent.transform);
        poolableObject.Parent = this;
        poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
    }

    public PoolableParticle GetObject(Vector3 Position, Quaternion Rotation)
    {
        if (AvailableObjectsPool.Count == 0) // auto expand pool size if out of objects
        {
            CreateObject();
        }

        PoolableParticle instance = AvailableObjectsPool[0];

        AvailableObjectsPool.RemoveAt(0);

        instance.transform.position = Position;
        instance.transform.rotation = Rotation;

        instance.gameObject.SetActive(true);

        return instance;
    }

    public PoolableParticle GetObject()
    {
        return GetObject(Vector3.zero, Quaternion.identity);
    }

    public void ReturnObjectToPool(PoolableParticle Object)
    {
        AvailableObjectsPool.Add(Object);
    }
}
