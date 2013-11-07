// C#
// SplitMeshIntoTriangles.cs
using UnityEngine;
using System.Collections;
 
public class SplitMeshIntoTriangles : MonoBehaviour
{
	public float TimeScale;
    IEnumerator SplitMesh ()
    {
        MeshFilter MF = GetComponent<MeshFilter>();
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Mesh M = MF.mesh;
        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);
            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    //newNormals[n] = normals[index];
                }
                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;
 
                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
 
                GameObject GO = new GameObject("Triangle " + (i / 3));
				GO.transform.localScale = new Vector3 (1.25f, 1.25f, 1.25f);
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                GO.AddComponent<Rigidbody>();
				
				GO.transform.tag = "shard";
				
				// bk hacks
				GO.rigidbody.useGravity = false;
				
				int xPow = Random.Range(0,300);
				int zPow = Random.Range(0,-1000);
				GO.rigidbody.AddExplosionForce(xPow, transform.position, zPow);
 
				
                Destroy(GO, Random.Range(0.0f, 2.0f) + Random.Range(1.0f, 2.0f));
            }
			
        }
		
        MR.enabled = false;
 
        //Time.timeScale = 0.2f;
		Time.timeScale = TimeScale;//1.0f;
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
	
    void OnMouseDown()
    {
        StartCoroutine(SplitMesh());
		
		//GameObject script = GameObject.Find("Boss");
		//BossLogic other = (BossLogic) script.GetComponent(typeof(BossLogic));
		//other.destroyed = true;
    }
	
	public void blowUp() {
		StartCoroutine(SplitMesh());
	}
	
	

	
}