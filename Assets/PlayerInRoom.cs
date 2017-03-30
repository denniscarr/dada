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
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetMouseButtonDown(0)){
//			Debug.Log("in room click");
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
//			RaycastHit hit;
//			if (Physics.Raycast (ray, out hit)){
//				if(hit.collider.name.Equals("PlaneFront")){
//					Debug.Log("in room -> zoom out");
//					player.SetActive(true);
//					player.SendMessage("InRoomChangeToZoomOut");
//				}
//			}
//		}
	}

	//角色控制器组件在与具有Collider组件对象之间的碰撞
	void OnControllerColliderHit(ControllerColliderHit hit)
	{

		//判断碰撞的对象是否具备刚体组件
		GameObject hitObject = hit.collider.gameObject;
		Rigidbody rigidbody = hitObject.GetComponent<Rigidbody>();
		if(rigidbody != null && !rigidbody.isKinematic)
		{
			//地面也具备刚体组件，这里判断一下
			if(!hitObject.name.Equals("Terrain") )
			{
				rigidbody.AddForce(new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z) * 10);
			}
		}

	}

	void OnCollisionEnter(Collision col) {
		
		if(col.gameObject.name.Equals("WallFoward")){
			Debug.Log(col.gameObject.name+" enter");
			enterTimeCount = 0;
		}
	}

	void OnCollisionStay(Collision col){
		
		if(col.gameObject.name.Equals("WallFoward")){
			Debug.Log(col.gameObject.name+" stay");
			enterTimeCount += Time.deltaTime;
			if(enterTimeCount > timeForPlayerToGetOutToZoomOut){
				enterTimeCount = 0;
				Debug.Log("in room -> zoom out");
				transform.position = initPos;
				underCamera.transform.parent.SendMessage("InRoomChangeToZoomOut");

			}
		}
	}

	void OnCollisionExit(Collision col){
		
		if(col.gameObject.name.Equals("WallFoward")){
			Debug.Log(col.gameObject.name+" exit");
			enterTimeCount = 0;
		}
	}
}
