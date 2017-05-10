using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;



public class NavigationController : MonoBehaviour
{

	public Transform realPosition;
	public GameObject user;
    public GameObject target;
    public Animation Child;
	public Transform camTrans;
	public Transform NavPreTrans;
	private UnityEngine.AI.NavMeshAgent navi;
	public UnityEngine.AI.NavMeshAgent naviPre;
	public GpsControl gpsControl;


    private int i, RestDest = 1;
	private int Movestate = 0;
	public string NameOfDest = "ARCamera";
	private Vector3 userFront;
	private GameObject naviLines;
	private GameObject[] naviLineArray;
	private GameObject[] naviLineArrayPre;
	public Text currentDest;	
	public GameObject naviLinePfs;

    public string[] course = new string[3];
    public GameObject survice;
    public OpenSpeakCanvas openService;
    public bool survice_continue = false;


    private Vector3 startPos;
	private Vector3 currPos;

	public bool isNavPre; 
	public bool isNaving;

    void Start()
    {
        i = 0;
		navi = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		user = GameObject.Find("ARCamera");
		target = user;
		navi.destination = target.transform.position;
		NameOfDest = "ARCamera";
		userFront = new Vector3 (0,0,0);
		isNavPre = false;
    }

    // Update is called once per framehit	
    void Update()
    {



		i++;
      	
		//AdjustPosition ();

		if (Vector3.Distance (NavPreTrans.position, target.transform.position) <= 0.3f)
			isNavPre = false;
		else if (Vector3.Distance(NavPreTrans.position,target.transform.position) > 0.3f && i%4 == 0) {
			currPos = new Vector3(NavPreTrans.position.x, NavPreTrans.position.y,NavPreTrans.position.z);

			if (isNavPre && currPos.x != startPos.x) {
				float k = 15	;
				for (int t = 1; t <= k; t++) {
					Instantiate (naviLinePfs, startPos * (1 - (t / k)) + currPos * (t / k), Quaternion.LookRotation(currPos-startPos));
				}

			}

			startPos = new Vector3 (currPos.x,currPos.y,currPos.z);
		}
     
		if (i % 10 == 0) 
		{
			

			float dis, disTarget;
           Vector3 goToFront = Vector3.ProjectOnPlane (camTrans.forward, new Vector3 (0, 1, 0));
                        //현재 카메라가 바라보는 방향 벡터의 평면에 대한 projection

                        goToFront.Scale (new Vector3 (1, 0, 1));
                        goToFront.Normalize();
                        goToFront.Scale (new Vector3 (2, 0, 2));
                        if (Vector3.Distance (user.transform.position + goToFront, userFront) > 0.1F) {
                            userFront = user.transform.position + goToFront;
                            if (NameOfDest != "ARCamera")
                                //userFront += goToFront;
                            userFront.y = 0;
                        }
          
            // 고양이의 목적지가 사용자인 경우
            if (NameOfDest == "ARCamera") {
				




				//userFront 유저가 바라보는 방향으로 scale의 배 만큼 떨어져 있는 Point


				const float tooLong = 15 , littleLong= 10 , littleShort= 6 , tooShort= 1;
				//거리 별 행동을 정하기 위한 단계 별 거리


				NavMeshHit hit;
				navi.destination = userFront;
				if (NavMesh.SamplePosition (userFront, out hit, 4.0F, NavMesh.AllAreas)) {
					userFront = hit.position;
					currentDest.text = "Hit";
					if (NavMesh.Raycast (transform.position, userFront, out hit, NavMesh.AllAreas)) {
						currentDest.text = "HitHit";
						navi.destination = userFront;
					}
				} else {
					currentDest.text = "noHit";
				
				}


				navi.stoppingDistance = 0.1F;
				//유저가 바라보는 곳 앞에 위치하기 위해 stoppingDistance를 작은 값으로 설정
				dis = Vector3.Distance (transform.position, userFront);
				//고양이와 유저정면의 거리
	

				if (dis > tooLong) {
					navi.speed = 100F;
					Child.Play ("Run");
				}//거리가 너무 먼 경우 빠른 속도로 뛰어온다
				else if (dis > littleLong) {
					navi.speed = 40F;
					Child.Play ("Run");
				}//거리가 적당히 먼 경우 적당히 빠른 속도로 뛰어온다
				else if (dis > littleShort) {
					navi.speed = 10F;
					Child.Play ("Run");
				} else if (dis > tooShort) {
					navi.speed = 4F;
					Child.Play ("Run");
				} else if (dis <= tooShort) {
					navi.ResetPath ();
					Child.Play ("Idle01");
                   
				}//목표에 도착시 대기 애니메이션 출력, 네비 Reset Path


				//고양이가 돌아서 가야 하는 경우를 대비
				if (navi.remainingDistance > tooLong)
					navi.speed = 100F;
				

			}
			// 고양이의 목적지가 정해져 있는 경우
			else {
				
				target=GameObject.Find (NameOfDest);

				const float tooLong = 2F , littleLong= 1.2F , littleShort= 0.8F , tooShort= 1.2F;

				disTarget = Vector3.Distance (transform.position, target.transform.position);
				navi.destination = target.transform.position;

				dis = Vector3.Distance (transform.position, userFront);


                if (disTarget < tooShort)
                {
                    Child.Play("Dance");
                    /*
					while ( naviLines = GameObject.FindWithTag ("NaviLine") ) {
						DestroyImmediate (naviLines);
					}
					*/
                    System.Console.WriteLine("arrive at dest");

                    if (survice_continue == false && RestDest == 1)
                    {
                        if(!survice.activeInHierarchy)
                        {
                            survice.SetActive(true);
                            openService.ChangeSpeakSentance(NameOfDest.Replace("도착", "") + "1");
                            openService.SetSpeakImage(NameOfDest.Replace("도착", "") + "1");
                        }

                    }
                    else if (survice_continue == false && RestDest > 1)
                    {
                        if (!survice.activeInHierarchy)
                        { 
                            survice.SetActive(true);
                            openService.ChangeSpeakSentance(NameOfDest.Replace("도착", "") + "1");
                            openService.SetSpeakImage(NameOfDest.Replace("도착", "") + "1");
                        }
                    }

                    else if (survice_continue == true && RestDest > 1)
                    {
                        survice.SetActive(false);
                        navi.ResetPath();
                        RestDest--;
                        setDestination(course[RestDest-1]);
                       
                    }
                 
                    
                }
                else if (dis > tooLong)
                {
                    navi.destination = userFront;
                    Child.Play("Run");
                    navi.speed = 5F;
                }
                else if (dis > littleLong)
                {
                    navi.ResetPath();
                    Child.Play("Idle01");
                }
                else if (dis > littleShort)
                {
                    Child.Play("Run");
                    navi.speed = 1F;

                }
                else
                {
                    Child.Play("Run");
                    navi.speed = 3F;


                }

                

				/*
				bool isExistNear = false;
				naviLineArray = GameObject.FindGameObjectsWithTag ("NaviLine");
				for(int j=0; j<naviLineArray.GetLength(0) ; j++)
				{
					if (Vector3.Distance (naviLineArray [j].transform.position, transform.position) < 1.0f)
						isExistNear = true;
				}
				if(!isExistNear)
					Instantiate (naviLinePfs, transform.position+ new Vector3(0,0.3F,0), transform.rotation);
				*/

			


			}


		}
			
    }


