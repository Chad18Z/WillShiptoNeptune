using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMesh : MonoBehaviour {

	#region Fields
	FOVScript fov; // get the fov script
	Mesh mesh; // Mesh for the fov
	RaycastHit2D hit;
	[SerializeField]
	float meshRes = 2; // mesh resolution
	public Vector3[] vertices;
	public int[] triangles;
	public int stepCount;

    // get material
    [SerializeField]
    Material mat;
	#endregion

	// Use this for initialization
	void Start () {
        // Get mesh components
		mesh = GetComponent<MeshFilter>().mesh;
		fov = GetComponentInParent<FOVScript>();
        // Change color of mesh material
        GetComponent<MeshRenderer>().material.color = new Color(255, 250, 0, 0.01f);
        GetComponent<MeshRenderer>().sortingLayerName = "Projectile";


    }
	
	// Update is called once per frame
	void LateUpdate()
	{
        //create the mesh
		MakeMesh();
	}

    /// <summary>
    /// Makes the mesh for the player to see.
    /// </summary>
	void MakeMesh()
	{
		if (fov)
		{
			stepCount = Mathf.RoundToInt(fov.viewAngle * meshRes);
		float stepAngle = fov.viewAngle / stepCount; // Get angle for fov mesh

        //create the list of vertex
		List<Vector3> viewVertex = new List<Vector3>();

		hit = new RaycastHit2D();

        // create raycasts based on stepcount
		for (int i = 0; i <= stepCount; i++)
		{
			float angle = fov.transform.eulerAngles.y - fov.viewAngle / 2 + stepAngle * i;
			Vector3 dir = fov.DirFromAngle(angle, false);

            //create the  raycast based on the direction, radius, and obstacle mask
			hit = Physics2D.Raycast(fov.transform.position, dir, fov.viewRadius, fov.obstacleMask);

            // If raycast hits an object's collider, then stop the ray cast and add it to the list of vertexes
			if (hit.collider == null)
			{
				viewVertex.Add(transform.position + dir.normalized * fov.viewRadius);
			}
			else
			{
				viewVertex.Add(transform.position + dir.normalized * hit.distance);
			}
		}

		int vertexCount = viewVertex.Count + 1;

		vertices = new Vector3[vertexCount];
		triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;

		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);

			if (i < vertexCount - 2)
			{
				triangles[i * 3 + 2] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3] = i + 2;
			}

		}

        mesh.Clear();

    


        // set the mesh properties to those generated above.
        mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		}
	}
}
