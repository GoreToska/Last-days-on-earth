using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMemory
{
    public float Age { get { return Time.time - lastSeen; } }
    public GameObject gameObject;
    public Vector3 position;
    public Vector3 direction;
    public float distance;
    public float angle;
    public float lastSeen;
    public float scrore;
}

public class AISensoryMemory
{
    public List<AIMemory> Memories = new List<AIMemory>();
    private GameObject[] _characters;

    public AISensoryMemory(int maxPlayers)
    {
        _characters = new GameObject[maxPlayers];
    }

    public void UpdateSenses(AISensor sensor)
    {
        //int targets = sensor.Filter(_characters, sensor.LayerNames[0], sensor.LayerNames[1]);
        int targets = sensor.Filter(_characters);

        for (int i = 0; i < targets; ++i)
        {
            GameObject target = _characters[i];
            RefreshMemory(sensor.gameObject, target);
        }
    }

    public void RefreshMemory(GameObject agent, GameObject target)
    {
        AIMemory memory = FetchMemory(target);
        memory.gameObject = target;
        memory.position = target.transform.position;
        memory.direction = target.transform.position - agent.transform.position;
        memory.distance = memory.direction.magnitude;
        memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
        memory.lastSeen = Time.time;
    }

    public AIMemory FetchMemory(GameObject gameObject)
    {
        AIMemory memory = Memories.Find(x => x.gameObject == gameObject);

        if (memory == null)
        {
            memory = new AIMemory();
            Memories.Add(memory);
        }

        return memory;
    }

    public void ForgetMemories(float olderThan)
    {
        Memories.RemoveAll(x => x.Age > olderThan);
        Memories.RemoveAll(x => !x.gameObject);
        Memories.RemoveAll(x => !x.gameObject.TryGetComponent<IDamagable>(out var status) || status.IsDead);
    }

    public void ForgetTarget(GameObject gameObject)
    {
        Memories.RemoveAll(x => x.gameObject == gameObject);
    }
}