    public void setRestDest(int a)
    {
        RestDest = a;

    }

    public void setcourse1(string destName1)
    {
        course[0] = destName1;



    }
    public void setcourse2(string destName2)
    {

        course[1] = destName2;



    }
    public void setcourse3(string destName3)
    {

        course[2] = destName3;

        this.setDestination(course[2]);


    }




    public void is_survice_continue()
    {
        survice_continue = true;
    }

    public void setDestination(string destName)
	{
        survice_continue = false;
        naviPre.ResetPath();
		isNavPre = true;
		isNaving = true;
		while ( naviLines = GameObject.FindWithTag ("NaviLine") ) {
			DestroyImmediate (naviLines);
		}

		Vector3 goToFront = Vector3.ProjectOnPlane (camTrans.forward, new Vector3 (0, 1, 0));
		goToFront.Scale (new Vector3(10,0,10));

		NameOfDest = destName;

		if (destName == "ARCamera") {
			navi.destination = user.transform.position + goToFront;
			isNavPre = false;
			isNaving = false;
		}
		if (target != user && target.name != destName ) 
			target.SetActive (false);


		target = GameObject.Find(destName);
		currentDest.text = "목적지: "+destName;


		NavPreTrans.position = gpsControl.realPosition.position;


		naviPre.destination = target.transform.position;

		startPos = new Vector3(NavPreTrans.position.x, NavPreTrans.position.y,NavPreTrans.position.z);
	}

	private void AdjustPosition()
	{
		NavMeshHit hit;
		if (!NavMesh.SamplePosition (realPosition.position, out hit, 0.3F, NavMesh.AllAreas)) {
			if (NavMesh.SamplePosition (realPosition.position, out hit, 3.0F, NavMesh.AllAreas)) {
				Vector3 offset = realPosition.position - hit.position;
				gpsControl.adjX -= offset.x;
				gpsControl.adjZ -= offset.z;
			}
		}

	}
}
