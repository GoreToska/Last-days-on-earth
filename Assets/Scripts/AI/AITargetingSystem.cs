using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AISensor)), ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    [Header("How long agent can remember about target")]
    public float memorySpan = 2.0f;
    [Header("How distance from target is affecting for choosing (0 - not affect)")]
    public float distanceWeight = 1.0f;
    [Header("How angle from target is affecting for choosing (0 - not affect)")]
    public float angleWeight = 1.0f;
    [Header("How age of memory is affecting for choosing (0 - not affect)")]
    public float ageWeight = 1.0f;

    private AISensoryMemory memory = new AISensoryMemory(10);
    private AISensor sensor;
    private AIMemory bestMemory;

    private void Start()
    {
        sensor = GetComponent<AISensor>();
    }

    private void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);

        EvaluateScores();
    }

    private void EvaluateScores()
    {
        bestMemory = null;

        foreach (var memory in memory.memories)
        {
            memory.scrore = CalculateScore(memory);

            if (bestMemory == null || memory.scrore > bestMemory.scrore)
            {
                bestMemory = memory;
            }
        }
    }

    private float CalculateScore(AIMemory memory)
    {
        float distanceScore = Normalize(memory.distance, sensor.distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, sensor.angle) * angleWeight;
        float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;

        return distanceScore + angleScore + ageScore;
    }

    private float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }

    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (var memory in memory.memories)
        {
            maxScore = Mathf.Max(maxScore, memory.scrore);
        }

        foreach (var memory in memory.memories)
        {
            Color color = Color.blue;
            color.a = memory.scrore / maxScore;

            if (memory == bestMemory)
            {
                color = Color.green;
            }

            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 0.2f);
        }
    }


    public bool HasTarget
    {
        get
        {
            return bestMemory != null;
        }
    }

    public GameObject Target
    {
        get
        {
            return bestMemory.gameObject;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return bestMemory.gameObject.transform.position;
        }
    }

    public bool TargetIsInSight
    {
        get
        {
            return bestMemory.Age < 1f;
        }
    }

    public float TargetDistance
    {
        get
        {
            return bestMemory.distance;
        }
    }
}
