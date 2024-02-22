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

    private AISensoryMemory _memory = new AISensoryMemory(10);
    private AISensor _sensor;
    private AIMemory _bestMemory;

    private void Awake()
    {
        _sensor = GetComponent<AISensor>();
    }

    private void Update()
    {
        _memory.UpdateSenses(_sensor);
        _memory.ForgetMemories(memorySpan);

        EvaluateScores();
    }

    public void ForgetTarget(GameObject target)
    {
        _memory.ForgetTarget(target);
    }

    public void AddMemory(GameObject target, float score)
    {
        var memory = _memory.FetchMemory(target);
        _memory.RefreshMemory(this.gameObject, target);
        memory.scrore = score;
    }

    private void EvaluateScores()
    {
        _bestMemory = null;

        foreach (var memory in _memory.Memories)
        {
            memory.scrore = CalculateScore(memory);

            if (_bestMemory == null || memory.scrore > _bestMemory.scrore)
            {
                _bestMemory = memory;
            }
        }
    }

    private float CalculateScore(AIMemory memory)
    {
        float distanceScore = Normalize(memory.distance, _sensor.Distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, _sensor.Angle) * angleWeight;
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
        foreach (var memory in _memory.Memories)
        {
            maxScore = Mathf.Max(maxScore, memory.scrore);
        }

        foreach (var memory in _memory.Memories)
        {
            Color color = Color.blue;
            color.a = memory.scrore / maxScore;

            if (memory == _bestMemory)
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
            return _bestMemory != null;
        }
    }

    public GameObject Target
    {
        get
        {
            return _bestMemory.gameObject;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return _bestMemory.gameObject.transform.position;
        }
    }

    public bool TargetIsInSight
    {
        get
        {
            return _bestMemory.Age < 1f;
        }
    }

    public float TargetDistance
    {
        get
        {
            return _bestMemory.distance;
        }
    }
}
