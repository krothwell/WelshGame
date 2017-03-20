using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlaneMesh : MonoBehaviour {
    public float width = 50f;
    public float height = 50f;
	// Use this for initialization
	void Start () {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        //Vertices
        Vector3[] vertices = new Vector3[4] {
            new Vector3(0,0,0), new Vector3(width, 0, 0), new Vector3(0, height, 0), new Vector3(width, height, 0)
        };

        //Triangles
        int[] triangle = new int[6];
        triangle[0] = 0;
        triangle[1] = 2;
        triangle[3] = 1;

        triangle[3] = 2;
        triangle[4] = 3;
        triangle[5] = 1;

        //Normals (only if you want to display object in game).
        Vector3[] normals = new Vector3[4];
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        //UVs (how textures are displayed).
        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[0] = new Vector2(1, 0);
        uv[0] = new Vector2(0, 1);
        uv[0] = new Vector2(1, 1);

        //Assign arrays! 

        mesh.vertices = vertices;
        mesh.triangles = triangle;
        mesh.normals = normals;
        mesh.uv = uv;

    }
	

}
