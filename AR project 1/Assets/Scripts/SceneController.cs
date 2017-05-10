using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
		SceneManager.LoadScene (1, LoadSceneMode.Single);
		//SceneManager.SetActiveScene (SceneManager.GetSceneByBuildIndex(1));
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
