using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class OuterFacingC : MonoBehaviour
{
    public float innerRadius = 0.5f;
    public float outerRadius = 1f;
    public float height = 0.5f;

    [Range(3, 128)]
    public int segments = 32;

    [Range(0f, 360f)]
    public float arcDegrees = 270f;

    void OnEnable() => Generate();
    void OnValidate() => Generate();

    void Generate()
    {
        Mesh mesh = new Mesh();
        mesh.name = "OuterFacingC";

        int vertsPerArc = segments + 1;
        Vector3[] vertices = new Vector3[vertsPerArc * 4 + 8]; // 4 per arc + 4 per tip (start/end)
        int vi = 0;

        float startAngle = 0f;
        float endAngle = Mathf.Deg2Rad * arcDegrees;

        // --- Arc vertices ---
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            Vector3 innerB = new Vector3(cos * innerRadius, 0, sin * innerRadius);
            Vector3 innerT = innerB + Vector3.up * height;
            Vector3 outerB = new Vector3(cos * outerRadius, 0, sin * outerRadius);
            Vector3 outerT = outerB + Vector3.up * height;

            // Each surface gets its own vertex
            vertices[vi++] = outerB; // outer wall bottom
            vertices[vi++] = outerT; // outer wall top
            vertices[vi++] = innerB; // inner wall bottom
            vertices[vi++] = innerT; // inner wall top
        }

        // --- Tip vertices ---
        int startIndex = vi;
        vertices[vi++] = new Vector3(Mathf.Cos(startAngle) * innerRadius, 0, Mathf.Sin(startAngle) * innerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(startAngle) * innerRadius, height, Mathf.Sin(startAngle) * innerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(startAngle) * outerRadius, 0, Mathf.Sin(startAngle) * outerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(startAngle) * outerRadius, height, Mathf.Sin(startAngle) * outerRadius);

        int endIndex = vi;
        vertices[vi++] = new Vector3(Mathf.Cos(endAngle) * innerRadius, 0, Mathf.Sin(endAngle) * innerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(endAngle) * innerRadius, height, Mathf.Sin(endAngle) * innerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(endAngle) * outerRadius, 0, Mathf.Sin(endAngle) * outerRadius);
        vertices[vi++] = new Vector3(Mathf.Cos(endAngle) * outerRadius, height, Mathf.Sin(endAngle) * outerRadius);

        // --- Triangles ---
        int[] triangles = new int[segments * 24 + 12 * 2];
        int ti = 0;

        for (int i = 0; i < segments; i++)
        {
            int baseIndex = i * 4;

            int outB0 = baseIndex;
            int outT0 = baseIndex + 1;
            int inB0 = baseIndex + 2;
            int inT0 = baseIndex + 3;

            int outB1 = baseIndex + 4;
            int outT1 = baseIndex + 5;
            int inB1 = baseIndex + 6;
            int inT1 = baseIndex + 7;

            // --- Outer wall (faces outward) ---
            triangles[ti++] = outB0; triangles[ti++] = outT0; triangles[ti++] = outT1;
            triangles[ti++] = outB0; triangles[ti++] = outT1; triangles[ti++] = outB1;

            // --- Inner wall (flip to face outward) ---
            triangles[ti++] = inB1; triangles[ti++] = inT1; triangles[ti++] = inT0;
            triangles[ti++] = inB1; triangles[ti++] = inT0; triangles[ti++] = inB0;

            // --- Top cap (faces outward) ---
            triangles[ti++] = outT0; triangles[ti++] = inT0; triangles[ti++] = outT1;
            triangles[ti++] = outT1; triangles[ti++] = inT0; triangles[ti++] = inT1;

            // --- Bottom cap (faces outward) ---
            triangles[ti++] = outB0; triangles[ti++] = outB1; triangles[ti++] = inB0;
            triangles[ti++] = outB1; triangles[ti++] = inB1; triangles[ti++] = inB0;
        }

        // --- End tip faces (flip to face outward) ---
        // Start tip
        triangles[ti++] = startIndex + 2; triangles[ti++] = startIndex; triangles[ti++] = startIndex + 3;
        triangles[ti++] = startIndex; triangles[ti++] = startIndex + 1; triangles[ti++] = startIndex + 3;

        // End tip
        triangles[ti++] = endIndex + 2; triangles[ti++] = endIndex + 3; triangles[ti++] = endIndex;
        triangles[ti++] = endIndex; triangles[ti++] = endIndex + 3; triangles[ti++] = endIndex + 1;

        // --- Apply mesh ---
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        var mc = GetComponent<MeshCollider>();
        mc.sharedMesh = null;
        mc.sharedMesh = mesh;
        mc.convex = true;

        var mr = GetComponent<MeshRenderer>();
        if (mr.sharedMaterial == null)
        {
            var mat = new Material(Shader.Find("Standard"));
            mat.color = Color.green;
            mr.sharedMaterial = mat;
        }
    }
}