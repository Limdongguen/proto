using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSpeakCanvas : MonoBehaviour {
	
    public float delay = 0.1f;
    public string fullText;
    private string currentText = "";
    private string location;

    //public GameObject speak_Canvas;

	// Use this for initialization
	void Start () {
        //StartCoroutine(ShowText());
        //ChangeSpeakSentance("디도");
    }

    IEnumerator ShowText()
    {
        for(int i=0; i<fullText.Length; i++)
        {
            currentText = fullText.Substring(0,i);
            this.GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    public void ChangeSpeakSentance(string location)
    {
        var temp1 = GameObject.Find("/main_Canvas/speak_Canvas/talk_window_Panel/building_title_Text").GetComponent<Text>();
        if(location.StartsWith("디도"))
        {
            temp1.text = "삼성 학술 정보관";
        }
        else if(location.StartsWith("학관"))
        {
            temp1.text = "학생회관";
        }
        else if(location.StartsWith("1공"))
        {
            temp1.text = "제1공학관";
        }
        else if(location.StartsWith("2공"))
        {
            temp1.text = "제2공학관";
        }
        else
        {
            temp1.text = location;
        }
        
        var temp2 = GameObject.Find("/main_Canvas/speak_Canvas/talk_window_Panel/building_text_list/"+location).GetComponent<GUIText>();
        this.fullText = temp2.text;
        StartCoroutine(ShowText());
    }

    public void ChangeSpeakSentance()
    {
        var temp2 = GameObject.Find("/main_Canvas/speak_Canvas/talk_window_Panel/building_text_list/" + location).GetComponent<GUIText>();
        this.fullText = temp2.text;
        StartCoroutine(ShowText());
    }

    public void SetSpeakImage(string location) //첫 목적지 도착시 호출
    {
        int num = int.Parse(location[location.Length - 1].ToString());
        var temp1 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/building_image_list/" + location);
        var temp2 = GameObject.Find("/main_Canvas/navi_Canvas");
        temp1.SetActive(true);
        if (!temp2)
        { 
            //temp2.SetActive(false);
        }

        this.location = location;
    }

    public void SetSpeakImage(int l_or_r) //화살표 클릭시 호출
    {
        var temp1 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/building_image_list/" + location);
        temp1.SetActive(false);

        int num = int.Parse(location[location.Length - 1].ToString());
        if (l_or_r == 1)
        {
            location = location.Remove(location.Length-1,1) + (++num).ToString();
        }
        else if (l_or_r == -1)
        {
            location = location.Remove(location.Length - 1, 1) + (--num).ToString();
        }

        temp1 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/building_image_list/" + location);
        temp1.SetActive(true);

        if (num == 1)
        {
            var temp2 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/left_arrow");
            temp2.SetActive(false);
        }
        else if (num > 1 && num < 3)
        {
            var temp2 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/left_arrow");
            temp2.SetActive(true);
            temp2 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/right_arrow");
            temp2.SetActive(true);
        }
        else
        {
            var temp2 = GameObject.Find("/main_Canvas/speak_Canvas/building_image_Panel/right_arrow");
            temp2.SetActive(false);
        }
        ChangeSpeakSentance();
    }

}
