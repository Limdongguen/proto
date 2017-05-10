using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDestinationText : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setDestination(NavigationController nc)
    {
        var temp1 = GameObject.Find("/main_Canvas/map_Canvas/destination_choose_Panel/destination_Text").GetComponent<Text>();
        string dest;
        dest = "도착" + temp1.text;
        var temp2 = GameObject.Find("/도착지/" + dest).GetComponent<GameObject>();
        temp2.SetActive(true);
        nc.setDestination(dest);
        System.Console.WriteLine(dest);
    }
}
