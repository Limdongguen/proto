using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhereIsCat : MonoBehaviour {

	public Transform cat;
	public Transform cam;
	public Text tt;
	public GameObject whereCat;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curPos = new Vector3 (cam.position.x,0,cam.position.z);
		Vector3 camDir = cam.forward;

		camDir.y = 0;
		float angle = Vector3.Angle (curPos - cat.position, camDir);

		if (angle < 160) {

			whereCat.SetActive (true);
			whereCat.transform.LookAt (cat.position);

		} else {
			whereCat.SetActive (false);
		}
		/*
		Vector3 curPos = new Vector3 (cam.position.x,0,cam.position.z);
		Vector3 camDir = cam.forward;

		camDir.y = 0;
		float angle = Vector3.Angle (curPos - cat.position, camDir);
		if (angle < 160){
			whereCat.position = Vector3.SmoothDamp (whereCat.position, cam.position + cam.forward * 1f, ref velocity, 0.1f);
		}
		*/
		
	}
}
