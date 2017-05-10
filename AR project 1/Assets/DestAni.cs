using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestAni : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Awake (){
		transform.localScale.Set (0.5F,0.5F,0.5F);
	}
	// Update is called once per frame
	void Update () {
		//transform.position = transform.parent.position + new Vector3(0, Mathf.Sin (Time.time*6)*0.5F +1.5F , 0);

	}
}
