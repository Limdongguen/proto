using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

	SetMaincam mapCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	
		
		GameObject temp=GameObject.Find ("ARCamera");
		float dist=Vector3.Distance (temp.transform.position,transform.position);
		dist /= 4;
		transform.localScale = new Vector3 (dist,dist,dist);
		transform.position = new Vector3(transform.position.x,dist/3.0F,transform.position.z);

		mapCam = GameObject.Find ("MapCamera").GetComponent<SetMaincam> ();
		if (mapCam.isChoose) {
			float size = mapCam.mapcam.orthographicSize / 3.5F;
			transform.localScale = new Vector3 (size,size,size);
		}
	}
}
