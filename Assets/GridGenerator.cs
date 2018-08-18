using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    MeshData gridMeshData;

    public int sizeX, sizeY;

	public GameObject gridObject;

    struct MeshData
    {
        Vector3[] vertices;
        int[] indices;

        public int[] Indices
        {
            get
            {
                return indices;
            }

            set
            {
                indices = value;
            }
        }

        public Vector3[] Vertices
        {
            get
            {
                return vertices;
            }

            set
            {
                vertices = value;
            }
        }
    };
    // Use this for initialization
    void Start()
    {
		gridMeshData = new MeshData();
        GenerateData(100, 100, 100, 100, ref gridMeshData);
		PopulateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

	void PopulateMesh()
	{
		Mesh m = new Mesh();
		m.vertices = gridMeshData.Vertices;
		m.triangles = gridMeshData.Indices;
		gridObject.GetComponent<MeshFilter>().mesh = m;
	}

    void GenerateData(uint width, uint height, uint verticesX, uint verticesY, ref MeshData mesh)
    {
        uint vertexCount = verticesX * verticesY;
        uint triangleCount = (verticesX - 1) * (verticesY - 1) * 2;

        //Create the vertices
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        float dx = width / (verticesX - 1);
        float dy = height / (verticesY - 1);

        int index = 0;
        mesh.Vertices = new Vector3[(int)vertexCount];
        for (int rows = 0; rows < verticesY ; rows++)
        {
            for (int cols = 0; cols < verticesX ; cols++)
            {
                mesh.Vertices[index++] = new Vector3(-halfWidth + dx * cols, -halfHeight + dy * rows, 0.0f);
            }
        }

        //Create indices
        mesh.Indices = new int[triangleCount * 3];
        index = 0;

        for (int rows = 0; rows < verticesY -1; rows++)
        {
            for (int cols = 0; cols < verticesX-1; cols++)
            {
                //Lower Triangle
                mesh.Indices[index++] = (int)(rows*verticesX + cols+1);
                mesh.Indices[index++] = (int)(rows*verticesX + cols);
                mesh.Indices[index++] = (int)((verticesX*(rows+1)) + cols);

                //Upper Triangle
                mesh.Indices[index++] = (int)(rows*verticesX + cols+1);
                mesh.Indices[index++] = (int)((verticesX*(rows+1)) + cols);
                mesh.Indices[index++] = (int)((verticesX*(rows+1)) + cols+1);
            }
        }
    }

    /*    private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < gridMesh.Vertices.Length; i++)
            {
                Gizmos.DrawSphere(gridMesh.Vertices[i], 0.1f);
            }
        }*/
}
