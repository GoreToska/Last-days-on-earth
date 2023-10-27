using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [Header("Debug")]
    public Color meshColor = Color.red;

    [Header("Vision Options")]
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public int scanFrequency = 30;
    public LayerMask targetLayers;
    public LayerMask obstaclesLayer;
    public List<GameObject> Objects
    {
        get { objects.RemoveAll(obj => !obj); return objects; }
    }
    public List<GameObject> objects = new List<GameObject>();

    private Collider[] colliders = new Collider[50];
    private int count;
    private float scanInterval;
    private float scanTimer;
    private Mesh mesh;

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        //scanTimer -= Time.deltaTime;

        //if (scanTimer < 0)
        //{
        //    scanTimer += scanInterval;
        //    Scan();
        //}

        Scan();
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayers, QueryTriggerInteraction.Collide);

        objects.Clear();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInside(obj) && !objects.Contains(obj))
            {
                if(obj.TryGetComponent<CharacterStatusManager>(out var status) && !status.IsDead)
                objects.Add(obj);
            }
        }
    }

    public bool IsInside(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        destination.y = origin.y;
        if(Physics.Linecast(origin, destination, obstaclesLayer))
        {
            return false;
        }

        return true;
    }

    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach (var obj in Objects)
        {
            if(obj.layer == layer && obj.TryGetComponent<CharacterStatusManager>(out var status) && !status.IsDead)
            {
                buffer[count++] = obj;
            }

            if(buffer.Length == count)
            {
                break; // buffer is full
            }
        }

        return count;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numberOfTriangles = (segments * 4) + 2 + 2;
        int numberOfVertices = numberOfTriangles * 3;

        Vector3[] vertices = new Vector3[numberOfVertices];
        int[] triangles = new int[numberOfVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        //  left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //  right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            //  far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //  top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //  bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numberOfVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);

        //Gizmos.color = Color.green;
        //foreach (var obj in Objects)
        //{
        //    Gizmos.DrawSphere(obj.transform.position, 0.2f);
        //}
    }
}
