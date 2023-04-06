using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeMesh : MonoBehaviour
{
    private Mesh mesh;
    private Material material;

    public Color color = Color.black;
    [Min(3)] public int numSides = 16;
    [Min(0)] public float radius = 0.5f;
    public float height = 1f;

    public enum BasePlane { XY, YZ, ZX }
    public BasePlane basePlane = BasePlane.ZX;

    private void OnEnable()
    {
        Redraw();
    }

    private void OnDisable()
    {
        mesh = null;
        material = null;
    }

    public void Redraw()
    {
        if (material == null)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            material.SetFloat("_Surface", 1);  // Transparent
            GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        if (mesh == null)
        {
            mesh = new Mesh { name = "Cone Mesh" };
        }
        mesh.Clear(false);

        // Create arrays to hold the vertices and triangles of the mesh
        Vector3[] vertices = new Vector3[numSides + 1];
        int[] triangles = new int[numSides * 3 + (numSides - 2) * 3];

        // Unit vectors for cone oriented forward (i.e. along the z axis)
        Vector3 e1 = Vector3.right;
        Vector3 e2 = Vector3.up;
        Vector3 e3 = Vector3.forward;

        switch (basePlane)
        {
            case BasePlane.YZ:
                e1 = Vector3.forward;
                e2 = Vector3.up;
                e3 = Vector3.right;
                break;
            case BasePlane.ZX:
                e1 = Vector3.forward;
                e2 = Vector3.right;
                e3 = Vector3.up;
                break;
        }

        // Calculate the angle between each vertex of the base polygon
        float deltaAngle = 360f / numSides;

        // Generate the vertices of the base polygon
        for (int i = 0; i < numSides; i++)
        {
            float angle = Mathf.Deg2Rad * (i * deltaAngle);
            Vector3 r1 = Mathf.Cos(angle) * e1;
            Vector3 r2 = Mathf.Sin(angle) * e2;
            vertices[i] = radius * (r1 + r2);
        }

        // Set the vertex at the top of the cone
        vertices[numSides] = height * e3;

        // Generate the triangles of the mesh
        for (int i = 0; i < numSides; i++)
        {
            triangles[i * 3 + 0] = i;
            triangles[i * 3 + 1] = numSides;
            triangles[i * 3 + 2] = (i + 1) % numSides;
        }
        int baseIndex = 3 * numSides;
        for (int i = 0; i < numSides - 2; i++)
        {
            triangles[baseIndex + i * 3 + 0] = 0;
            triangles[baseIndex + i * 3 + 1] = i + 1;
            triangles[baseIndex + i * 3 + 2] = i + 2;
        }

        // Set the mesh's vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate the mesh's normals
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        GetComponent<MeshFilter>().mesh = mesh;
        material.color = color;
    }

    public void SetMaterial(Material material)
    {
        GetComponent<MeshRenderer>().sharedMaterial = material;
        color = material.color;
        this.material = material;
    }
}
