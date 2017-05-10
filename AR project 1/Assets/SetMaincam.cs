using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaincam : MonoBehaviour {

	public Camera ARcam;
	public Camera mapcam;
	public Transform cat;
	public GameObject currentPositionParent;
	public GameObject currentPosition;
	public GameObject neviMesh;
	public GameObject mapPlane;
	public GameObject mapUI;
	public NavigationController catNavi;
	public Material NaviLineMat;
	public GameObject NaviLinePfs;
	public GameObject destArrow;

	private GameObject dest;
	public bool isChoose;
	private int count;

	public bool see3D;
	// Use this for initialization
	void Start () {
		
		#if UNITY_STANDALONE || UNITY_EDITOR
		ARcam.clearFlags = CameraClearFlags.Skybox;
		#endif
		count = 0;
		isChoose=false;
		dest = GameObject.Find ("ARCamera");
	}
	
	// Update is called once per frame
	void Update () {
		
		count++;
		transform.position = new Vector3 (cat.position.x , transform.position.y, cat.position.z);
		transform.position = transform.position - new Vector3 (0,0,40.0F);
		mapcam.orthographicSize=120;
		if (isChoose) {
			currentPosition.SetActive (true);
			Vector3 middle;
			middle = new Vector3 ((cat.position.x + dest.transform.position.x) / 2.0F, Vector3.Distance (cat.position, dest.transform.position) * 5, (cat.position.z + dest.transform.position.z) / 2.0F);
			mapcam.orthographicSize = Vector3.Distance (cat.position, dest.transform.position) / 0.8F;
			transform.position = middle;
			transform.position = transform.position - new Vector3 (0, 0, Vector3.Distance (cat.position, dest.transform.position) / 2.5F);

			if (dest.name != "ARcamera") {
				destArrow.SetActive (true);
				destArrow.transform.position = dest.transform.position + new Vector3(0,13.0F,0);


			} 
		} else {
			isChoose = false;
			destArrow.SetActive (false);
		}

		destArrow.transform.localScale = new Vector3 (mapcam.orthographicSize/3F,mapcam.orthographicSize/3F,mapcam.orthographicSize/3F);
		currentPositionParent.transform.localScale =  new Vector3 (mapcam.orthographicSize/3F,mapcam.orthographicSize/3F,mapcam.orthographicSize/3F);

		if (dest.name == "ARCamera") {
			transform.position = cat.transform.position + new Vector3 (0, 150, 0);
			mapcam.orthographicSize = 120;
		}




		if (mapcam.enabled) {
			GameObject[] naviLine = GameObject.FindGameObjectsWithTag ("GridLine");
			for (int i = 0; i < naviLine.Length; i++) {
				naviLine [i].GetComponent<LineRenderer> ().startWidth = mapcam.orthographicSize / 30F;
				naviLine [i].GetComponent<LineRenderer> ().endWidth = mapcam.orthographicSize / 30F;
			
			}
		} else {
			GameObject[] naviLine = GameObject.FindGameObjectsWithTag ("GridLine");
			for (int i = 0; i < naviLine.Length; i++) {
				naviLine [i].GetComponent<LineRenderer> ().startWidth =1F;
				naviLine [i].GetComponent<LineRenderer> ().endWidth = 1F;

			}
		}
	}

	public void SetMain()
	{
		ARcam.enabled = true;
		mapcam.enabled = false;
		currentPosition.SetActive (false);
		neviMesh.SetActive (false);
		mapPlane.SetActive (false);
		mapUI.SetActive (false);
		NaviLineMat.color = new Color (100/255F,180/255F,255/255F,190/255F);
	}

	public void SetMapcam()
	{
		
		ARcam.enabled = false;
		mapcam.enabled = true;
		currentPosition.SetActive (true);
		mapUI.SetActive (true);
		neviMesh.SetActive (true);
		mapPlane.SetActive (false);
		NaviLineMat.color = new Color (50/255F,160/255F,255/255F,255/255F);
	}

	public void SetSee3D(bool b)
	{
		see3D = b;
	}

	public void ChooseDestination(string des)
	{
		if (dest.name != des) {
			catNavi.setDestination (des);
			dest = GameObject.Find (des);
		}
	}

	public void setIsChoose(bool b)
	{
		isChoose = b;
		GameObject[] naviLines = GameObject.FindGameObjectsWithTag ("NaviLine");
		for (int i = 0; i < naviLines.Length; i++) {
			naviLines [i].transform.localScale = new Vector3 (mapcam.orthographicSize/600F,0.1F,mapcam.orthographicSize/600F);
		}
	}

	public void setDest(string des)
	{
		dest = GameObject.Find (des);
	}
}
