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
		
		GameObject loaderCopy = Instantiate(loaderPrefab,Vector3.zero, Quaternion.identity) as GameObject;
		lastcreated = loaderCopy;
		Transform front = loaderCopy.transform.FindChild("frontMesh");
		Transform back  = loaderCopy.transform.FindChild("backMesh");
		
		OBJ objectLoader1 = front.gameObject.AddComponent<OBJ>();
		OBJ objectLoader2 = back.gameObject.AddComponent<OBJ>();

		objectLoader1.objPath = path+"/mesh1.obj";
		objectLoader2.objPath = path+"/mesh2.obj";
		
		objectLoader1.material = defaultMaterial;
		objectLoader2.material = defaultMaterial;
		
		yield return StartCoroutine( objectLoader1.Load(objectLoader1.objPath) ); //objectLoader1.CreateOBJ();
		//SetupColliders(front.gameObject, defaultMaterial);
		yield return StartCoroutine( objectLoader2.Load(objectLoader2.objPath) ); 
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
		
		MeshFilter[] meshFilters = loaderCopy.GetComponentsInChildren<MeshFilter>() as MeshFilter[];
    	CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i < meshFilters.Length; i++){
        	combine[i].mesh = meshFilters[i].sharedMesh;
        	combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        	meshFilters[i].gameObject.SetActive(false);
    	}
		MeshFilter filter = loaderCopy.gameObject.AddComponent<MeshFilter>();
    	filter.mesh = new Mesh();
    	filter.mesh.CombineMeshes(combine);
		loaderCopy.gameObject.AddComponent<MeshRenderer>();
		loaderCopy.gameObject.renderer.sharedMaterial = mat;
	
				
		loaderCopy.gameObject.AddComponent<Rigidbody>();
		MeshCollider collider = loaderCopy.gameObject.AddComponent<MeshCollider>();
		collider.sharedMesh = filter.mesh;
		collider.convex = true;
		filter.mesh.RecalculateNormals();
		Destroy (front.gameObject);
		Destroy (back.gameObject);

		Resources.UnloadUnusedAssets();
		
		//Debug.Log ("mesh renderer? " + back.gameObject.ren<MeshRenderer>());
		
	}
	
	
	void SetupColliders(GameObject obj, Material mat){
		obj.AddComponent<MeshRenderer>();
		obj.renderer.sharedMaterial = mat;
		//obj.AddComponent<Rigidbody>();
		MeshFilter filter = obj.GetComponent<MeshFilter>();
		//MeshCollider collider = obj.AddComponent<MeshCollider>();
		//collider.sharedMesh = filter.mesh;
		//collider.convex = true;
	}
	
	
	void MeshesComplete(GameObject meshObject){
		Debug.Log ("*** COMPLETED MESHES");
		//meshObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
	}		
}
