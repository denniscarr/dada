using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.FirstPerson;

public class InsideVisorMan : MonoBehaviour {

    [SerializeField] GameObject freeLookCam;
    Camera freeLookCamCamera;
    [SerializeField] GameObject controlledObject;
    [SerializeField] Camera myCam;

    [SerializeField] float carriedObjHeight = 0.5f;
    [SerializeField] float carriedObjectMoveSpeed = 1f;
    [SerializeField] float carriedObjectRotateSpeed = 1f;

    enum State {NormalMovement, ControllingObject};
    State currentState = State.NormalMovement;


    void Start()
    {
        freeLookCamCamera = freeLookCam.transform.FindChild("Pivot").GetComponentInChildren<Camera>();
    }

	
	void Update ()
    {
		if (currentState == State.NormalMovement)
        {
            // See if I clicked on a thingy.
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(myCam.ScreenPointToRay(Input.mousePosition), out hit, 10f))
            {
                // See if the thingy was interactable.
                if (hit.collider.gameObject.GetComponentInChildren<InteractionSettings>() != null)
                {
                    controlledObject = hit.collider.gameObject;
                    controlledObject.GetComponent<Rigidbody>().useGravity = false;
                    freeLookCam.SetActive(true);
                    freeLookCam.GetComponent<FreeLookCam>().SetTarget(hit.collider.gameObject.transform);
                    GetComponent<RigidbodyFirstPersonController>().enabled = false;
                    myCam.enabled = false;
                    currentState = State.ControllingObject;
                }
            }
        }

        else if (currentState == State.ControllingObject)
        {
            // See if the controlled object has for some reason stopped existing or whatever.
            if (controlledObject == null)
            {
                currentState = State.NormalMovement;
                return;
            }

            // Set velocity to zero.
            controlledObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            controlledObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Vector3 newPosition = controlledObject.transform.position;
            Quaternion newRotation = controlledObject.transform.rotation;

            // Control the object.
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 modDirection = freeLookCamCamera.transform.forward;
                modDirection.y = 0;
                newPosition += modDirection * carriedObjectMoveSpeed * Time.deltaTime;
            }

            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 modDirection = freeLookCamCamera.transform.forward * -1;
                modDirection.y = 0;
                newPosition += modDirection * carriedObjectMoveSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 modDirection = freeLookCamCamera.transform.right;
                modDirection.y = 0;
                newPosition += modDirection * carriedObjectMoveSpeed * Time.deltaTime;
            }

            else if (Input.GetKey(KeyCode.A))
            {
                Vector3 modDirection = freeLookCamCamera.transform.right * -1;
                modDirection.y = 0;
                newPosition += modDirection * carriedObjectMoveSpeed * Time.deltaTime;
            }

            // Up/Down arrow keys move controlled object on Y axis.
            if (Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += Vector3.up * carriedObjectMoveSpeed * Time.deltaTime;
            }

            else if (Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += Vector3.down * carriedObjectMoveSpeed * Time.deltaTime;
            }

            // Other buttons rotate object.
            if (Input.GetKey(KeyCode.Space))
            {
                Quaternion awayFromCam = Quaternion.LookRotation(transform.position - freeLookCamCamera.transform.position);
                //newRotation = Quaternion.RotateTowards(newRotation, awayFromCam, carriedObjectRotateSpeed * Time.deltaTime);
                newRotation = awayFromCam;
            }

            controlledObject.GetComponent<Rigidbody>().MovePosition(newPosition);
            controlledObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }
}
