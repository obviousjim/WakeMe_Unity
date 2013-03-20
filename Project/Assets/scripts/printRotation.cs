using UnityEngine;
using System.Collections;

public class printRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log (	gameObject.transform.rotation.x + " " + 
					gameObject.transform.rotation.y + " " + 
					gameObject.transform.rotation.z + " " + 
					gameObject.transform.rotation.w);
		Debug.Log (gameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
