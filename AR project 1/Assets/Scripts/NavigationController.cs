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

	private UnityEngine.AI.NavMeshAgent navi;
    public UnityEngine.AI.NavMeshAgent navi2;
    public UnityEngine.AI.NavMeshAgent navi3;
    public UnityEngine.AI.NavMeshAgent naviAR;
	public Transform naviARtrans;
    private Transform tempPosition;

	public GpsControl gpsControl;
    public Material NaviLineMat;
    public bool see3D;


    private int i, RestDest = 1;
	private int Movestate = 0;

	private Vector3 userFront;
	private GameObject naviLines;
    public Vector3[] naviLineArray;
	private GameObject[] naviLineArrayPre;
	public Text currentDest;	
	public GameObject naviLinePfs;

    
    public GameObject survice;
    public OpenSpeakCanvas openService;
    public bool survice_continue = false;
    GameObject[] prePaths ;


    private Vector3 startPos;
	private Vector3 currPos;

	public bool isNaving;

    /*
    ...........
     */
    public string NameOfDest = "ARCamera";
    private int currentNavi = 0;
    public string[] course = new string[3];
    private int currentIndex=0;
    public Text testText;
	public GameObject LinePfs;


    void Start()
    {
        i = 0;	
		navi = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		user = GameObject.Find("ARCamera");
		target = user;
		navi.destination = target.transform.position;
		NameOfDest = "ARCamera";
		userFront = new Vector3 (0,0,0);
    }

    // Update is called once per framehit	
    void Update()
    {

  
            naviARtrans.position = realPosition.transform.position;


            float dis, disTarget;
            
            if (Vector3.Distance (user.transform.position + goToFront(), userFront) > 0.1F) {
                userFront = user.transform.position + goToFront();
                 if (NameOfDest != "ARCamera")
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
					Vector3 velocity = Vector3.zero;
					navi.Warp(Vector3.SmoothDamp(transform.position,userFront,ref velocity,0.1F));
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

                if(survice_continue ==true)survice_continue = false;
                //고양이가 돌아서 가야 하는 경우를 대비
                if (navi.remainingDistance > tooLong)
					navi.speed = 100F;
				

			}
			// 고양이의 목적지가 정해져 있는 경우
		    else {
          
                int targetIndex= gpsControl.closestIndex + 10 ;
           
            if (targetIndex+10 >= naviLineArray.Length)
                {
                    Child.Play("Dance");
                    
                    
                }
			else if (Vector3.Distance(transform.position,naviLineArray[targetIndex]) > 0.1F){
                    Child.Play("Run");
					Vector3 velocity = Vector3.zero;
					navi.Warp(Vector3.SmoothDamp(transform.position,naviLineArray[targetIndex],ref velocity,0.1F));
                    transform.LookAt(naviLineArray[targetIndex+3]);
                    currentIndex = targetIndex;
                 
           	 	}
                else
                {
                    Child.Play("Idle01");
                }
            //testText.text = targetIndex.ToString() + "  " + naviLineArray[targetIndex].x.ToString();// + "  " + transform.position.x.ToString();
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

        //this.setDestination(course[2]);


    }




    public void is_survice_continue()
    {
        survice_continue = true;
        
    }

    public void setDestination(string destName)
	{
        DestroyAll("NaviLine");
		isNaving = true;

        Vector3 goToFront = Vector3.ProjectOnPlane (camTrans.forward, new Vector3 (0, 1, 0));
		goToFront.Scale (new Vector3(10,0,10));

		NameOfDest = destName;
        target = GameObject.Find(destName);

        if (destName == "ARCamera") {
			navi.destination = user.transform.position + goToFront;
			isNaving = false;
		}
        else DrawPathLine(FindClosestStart(), target.transform.position);   
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

    private void DrawPathLine(Vector3 start, Vector3 end)
    {
        naviARtrans.position = start;
        NavMeshPath path = new NavMeshPath();
        naviAR.CalculatePath(GameObject.Find(NameOfDest).transform.position, path);
        Vector3[] lines = path.corners;
        /*/
        tempPosition.position = GameObject.Find(course[1]).transform.position;
        navi2.transform.position = tempPosition.position;
        tempPosition.position = GameObject.Find(course[0]).transform.position;
        navi3.transform.position = tempPosition.position;

        NavMeshPath path2 = new NavMeshPath();
        NavMeshPath path3 = new NavMeshPath();
        navi2.CalculatePath(naviAR.transform.position, path2);
        navi3.CalculatePath(navi2.transform.position, path3);
        GameObject linePointer2 = Instantiate(LinePfs, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject linePointer3 = Instantiate(LinePfs, new Vector3(0, 0, 0), Quaternion.identity);
        LineRenderer lineRenderer2 = linePointer2.GetComponent<LineRenderer>();
        LineRenderer lineRenderer3 = linePointer3.GetComponent<LineRenderer>();


        /*/
        GameObject linePointer = Instantiate (LinePfs,new Vector3(0,0,0),Quaternion.identity);
		LineRenderer lineRenderer = linePointer.GetComponent<LineRenderer> ();


		lineRenderer.numPositions = lines.Length;
		lineRenderer.SetPositions (lines);

        int arrSize = 0;
        for (int i = 0; i < lines.Length - 1; i++)
        {
            float dist = Vector3.Distance(lines[i],lines[i+1])*3;
            for (int t = 1; t <= dist; t++)
            {
                arrSize++;
            }
        }
        naviLineArray = new Vector3[arrSize];
        int count = 0;
        for (int i = 0; i < lines.Length - 1; i++)
        {
			
            float dist = Vector3.Distance(lines[i], lines[i + 1])*3;

            for (int t = 1; t <= dist; t++)
            {
                naviLineArray[count] = lines[i] * (1 - (t / dist)) + lines[i + 1] * (t / dist);
                //Instantiate(naviLinePfs, lines[i] * (1 - (t / dist)) + lines[i + 1] * (t / dist), Quaternion.LookRotation(currPos - startPos));
                count++;
            }
        }
       
       
    }
    
    private Vector3 FindClosestStart()
    {
        prePaths = GameObject.FindGameObjectsWithTag("prePath");
        float closest = 99999F;
        int closestIndex = 0;


        for (int i = 0; i < prePaths.Length; i++)
        {
            if (Vector3.Distance(realPosition.position, prePaths[i].transform.position) <= closest)
            {
                closestIndex = i;
                closest = Vector3.Distance(realPosition.position, prePaths[i].transform.position);
            }
        }

        return prePaths[closestIndex].transform.position;
    }

    private void DestroyAll(string target)
    {
        
        while (naviLines = GameObject.FindWithTag(target))
        {
            DestroyImmediate(naviLines);
        }

		while (GameObject.FindWithTag("GridLine"))
		{
			DestroyImmediate(GameObject.FindWithTag("GridLine"));
		}
    }

    private Vector3 goToFront()
    {
        Vector3 goToFront = Vector3.ProjectOnPlane(camTrans.forward, new Vector3(0, 1, 0));
        //현재 카메라가 바라보는 방향 벡터의 평면에 대한 projection

        goToFront.Scale(new Vector3(1, 0, 1));
        goToFront.Normalize();
        goToFront.Scale(new Vector3(2, 0, 2));

        return goToFront;	
    }
}

