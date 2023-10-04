using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiSensor : MonoBehaviour {
    public float distance = 10f, height = 10, angle = 10;
    public int segments = 10;
    public Color meshColor;

    public float collisionRadius;

    public int scanFrequency = 30;
    public LayerMask scanLayers, occlusionLayers;


    public List<GameObject> ObjectsCollided = new List<GameObject>();

    Collider[] scanColliders = new Collider[10];
    int scanCollidersCount;
    float scanInterval, scanTimer;

    Mesh mesh;
    void Start() {
        scanInterval = 1f / scanFrequency;
    }

    void Update() {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0) {
            scanTimer += scanInterval;
            Scan();
        }
    }

    void Scan() {
        scanCollidersCount = Physics.OverlapSphereNonAlloc(transform.position, distance, scanColliders, scanLayers, QueryTriggerInteraction.Collide);

        ObjectsCollided.Clear();
        for (int i = 0; i < scanCollidersCount; i++) {
            GameObject obj = scanColliders[i].gameObject;
            if (IsInSight(obj)) {
                ObjectsCollided.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj) {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if ((direction.y + collisionRadius) < origin.y || direction.y > height) {
            return false;
        }

        direction.y = 0f;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle) {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers)) {
            return false;
        }
        
        return true;
    }

    Mesh CreateWedgeMesh() {
        Mesh mesh = new Mesh();

        int numTriangles = (segments * 4) + 2 + 2; // +2 left side and +2 right side
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;

        // Left Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // Right Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++) {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            // Far Side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Bottom (use left hand rule to check that normal point outwards)
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++){
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    void OnValidate() {
        mesh = CreateWedgeMesh();
        scanInterval = 1f / scanFrequency;
    }

    void OnDrawGizmos() {
        if (mesh) {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < scanCollidersCount; i++) {
            Gizmos.DrawSphere(scanColliders[i].transform.position, collisionRadius);
        }

        Gizmos.color = Color.green;
        foreach( var obj in ObjectsCollided ) {
            Gizmos.DrawSphere(obj.transform.position, collisionRadius);
        }
    }
}
