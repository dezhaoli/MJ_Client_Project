﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
public class ZhuMaScript : MonoBehaviour {

	public GameObject bottomContaner;
	public GameObject topContaner;
	public GameObject leftContaner;
	public GameObject rightContaner;

	public Image imgL;
	public Image imgB;
	public Image imgR;
	public Image imgT;

	public GameObject mapaiContaner;//中间显示码牌的容器
	public GameObject mapaiBg;//中间显示码牌的背景框


	private List<int> validMas;
	private List<string> mapaiList;
	private string uuid;

	private GamingData _data;

	public void init(GamingData data){
		_data = data;
	}
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void arrageMas(string allMas,List<int> validMasParms){
		
		validMas = validMasParms;
		if (allMas == null) {
			return;
		}
		uuid = allMas.Split (new char[1]{ ':' })[0];
		string[] paiArray = allMas.Split (new char[1]{ ':' });
		mapaiList = new List<string> (paiArray);
		mapaiList.RemoveAt (0);
	
		dispalyInitMas ();
		Invoke ("doArrage", 2.5f);


	}

	private bool checkIsVaild(int ma){
		if (validMas != null && validMas.Count > 0) {
			for (int i = 0; i < validMas.Count; i++) {
			
				if (ma == validMas [i]) {
					return true;
 				}
			}
		}
		return false;
	}


	private void dispalyInitMas(){
        if (mapaiList == null || mapaiList.Count == 0)
        {
            mapaiBg.SetActive(false);
            return;
        }
        float leftMargin = -(mapaiList.Count * 70f)/2+35f;
        for (int i = 0; i < mapaiList.Count; i++) {
			GameObject itemTemp =  Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_B")) as GameObject;
            int cardPoint = int.Parse(mapaiList[i]);
            itemTemp.GetComponent<PutoutCardView>().setPoint(cardPoint);
            itemTemp.transform.parent = mapaiContaner.transform;
			itemTemp.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			itemTemp.transform.localPosition = new Vector3 (leftMargin +i* 70f, 0, 0);
		}
	}


	private void doArrage(){
		int referIndex = _data.toAvatarIndex (int.Parse(uuid));
		int startPositionB = 0;
		int startPositionT = 0;
		int startPositionL = 0;
		int startPositionR = 0;
		mapaiBg.SetActive (false);

		for (int i = 0; i < mapaiList.Count; i++) {
			int cardPoint =int.Parse(mapaiList [i]);
			int positionIndex = (cardPoint + 1) % 9;
			Direction dir = Direction.B;
			if (cardPoint != 31) {
				switch(positionIndex){
				case 1:
				case 5:
				case 0:

					dir = _data.toGameDir (referIndex);	
					break;
				case 2:
				case 6:
					if ((referIndex + 1) == 4) {
						dir = _data.toGameDir (0);	
					} else {
						dir = _data.toGameDir (referIndex + 1);	
					}

					break;
				case 4:
				case 8:
					if ((referIndex - 1) < 0) {
						dir = _data.toGameDir (3);	
					} else {
						dir = _data.toGameDir (referIndex - 1);	
					}
					break;
				case 3:
				case 7:
					if ((referIndex + 2) == 4) {
						dir = _data.toGameDir (0);	
					} else if ((referIndex + 2) > 4) {
						dir = _data.toGameDir (1);	
					} else if ((referIndex + 2) < 4) {
						dir = _data.toGameDir (referIndex + 2);	
					}
					break;	
				}
			} else {
				dir = _data.toGameDir (referIndex);
			}



			GameObject itemTemp;


			switch (dir) {
			case Direction.B:
				if (checkIsVaild (cardPoint)) {
					imgB.transform.gameObject.SetActive (true);
				}
				itemTemp = Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_B")) as GameObject;
				itemTemp.GetComponent<PutoutCardView> ().setPoint (cardPoint);
				itemTemp.transform.parent = bottomContaner.transform;
				itemTemp.transform.localScale = Vector3.one;
				itemTemp.transform.localPosition = new Vector3 (-149f+startPositionB  * 60f,0f, 0);
				startPositionB += 1;
				break;
			case Direction.L:
				if (checkIsVaild (cardPoint)) {
					imgL.transform.gameObject.SetActive (true);
				}
				itemTemp = Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_L")) as GameObject;
				itemTemp.GetComponent<PutoutCardView> ().setPoint (cardPoint,Direction.L);
				itemTemp.transform.parent = leftContaner.transform;
				itemTemp.transform.localScale = new Vector3(2.0f,2.0f,1.0f);
				itemTemp.transform.localPosition = new Vector3 (0, 140f-startPositionL*50f, 0);

				startPositionL += 1;
				break;
			case Direction.R:
				if (checkIsVaild (cardPoint)) {
					imgR.transform.gameObject.SetActive (true);
				}
				itemTemp = Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_R")) as GameObject;
				itemTemp.GetComponent<PutoutCardView> ().setPoint (cardPoint,Direction.L);
				itemTemp.transform.parent = rightContaner.transform;
				itemTemp.transform.localScale = new Vector3(2.0f,2.0f,1.0f);
				itemTemp.transform.localPosition = new Vector3 (0f, -140f+startPositionR*50f, 0);
				startPositionR += 1;
				itemTemp.transform.SetSiblingIndex (0);
				break;
			case Direction.T:
				if (checkIsVaild (cardPoint)) {
					imgT.transform.gameObject.SetActive (true);
				}
				itemTemp = Instantiate (Resources.Load ("Prefab/PengGangCard/PengGangCard_B")) as GameObject;
				itemTemp.GetComponent<PutoutCardView> ().setPoint (cardPoint);
				itemTemp.transform.parent = topContaner.transform;
				itemTemp.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
				itemTemp.transform.localPosition = new Vector3 (149f-startPositionT * 60f, 0, 0);

				startPositionT += 1;
				break;
			} 


		}
	}








}
