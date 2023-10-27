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
    public List<AIMemory> memories = new List<AIMemory>();
    GameObject[] characters;

    public AISensoryMemory(int maxPlayers)
    {
        characters = new GameObject[maxPlayers];
    }

    public void UpdateSenses(AISensor sensor)
    {
        int targets = sensor.Filter(characters, "Character");

        for (int i = 0; i < targets; ++i)
        {
            GameObject target = characters[i];
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
        AIMemory memory = memories.Find(x => x.gameObject == gameObject);

        if (memory == null)
        {
            memory = new AIMemory();
            memories.Add(memory);
        }

        return memory;
    }

    public void ForgetMemories(float olderThan)
    {
        memories.RemoveAll(x => x.Age > olderThan);
        memories.RemoveAll(x => !x.gameObject);
        memories.RemoveAll(x => !x.gameObject.TryGetComponent<CharacterStatusManager>(out var status) || status.IsDead);
    }

    public void ForgetTarget(GameObject gameObject)
    {
        memories.RemoveAll(x => x.gameObject == gameObject);
    }
}
