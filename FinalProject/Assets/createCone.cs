using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCone : MonoBehaviour {
    public GameObject cone;
    public Transform bodyTransform;
    Mesh mesh;
    MeshRenderer meshRenderer;
    List<Vector3> vertices;
    List<int> triangles;
    public Material material;
    public int segments = 7;
    public float radius = 1.0f;
    public float height = 2.0f;
    public Transform LHand;
    Vector3 pos;
    float angle = 0.0f;
    float angleAmount = 0.0f;
    // Use this for initialization
    void Start () {
        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new List<Vector3>();
        pos = new Vector3();
        angleAmount = 2 * Mathf.PI / segments;
        angle = 0.0f;
        pos.x = 0.0f;
        pos.y = height;
        pos.z = 0.0f;
        vertices.Add(new Vector3(pos.x, pos.y, pos.z));
        pos.y = 0.0f;
        vertices.Add(new Vector3(pos.x, pos.y, pos.z));
        for (int i = 0; i < segments; i++)
        {
            pos.x = radius * Mathf.Sin(angle);
            pos.z = radius * Mathf.Cos(angle);
            vertices.Add(new Vector3(pos.x, pos.y, pos.z));
            angle -= angleAmount;
        }
        mesh.vertices = vertices.ToArray();
        triangles = new List<int>();
        for (int i = 2; i < segments + 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(segments + 1);
        for (int i = 2; i < segments + 1; i++)
        {
            triangles.Add(1);
            triangles.Add(i);
            triangles.Add(i + 1);
        }
        triangles.Add(1);
        triangles.Add(segments + 1);
        triangles.Add(2);
        mesh.triangles = triangles.ToArray();
    }

    // Update is called once per frame
    void Update () {
        cone.transform.position = LHand.position;
    }

}
