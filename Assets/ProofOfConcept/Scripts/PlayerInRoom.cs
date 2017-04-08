using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRoom : MonoBehaviour {
	public GameObject underCamera;
	public float timeForPlayerToGetOutToZoomOut = 0.5f;
	float enterTimeCount;
	Vector3 initPos;
	void Start(){
		initPos = transform.position;
		//Debug.Log("init pos:"+ initPos);
	}

	// Use this for initialization
	void OnEnable () {
		enterTimeCount = 0;
		//Debug.Log("Player in room enable");
	}



//	void OnCollisionEnter(Collision col) {
//		
//		if(col.gameObject.name.Equals("WallFoward")){
//			Debug.Log(col.gameObject.name+" enter");
//			enterTimeCount = 0;
//		}
//	}
//
//	void OnCollisionStay(Collision col){
//		
//		if(col.gameObject.name.Equals("WallFoward")){
//			Debug.Log(col.gameObject.name+" stay");
//			enterTimeCount += Time.deltaTime;
//			if(enterTimeCount > timeForPlayerToGetOutToZoomOut){
//				enterTimeCount = 0;
//				Debug.Log("in room -> zoom out");
//				transform.position = initPos;
//				underCamera.transform.parent.SendMessage("InRoomChangeToZoomOut");
//
//			}
//		}
//	}
//
//	void OnCollisionExit(Collision col){
//		
//		if(col.gameObject.name.Equals("WallFoward")){
//			Debug.Log(col.gameObject.name+" exit");
//			enterTimeCount = 0;
//		}
//	}

	private void OnCollisionStay(Collision hit)
	{
		if(gameObject.name.Equals("PlayerInRoom") && hit.collider.name.Equals("WallFoward")){
            Debug.Log("doin it");
			enterTimeCount += Time.deltaTime;
			if(enterTimeCount > 0.5f){
				enterTimeCount = 0;
				Debug.Log("in room -> zoom out");
				transform.position = initPos;
				underCamera.transform.parent.SendMessage("InRoomChangeToZoomOut");
			}
		}
		//body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
	}
}
