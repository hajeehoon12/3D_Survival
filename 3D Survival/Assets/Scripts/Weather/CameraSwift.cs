using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwift : MonoBehaviour
{
	public Camera[] arrCam; //카메라 요소들을 추가한다.



	int nCamCount = 3;

	int nNowCam = 0;


	void Start()
	{

		
	
	}


	void Update()

	{

		if (Input.GetButtonUp("Fire3"))

		{

			++nNowCam;



			if (nNowCam >= nCamCount)

			{

				nNowCam = 0;

			}



			for (int i = 0; i < arrCam.Length; ++i)

			{

				arrCam[i].enabled = (i == nNowCam);

			}

		}

	}



}
