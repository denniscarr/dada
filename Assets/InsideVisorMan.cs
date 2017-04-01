using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.FirstPerson;

public class InsideVisorMan : MonoBehaviour {

    [SerializeField] GameObject freeLookCam;
    [SerializeField] Camera freeLookCamCamera;
    [SerializeField] GameObject controlledObject;
    [SerializeField] Camera myCam;
    [SerializeField] GameObject spotlight;

    [SerializeField] float controlledObjectHeight = 0.5f;
    [SerializeField] float controlledObjectMoveSpeedSlow = 1f;
    [SerializeField] float controlledObjectRotateSpeedSlow = 1f;
    [SerializeField] float controlledObjectMoveSpeedFast = 2f;
    [SerializeField] float controlledObjectRotateSpeedFast = 2f;
    float controlledObjectCurrentMoveSpeed;
    float controlledObjectCurrentRotateSpeed;

    bool rotateMode = false;

    enum State {NormalMovement, ControllingObject, DroppingObject};
    State currentState = State.NormalMovement;


    void Start()
    {
        controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedSlow;
        controlledObjectCurrentRotateSpeed = controlledObjectRotateSpeedSlow;

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
                    spotlight.SetActive(true);   

                    if (controlledObject.GetComponent<StickToNextObject>() != null)
                    {
                        Destroy(controlledObject.GetComponent<StickToNextObject>());
                    }

                    currentState = State.ControllingObject;
                }
            }
        }

        else if (currentState == State.ControllingObject)
        {
            // Move spotlight to highlight controlled object.
            spotlight.transform.position = new Vector3(controlledObject.transform.position.x, spotlight.transform.position.y, controlledObject.transform.position.z);

            // Drop object and tell it to stick to stuff.
            if (Input.GetMouseButtonDown(0))
            {

                if (controlledObject.GetComponent<StickToNextObject>() == null)
                {
                    controlledObject.AddComponent<StickToNextObject>();
                }

                controlledObject.GetComponent<StickToNextObject>().doIt = true;

                controlledObject.GetComponent<Rigidbody>().useGravity = true;

                currentState = State.DroppingObject;
            }

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
            Vector3 newScale = controlledObject.transform.localScale;
            Quaternion newRotation = controlledObject.transform.rotation;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedFast;
                controlledObjectCurrentRotateSpeed = controlledObjectRotateSpeedFast;
            }

            else
            {
                controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedSlow;
                controlledObjectCurrentRotateSpeed = controlledObjectRotateSpeedSlow;
            }

            // Control the object. (Space = rotate)
            if (!Input.GetKey(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 modDirection = freeLookCamCamera.transform.forward;
                    modDirection.y = 0;
                    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }

                else if (Input.GetKey(KeyCode.S))
                {
                    Vector3 modDirection = freeLookCamCamera.transform.forward * -1;
                    modDirection.y = 0;
                    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 modDirection = freeLookCamCamera.transform.right;
                    modDirection.y = 0;
                    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    Vector3 modDirection = freeLookCamCamera.transform.right * -1;
                    modDirection.y = 0;
                    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }

                // Up/Down arrow keys move controlled object on Y axis.
                if (Input.GetKey(KeyCode.E))
                {
                    newPosition += Vector3.up * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }

                else if (Input.GetKey(KeyCode.Q))
                {
                    newPosition += Vector3.down * controlledObjectCurrentMoveSpeed * Time.deltaTime;
                }
            }

            // Rotate the object.
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    newRotation *= CamRelativeRotate(0, -1, 0);
                }

                else if (Input.GetKey(KeyCode.S))
                {
                    newRotation *= CamRelativeRotate(0, 1, 0);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    newRotation *= CamRelativeRotate(-1, 0, 0);
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    newRotation *= CamRelativeRotate(1, 0, 0);
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    newRotation *= CamRelativeRotate(0, 0, -1);
                }

                else if (Input.GetKey(KeyCode.E))
                {
                    newRotation *= CamRelativeRotate(0, 0, 1);
                }


                // Up/Down arrow keys move controlled object on Y axis.
                //if (Input.GetKey(KeyCode.UpArrow))
                //{
                //    newPosition += Vector3.up * carriedObjectMoveSpeed * Time.deltaTime;
                //}

                //else if (Input.GetKey(KeyCode.DownArrow))
                //{
                //    newPosition += Vector3.down * carriedObjectMoveSpeed * Time.deltaTime;
                //}
            }

            // Scaling object.
            if (Input.GetKey(KeyCode.Minus))
            {
                newScale *= 0.8f;
            }

            else if (Input.GetKey(KeyCode.Equals))
            {
                newScale *= 1.2f;
            }

            controlledObject.GetComponent<Rigidbody>().MovePosition(newPosition);
            controlledObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
            controlledObject.transform.localScale = newScale;
        }

        else if (currentState == State.DroppingObject)
        {
            // See if the object I just dropped has stuck to something, if it has then switch back to normal movement.
            if (controlledObject != null && controlledObject.GetComponent<StickToNextObject>() != null && controlledObject.GetComponent<StickToNextObject>().dropping == true)
            {
                if (controlledObject.GetComponent<StickToNextObject>().doIt == false)
                {
                    freeLookCam.SetActive(false);
                    freeLookCam.GetComponent<FreeLookCam>().SetTarget(null);
                    GetComponent<RigidbodyFirstPersonController>().enabled = true;
                    myCam.enabled = true;
                    controlledObject = null;
                    spotlight.SetActive(false);
                    currentState = State.NormalMovement;
                }
            }
        }
    }

    public Quaternion CamRelativeRotate(float rotateLeftRight, float rotateUpDown, float rotateForwardBack)
    {
        float sensitivity = controlledObjectCurrentRotateSpeed * Time.deltaTime;

        Camera cam = freeLookCamCamera;
        // Gets the world vector space for cameras up vector 
        Vector3 relativeUp = cam.transform.TransformDirection(Vector3.up);
        // Gets world vector for space cameras right vector
        Vector3 relativeRight = cam.transform.TransformDirection(Vector3.right);
        // Gets the world vector space for cameras forward vector 
        Vector3 relativeForward = cam.transform.TransformDirection(Vector3.forward);

        // Changes relativeUp vector from world to objects local space
        Vector3 objectRelativeUp = controlledObject.transform.InverseTransformDirection(relativeUp);
        // Changes relativeRight vector from world to object local space
        Vector3 objectRelaviveRight = controlledObject.transform.InverseTransformDirection(relativeRight);
        // Changes relativeForward vector from world to object local space
        Vector3 objectRelativeForward = controlledObject.transform.InverseTransformDirection(relativeForward);

        return Quaternion.AngleAxis(rotateLeftRight / gameObject.transform.localScale.x * sensitivity, objectRelativeUp)
            * Quaternion.AngleAxis(-rotateUpDown / gameObject.transform.localScale.x * sensitivity, objectRelaviveRight)
            * Quaternion.AngleAxis(-rotateForwardBack / gameObject.transform.localScale.x * sensitivity, objectRelativeForward);

    }

}
