using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InstPath : MonoBehaviour { 

	static public GameObject pre = GameObject.Find("PreNaviLine");
	static public GameObject prePath = GameObject.Find("PrePath1");
	[MenuItem ("Mymenu/Intantiate Selected")]
	static void CreatePfs()
	{
		pre = GameObject.Find("PreNaviLine");
        prePath = GameObject.Find("PrePath1");
        GameObject[] go = Selection.gameObjects;
		float dist = Vector3.Distance (go[0].transform.position,go[1].transform.position);
		dist /= 5;
		for(int k=0;k<dist;k++)
		{
			
			Instantiate (pre, go[0].transform.position * (1 - (k / dist)) + go[1].transform.position * (k / dist), Quaternion.LookRotation (go[0].transform.position-go[1].transform.position),prePath.transform);
		}
	}
}
