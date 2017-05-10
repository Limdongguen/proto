using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamNavi : MonoBehaviour {
	
	public GameObject target;
	private UnityEngine.AI.NavMeshAgent navi;

	void Start()
	{
		

		navi = GetComponent<UnityEngine.AI.NavMeshAgent>();

	}

	// Update is called once per frame
	void Update ()
	{
		navi.destination = target.transform.position;	
	
	}
}