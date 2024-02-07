using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _debug = true;
    public Color MeshColor = Color.red;

    [Header("Vision Options")]
    public float Distance = 10;
    public float Angle = 30;
    public float Height = 1.0f;
    public int ScanFrequency = 30;
    public LayerMask TargetLayers;
    public LayerMask ObstaclesLayer;
    public List<string> LayerNames = new List<string>();

    public List<GameObject> Objects
    {
        get { objects.RemoveAll(obj => obj == null); return objects; }
    }
    public List<GameObject> objects = new List<GameObject>();

    private Collider[] colliders = new Collider[50];
    private int count;
    private float scanInterval;
    private float scanTimer;
    private Mesh mesh;

    private void Start()
    {
        scanInterval = 1.0f / ScanFrequency;
    }

    private void Update()
    {
        Scan();
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, Distance, colliders, TargetLayers, QueryTriggerInteraction.Collide);
        objects.Clear();

        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject.transform.root.gameObject;

            if (IsInside(obj) && !Objects.Contains(obj))
            {
                if (obj.TryGetComponent<IDamagable>(out var status) && status.IsDead == false)
                {
                    objects.Add(obj);
                }
            }
        }
    }

    public void AddTarget(GameObject target)
    {
        objects.Add(target);
    }

    public bool IsInside(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < -Height || direction.y > Height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > Angle)
        {
            return false;
        }

        origin.y += Height / 2;
        destination.y = origin.y;
        if (Physics.Linecast(origin, destination, ObstaclesLayer))
        {
            return false;
        }

        return true;
    }

    public int Filter(GameObject[] buffer, string layerName = null, string layerName2 = null, string layerName3 = null)
    {
        //var layer = LayerMask.NameToLayer(layerName);
        //int layer2 = 0;
        //var layer3 = 0;

        //if (layerName2 != null)
        //{
        //    layer2 = LayerMask.NameToLayer(layerName2);
        //}

        //if (layerName3 != null)
        //{
        //    layer3 = LayerMask.NameToLayer(layerName3);
        //}

        int count = 0;

        //foreach (var obj in Objects)
        //{
        //    if ((obj.layer == layer || obj.layer == layer2 || obj.layer == layer3) && obj.TryGetComponent<IDamagable>(out var status) && status.IsDead == false)
        //    {
        //        buffer[count++] = obj;
        //    }

        //    if (buffer.Length == count)
        //    {
        //        break; // buffer is full
        //    }
        //}

        foreach (var obj in Objects)
        {
            if (obj.TryGetComponent<IDamagable>(out var status) && status.IsDead == false)
            {
                buffer[count++] = obj;
            }

            if (buffer.Length == count)
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
        Vector3 bottomLeft = Quaternion.Euler(0, -Angle, 0) * Vector3.forward * Distance;
        Vector3 bottomRight = Quaternion.Euler(0, Angle, 0) * Vector3.forward * Distance;

        Vector3 topCenter = bottomCenter + Vector3.up * Height;
        Vector3 topLeft = bottomLeft + Vector3.up * Height;
        Vector3 topRight = bottomRight + Vector3.up * Height;

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

        float currentAngle = -Angle;
        float deltaAngle = (Angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * Distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * Distance;

            topLeft = bottomLeft + Vector3.up * Height;
            topRight = bottomRight + Vector3.up * Height;

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
        if (!_debug)
            return;

        if (mesh)
        {
            Gizmos.color = MeshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, Distance);
    }
}
