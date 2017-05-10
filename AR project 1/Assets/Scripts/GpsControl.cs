using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GpsControl : MonoBehaviour {



	public Transform child;
	public Text info;
	public float Height = 5.5F;
	public float speed = 4;
	public Text Height_text;
	public GameObject mapDirection;
	public float mulX, mulZ;
	public DaburuTools.Input.GyroControl gyroControl;
	public Transform realPosition;	// virtual trasnform to adjust position
	public NavigationController naviControl; // NavigationController to adjust position
	public float adjX,adjZ;
	private float lati, longi, timer;
	private float pivot_lati = 293712;
	private float pivot_long = 974494;
	private float offset_lati;
	private float offset_long;
	private int i;
	private Vector3 velocity = Vector3.zero;
	private Vector3 y_Rotate ;
    // Use this for initialization
    public int closestIndex = 0;


	public Text TextX, TextZ, TextR;

    void awake()
	{
		
	}


	IEnumerator  Start () {

		#if UNITY_STANDALONE || UNITY_EDITOR
		offset_lati=transform.position.z;
		offset_long=transform.position.x;
		#endif


		timer = 0;
		i = 0;
		// First, check if user has location service enabled

		// Start service before querying location
		Input.location.Start((float)5,(float)0.1);

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(0.5f);

			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}
		else
		{
			lati = Input.location.lastData.latitude;
			longi = Input.location.lastData.longitude;
			// Access granted and location value could be retrieved

		}
	
	}

	// Update is called once per frame
	void Update(){
		
		TextX.text = adjX.ToString ();
		if (TextX.text.Length > 4)
			TextX.text = TextX.text.Remove (3);
		TextZ.text = adjZ.ToString ();
		if (TextZ.text.Length > 4)
			TextZ.text = TextZ.text.Remove (3);
		TextR.text = gyroControl.rotOffset.ToString ();
		if (TextR.text.Length > 4)
			TextR.text = TextR.text.Remove (3);
		
		info.text = "X: " + adjX.ToString () + " Z: " + adjZ.ToString () + "RotationOffset: " + gyroControl.rotOffset.ToString();

        gps();


        Vector3 y_Rotate = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
		y_Rotate.Normalize ();
		mapDirection.transform.forward = y_Rotate;


		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			y_Rotate = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
			offset_long += y_Rotate.x*speed*Time.deltaTime;
			offset_lati += y_Rotate.z*speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			y_Rotate = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
			offset_long -= y_Rotate.x*speed*Time.deltaTime;
			offset_lati -= y_Rotate.z*speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			y_Rotate = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
			offset_lati += y_Rotate.x*speed*Time.deltaTime;
			offset_long -= y_Rotate.z*speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			y_Rotate = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
			offset_lati -= y_Rotate.x*speed*Time.deltaTime;
			offset_long += y_Rotate.z*speed*Time.deltaTime;
		}

		Vector3 final = new Vector3 (offset_long, 0, offset_lati);
		final.x += adjX;
		final.z += adjZ;
		realPosition.position = final;
		/*
		info.text = "Location:\n" + offset_lati.ToString () + "\n" + offset_long.ToString ()+"\n"
			+ transform.position.x.ToString() + ", " + transform.position.z.ToString();
		*/

		if (naviControl.isNaving ) {
			bool nearby = false;
			bool tooFar = false;
			float closest = 99999F;
            int TMPclosestIndex = 0;

			if (naviControl.naviLineArray.Length>0) {
				for (int i = 0; i < naviControl.naviLineArray.Length; i++) {
					if (Vector3.Distance (realPosition.position, naviControl.naviLineArray[i]) <= closest) {
						TMPclosestIndex = i;
						closest = Vector3.Distance (realPosition.position, naviControl.naviLineArray[i]);
					}
				}

                //if (Mathf.Abs((float)closestIndex-TMPclosestIndex)>4.0f)
                    closestIndex = TMPclosestIndex;

                Vector3 adjustPostion = naviControl.naviLineArray[closestIndex];
					adjustPostion.y = Height;
					transform.position = Vector3.SmoothDamp (transform.position,adjustPostion , ref velocity, 0.1F);

			}

		}
		else
			transform.position = Vector3.SmoothDamp (transform.position, realPosition.position+new Vector3(0,Height,0), ref velocity, 0.3F);

	}


	public void setHeight(float x)
	{
		Height += x;
		if (Height.ToString ().Length > 3) {
			Height_text.text = Height.ToString ().Remove (3) + " m";
		}
		else 
			Height_text.text = Height.ToString () + " m";
	}

	public void setOffset(string dir)
	{
		info.text = "X: " + adjX.ToString () + " Z: " + adjZ.ToString ();
		Vector3 userDir = Vector3.ProjectOnPlane (child.forward, new Vector3 (0, 1, 0));
		userDir.Normalize ();
		userDir.Scale (new Vector3(0.3F,0,0.3F));

		if (dir == "UP") {
			adjX += userDir.x;
			adjZ += userDir.z;
		}
		else if (dir == "DOWN") {
			adjX -= userDir.x;
			adjZ -= userDir.z;
		}
		else if (dir == "LEFT") {
			adjX -= userDir.z;
			adjZ += userDir.x;
		}
		else if (dir == "RIGHT") {
			adjX += userDir.z;
			adjZ -= userDir.x;
		}


	}

	public void setRotOffset(string dir)
	{
		if (dir == "RIGHT") {
			gyroControl.rotOffset += 1;
		} else if (dir == "LEFT") {
			gyroControl.rotOffset -= 1;
		}
	}

    public void gps()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            lati = Input.location.lastData.latitude;
            longi = Input.location.lastData.longitude;

            lati -= 37;
            lati *= 1000000;
            longi -= 126;
            longi *= 1000000;

            offset_lati = lati - pivot_lati;
            offset_long = longi - pivot_long;
            offset_lati /= (float)mulZ;
            offset_long /= (float)mulX;


        }

        Vector3 final = new Vector3(offset_long, 0, offset_lati);
        final.x += adjX;
        final.z += adjZ;
        realPosition.position = final;

    }
}
