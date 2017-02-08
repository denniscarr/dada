using UnityEngine;
using System.Collections;

public class CountThePossibilites : MonoBehaviour {

	int process = 1;
	int move = 0;
	int sum = 0;
	int times = 2000;
	int[] moveGroup;


	void Start () {
		
		moveGroup = new int[times];
		for (int i = 0; i < times; i++)
		{
			while(process < 24) {
				move++;
				process += Random.Range(1,6);

				switch(process){
					case 4:process = 16;break;
					case 9:process = 21;break;
					case 19:process = 7;break;
					case 24:process = 2;break;
					default:break;
				}

			}

			sum += move;
			moveGroup[i] = move;
			//Debug.Log("total move:"+moveGroup[i]);
			move = 0;
			process = 1;
		}

		float mean = sum / (float) times;
		Debug.Log("mean:" + mean);

	}
}
