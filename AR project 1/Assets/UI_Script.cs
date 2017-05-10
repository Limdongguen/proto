using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour {

	public Text testText;
	public GameObject gridLinePfs;
	public int gridLineB;
	public Material lineMat;
	public 	int k = 300;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}


	public void DrawGridLine()
	{

		if (gridLineB == 1) {

			//GameObject gridLineArray;
			gridLineB = 0;

			lineMat.color = new Color (lineMat.color.r,lineMat.color.g,lineMat.color.b,0F);
			/*
			while(true)
			{
				gridLineArray = GameObject.FindGameObjectWithTag ("GridLine");
				if (gridLineArray != null)
					DestroyImmediate (gridLineArray);
				else {
					gridLineB = 0;
					testText.text = "Test Message: " + "break";
					break;
				}
			}
			testText.text = "Test Message: " + "exit";
			*/

		}
		else {
			gridLineB = 1;

			lineMat.color = new Color (lineMat.color.r,lineMat.color.g,lineMat.color.b,150/255F);
			/*
			testText.text = "Test Message: " + gridLineB.ToString ();
			GameObject gridLineTmp;
			LineRenderer gridLineTmpLR;
		
			//가로 격자선 생성
			for (int i = 0; i <= k; i++) {
			
				Vector3[] positions = new Vector3[2];
				positions [0] = new Vector3 (-500 + i * (1000F / k), 0.01F, -500);
				positions [1] = new Vector3 (-500 + i * (1000F / k), 0.01F, 500);
				gridLineTmp = Instantiate (gridLinePfs, new Vector3 (0, 0, 0), new Quaternion ());
				gridLineTmpLR = gridLineTmp.GetComponent<UnityEngine.LineRenderer> ();
				gridLineTmpLR.SetPositions (positions);
			}

			for (int i = 0; i <= k; i++) {

				Vector3[] positions = new Vector3[2];
				positions [0] = new Vector3 (500, 0.01F, -500 + i * (1000F / k));
				positions [1] = new Vector3 (-500, 0.01F, -500 + i * (1000F / k));
				gridLineTmp = Instantiate (gridLinePfs, new Vector3 (0, 0, 0), new Quaternion ());
				gridLineTmpLR = gridLineTmp.GetComponent<UnityEngine.LineRenderer> ();
				gridLineTmpLR.SetPositions (positions);
			}
			*/

		}
	}
		
}
