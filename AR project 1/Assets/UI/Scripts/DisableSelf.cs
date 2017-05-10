using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour {

    public float DisableTime = 2.0f;

    void Update()
    {
        StartCoroutine(disableSelf());     
    }

    IEnumerator disableSelf()
    {
        yield return new WaitForSeconds(DisableTime);
        GameObject thisObject = GameObject.Find(gameObject.name);
        thisObject.SetActive(false);
        //Debug.Log(gameObject.name);
        //Debug.Log(thisObject.activeSelf);
    }
}
