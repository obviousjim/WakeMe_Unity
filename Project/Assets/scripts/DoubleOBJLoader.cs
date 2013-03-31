using UnityEngine;
using System.Collections;

public class DoubleOBJLoader : MonoBehaviour {
	
	public GameObject loaderPrefab;
	public Material defaultMaterial;
	public Shader sourceShader;
	public Transform spawnTarget;
	public GameObject lastcreated;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("r")){
			lastcreated.transform.position = spawnTarget.position;
			lastcreated.transform.rotation = spawnTarget.rotation;
		}
	}
	
	IEnumerator LoadNewOBJ(string path)  {
		float startTime = Time.time;
		Debug.Log ("loading OBJs at " + path + " START TIME " + Time.time);
		
		GameObject loaderCopy = Instantiate(loaderPrefab,loaderPrefab.transform.position, 
											loaderPrefab.transform.rotation) as GameObject;
		lastcreated = loaderCopy;
		loaderCopy.SetActive(false);
		
		OBJ objectLoader = loaderCopy.gameObject.AddComponent<OBJ>();
		objectLoader.objPath = path+"/mesh.obj";
		objectLoader.material = defaultMaterial;
		
		Debug.Log ("STEP 2 " + (Time.time - startTime) );
		
		yield return StartCoroutine( objectLoader.Load(objectLoader.objPath) );

		Debug.Log ("STEP 3 " + (Time.time - startTime) );
		
		WWW photo = new WWW(path+"/photo.jpg");
	    // Wait for download to complete
    	yield return photo;
		
		Debug.Log ("STEP 4 " + (Time.time - startTime) );		
		
		Debug.Log ("texture size " + photo.texture.width + " " + photo.texture.height);
		Material mat = new Material(sourceShader);
	    // assign texture
		mat.mainTexture = photo.texture; 	

		MeshFilter filter = loaderCopy.gameObject.GetComponent<MeshFilter>();
		filter.mesh.RecalculateNormals();
		loaderCopy.gameObject.renderer.sharedMaterial = mat;
		
		loaderCopy.gameObject.AddComponent<Rigidbody>();
		MeshCollider collider = loaderCopy.gameObject.AddComponent<MeshCollider>();
		collider.sharedMesh = filter.mesh;
		collider.convex = true;
		
		Debug.Log ("STEP 5 " + (Time.time - startTime) );
		
		loaderCopy.SetActive(true);
		
		Debug.Log ("STEP 6 " + (Time.time - startTime) );
		
	}
	
	void MeshesComplete(GameObject meshObject){
		Debug.Log ("*** COMPLETED MESHES");
		//meshObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
	}		
}
