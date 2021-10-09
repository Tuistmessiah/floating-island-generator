using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public float radius = 100f;
    public float flattenLimit = 90f;
    public float elevation = 10f;

    void Start()
    {
        // this.mesh = new Mesh();
        this.mesh = GenerateCircle(10, this.radius, this.elevation, this.flattenLimit);
        
        this.GetComponent<MeshFilter>().mesh = mesh;

        // CreateShape();
        // UpdateMesh();
    }

    public void Initiate(float _radius, float _elevation, float _flattenLimit, Vector3 _displacement) {
      this.radius = _radius;
      this.elevation = _elevation;
      this.flattenLimit = _flattenLimit;
      this.transform.position += _displacement;
    }

    void CreateShape() {
        // Set vertices
        this.vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for(int z = 0; z <= zSize; z++) {

          for(int x = 0; x <= xSize; x++) {
            float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
            vertices[i] = new Vector3(x, y, z);
            i++;
          }
        }

        // Set triangles
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tri = 0;
        for(int z = 0; z < zSize; z++) {
          for(int x = 0; x < xSize; x++) {
            triangles[tri + 0] = vert;
            triangles[tri + 1] = vert + xSize + 1;
            triangles[tri + 2] = vert + 1;

            triangles[tri + 3] = vert + 1;
            triangles[tri + 4] = vert + xSize + 1;
            triangles[tri + 5] = vert + xSize + 2;

            vert++;
            tri += 6;
          }
            vert++;
        }
    }

    // Get the index of point number 'x' in circle number 'c'
    static int GetPointIndex(int c, int x) {
        if (c < 0) return 0; // In case of center point
        x = x % ((c + 1) * 6); // Make the point index circular
                              // Explanation: index = number of points in previous circles + central point + x
                              // hence: (0+1+2+...+c)*6+x+1 = ((c/2)*(c+1))*6+x+1 = 3*c*(c+1)+x+1
        return (3 * c * (c + 1) + x + 1);
    }

    public static Mesh GenerateCircle(int res, float _radius, float _elevation, float _flattenLimit) {
      float d = 1f / res;

      var vtc = new List<Vector3>();
      vtc.Add(new Vector3(.0f, Mathf.PerlinNoise(.0f, .0f) * 2f, .0f)); // Start with only center point
      var tris = new List<int>();

      // First pass => build vertices
      for (int circ = 0; circ < res; ++circ) {
          float angleStep = (Mathf.PI * 2f) / ((circ + 1) * 6);
          for (int point = 0; point < (circ + 1) * 6; ++point) {
              float x = _radius * Mathf.Sin(angleStep * point) * d * (circ + 1);
              float z = _radius * Mathf.Cos(angleStep * point) * d * (circ + 1);
              float y = Mathf.PerlinNoise(x * .3f, z * .3f) * _elevation;
              if(circ >= res * _flattenLimit) y /= (circ + 1);
              vtc.Add(new Vector3(x, y, z));
          }
      }

      // Second pass => connect vertices into triangles
      for (int circ = 0; circ < res; ++circ) {
          for (int point = 0, other = 0; point < (circ + 1) * 6; ++point) {
              if (point % (circ + 1) != 0) {
                  // Create 2 triangles
                  tris.Add(GetPointIndex(circ - 1, other + 1));
                  tris.Add(GetPointIndex(circ - 1, other));
                  tris.Add(GetPointIndex(circ, point));
                  tris.Add(GetPointIndex(circ, point));
                  tris.Add(GetPointIndex(circ, point + 1));
                  tris.Add(GetPointIndex(circ - 1, other + 1));
                  ++other;
              } else {
                  // Create 1 inverse triange
                  tris.Add(GetPointIndex(circ, point));
                  tris.Add(GetPointIndex(circ, point + 1));
                  tris.Add(GetPointIndex(circ - 1, other));
                  // Do not move to the next point in the smaller circle
              }
          }
      }

      // Create the mesh
      var m = new Mesh();
      m.SetVertices(vtc);
      m.SetTriangles(tris, 0);
      m.RecalculateNormals();
      m.UploadMeshData(true);

      // foreach(Vector3 vert in vtc) {
      //   DrawPrimitives.instance.DrawBall(vert, _radius / res / 10f, Color.red);
      // }

      return m;
    }

    // private void OnDrawGizmos() {
    //   if(vertices == null) return;

    //   for(int i = 0; i < vertices.Length; i++) {
    //     Gizmos.DrawSphere(vertices[i], .1f);
    //   }
    // }

    void UpdateMesh() {
      mesh.Clear();

      mesh.vertices = this.vertices;
      mesh.triangles = this.triangles;

      mesh.RecalculateNormals();
    }
}
