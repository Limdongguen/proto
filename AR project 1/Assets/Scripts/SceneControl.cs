using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour {

	public Transform rotateMain;


	public void LoadMainScene()
	{
		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}
}
