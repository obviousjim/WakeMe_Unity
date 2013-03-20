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
		Debug.Log ("loading OBJs at " + path);
		
		GameObject loaderCopy = Instantiate(loaderPrefab,loaderPrefab.transform.position, 
											loaderPrefab.transform.rotation) as GameObject;
		
		lastcreated = loaderCopy;
		loaderCopy.SetActive(false);
		
		//Transform front = loaderCopy.transform.FindChild("frontMesh");
		//Transform back  = loaderCopy.transform.FindChild("backMesh");
		
		OBJ objectLoader = loaderCopy.gameObject.AddComponent<OBJ>();
		//OBJ objectLoader2 = back.gameObject.AddComponent<OBJ>();

		objectLoader.objPath = path+"/mesh.obj";
		//objectLoader2.objPath = path+"/mesh2.obj";
		
		objectLoader.material = defaultMaterial;
		//objectLoader2.material = defaultMaterial;
		
		yield return StartCoroutine( objectLoader.Load(objectLoader.objPath) ); //objectLoader1.CreateOBJ();
		//SetupColliders(front.gameObject, defaultMaterial);
		//yield return StartCoroutine( objectLoader2.Load(objectLoader2.objPath) ); 
		//SetupColliders(back.gameObject, defaultMaterial);
		
		WWW photo = new WWW(path+"/photo.jpg");
	    // Wait for download to complete
    	yield return photo;
	    // assign texture
    	
		Debug.Log ("texture size " + photo.texture.width + " " + photo.texture.height);
		Material mat = new Material(sourceShader);
		mat.mainTexture = photo.texture; 	

		//floor.renderer.material.SetTextureScale("Tiling", new Vector2(100,0));
		//floor.renderer.material.mainTexture = www.texture;
		
		/*
		MeshFilter[] meshFilters = loaderCopy.GetComponentsInChildren<MeshFilter>() as MeshFilter[];
    	CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i < meshFilters.Length; i++){
        	combine[i].mesh = meshFilters[i].sharedMesh;
        	combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        	meshFilters[i].gameObject.SetActive(false);
    	}
    	*/
		
		MeshFilter filter = loaderCopy.gameObject.GetComponent<MeshFilter>();
		filter.mesh.RecalculateNormals();
		loaderCopy.gameObject.renderer.sharedMaterial = mat;
		
		loaderCopy.gameObject.AddComponent<Rigidbody>();
		MeshCollider collider = loaderCopy.gameObject.AddComponent<MeshCollider>();
		collider.sharedMesh = filter.mesh;
		collider.convex = true;
		
		//Resources.UnloadUnusedAssets();
		//Debug.Log ("mesh renderer? " + back.gameObject.ren<MeshRenderer>());
		loaderCopy.SetActive(true);
	}
	
	
	void SetupColliders(GameObject obj, Material mat){
		obj.AddComponent<MeshRenderer>();
		obj.renderer.sharedMaterial = mat;
		//obj.AddComponent<Rigidbody>();
		//MeshFilter filter = obj.GetComponent<MeshFilter>();
		//MeshCollider collider = obj.AddComponent<MeshCollider>();
		//collider.sharedMesh = filter.mesh;
		//collider.convex = true;
	}
	
	
	void MeshesComplete(GameObject meshObject){
		Debug.Log ("*** COMPLETED MESHES");
		//meshObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
	}		
}
