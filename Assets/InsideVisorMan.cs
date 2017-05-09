using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.FirstPerson;

public class InsideVisorMan : MonoBehaviour
{

    [SerializeField]
    GameObject objectCamGameObject;
    [SerializeField]
    Camera objectCamera;
    [SerializeField]
    GameObject controlledObject;
    [SerializeField]
    Camera myCam;
    [SerializeField]
    GameObject spotlight;

    [SerializeField]
    float controlledObjectHeight = 0.5f;
    [SerializeField]
    float controlledObjectMoveSpeedSlow = 1f;
    [SerializeField]
    float controlledObjectMoveSpeedFast = 2f;
    [SerializeField]
    float controlledObjectRotateSpeed = 2f;

    float controlledObjectCurrentMoveSpeed;
    float controlledObjectCurrentRotateSpeed;

    bool rotateMode = false;

    enum State { FirstPerson, FurnitureView, ControllingObject, DroppingObject };
    State currentState = State.FurnitureView;


    void Start()
    {
        controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedSlow;
        objectCamera = objectCamGameObject.transform.FindChild("Pivot").GetComponentInChildren<Camera>();
    }


    void Update()
    {
        if (currentState == State.FirstPerson)
        {
            //// Switch to furniture view.
            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    objectCamGameObject.SetActive(true);
            //    objectCamGameObject.GetComponent<FreeLookCam>().SetTarget(transform);
            //    GetComponent<RigidbodyFirstPersonController>().enabled = false;
            //    myCam.enabled = false;
            //    Cursor.lockState = CursorLockMode.None;
            //    currentState = State.FurnitureView;
            //}
        }

        else if (currentState == State.FurnitureView)
        {
            //// Switch to first person view.
            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    objectCamGameObject.SetActive(false);
            //    objectCamGameObject.GetComponent<FreeLookCam>().SetTarget(null);
            //    GetComponent<RigidbodyFirstPersonController>().enabled = true;
            //    myCam.enabled = true;
            //    controlledObject = null;
            //    spotlight.SetActive(false);
            //    currentState = State.FirstPerson;
            //}

            //// See if I clicked on a thingy.
            //RaycastHit hit;
            //if (Input.GetMouseButtonDown(0) && Physics.Raycast(myCam.ScreenPointToRay(Input.mousePosition), out hit, 10f))
            //{
            //    // See if the thingy was interactable.
            //    if (hit.collider.gameObject.GetComponentInChildren<InteractionSettings>() != null)
            //    {
            //        controlledObject = hit.collider.gameObject;
            //        controlledObject.GetComponent<Rigidbody>().useGravity = false;
            //        controlledObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            //        objectCamGameObject.GetComponent<FreeLookCam>().SetTarget(hit.collider.gameObject.transform);
            //        spotlight.SetActive(true);

            //        if (controlledObject.GetComponent<StickToNextObject>() != null)
            //        {
            //            Destroy(controlledObject.GetComponent<StickToNextObject>());
            //        }

            //        currentState = State.ControllingObject;
            //    }
            //}
        }

        else if (currentState == State.ControllingObject)
        {
            //// Reset camera movement.
            //objectCamGameObject.GetComponent<FreeLookCam>().m_TurnSpeed = 1.5f;

            //// Move spotlight to highlight controlled object.
            //spotlight.transform.position = new Vector3(controlledObject.transform.position.x, spotlight.transform.position.y, controlledObject.transform.position.z);

            //// Drop object and tell it to stick to stuff.
            //if (Input.GetMouseButtonDown(0))
            //{

            //    if (controlledObject.GetComponent<StickToNextObject>() == null)
            //    {
            //        controlledObject.AddComponent<StickToNextObject>();
            //    }

            //    controlledObject.GetComponent<Rigidbody>().useGravity = true;

            //    currentState = State.DroppingObject;
            //}

            //// See if the controlled object has for some reason stopped existing or whatever.
            //if (controlledObject == null)
            //{
            //    currentState = State.FurnitureView;
            //    return;
            //}

            //// Set velocity to zero.
            //controlledObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //controlledObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //Vector3 newPosition = controlledObject.transform.position;
            //Vector3 newScale = controlledObject.transform.localScale;
            //Quaternion newRotation = controlledObject.transform.rotation;

            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedSlow;
            //}

            //else
            //{
            //    controlledObjectCurrentMoveSpeed = controlledObjectMoveSpeedFast;
            //}


            ///* CONTROLLING OBJECT */

            //if (Input.GetKey(KeyCode.W))
            //{
            //    Vector3 modDirection = objectCamera.transform.forward;
            //    modDirection.y = 0;
            //    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}

            //else if (Input.GetKey(KeyCode.S))
            //{
            //    Vector3 modDirection = objectCamera.transform.forward * -1;
            //    modDirection.y = 0;
            //    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}

            //if (Input.GetKey(KeyCode.D))
            //{
            //    Vector3 modDirection = objectCamera.transform.right;
            //    modDirection.y = 0;
            //    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}

            //else if (Input.GetKey(KeyCode.A))
            //{
            //    Vector3 modDirection = objectCamera.transform.right * -1;
            //    modDirection.y = 0;
            //    newPosition += modDirection * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}

            //// Up/Down arrow keys move controlled object on Y axis.
            //if (Input.GetKey(KeyCode.E))
            //{
            //    newPosition += Vector3.up * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}

            //else if (Input.GetKey(KeyCode.Q))
            //{
            //    newPosition += Vector3.down * controlledObjectCurrentMoveSpeed * Time.deltaTime;
            //}


            //if (!Input.GetMouseButton(1))
            //{
            //    // Scaling object.
            //    if (Input.mouseScrollDelta.y < 0)
            //    {
            //        newScale *= 0.8f;
            //    }

            //    else if (Input.mouseScrollDelta.y > 0)
            //    {
            //        newScale *= 1.2f;
            //    }
            //}

            //// Rotate the object.
            //else
            //{
            //    // Make sure camera does not move.
            //    objectCamGameObject.GetComponent<FreeLookCam>().m_TurnSpeed = 0f;

            //    // Rotate it according to mouse.
            //    newRotation *= CamRelativeRotate(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), -Input.mouseScrollDelta.y);
            //}

            //controlledObject.GetComponent<Rigidbody>().MovePosition(newPosition);
            //controlledObject.GetComponent<Rigidbody>().MoveRotation(newRotation);
            //controlledObject.transform.localScale = newScale;
        }

        else if (currentState == State.DroppingObject)
        {
            //// See if the object I just dropped has stuck to something, if it has then switch back to normal movement.
            //if (controlledObject != null && controlledObject.GetComponent<StickToNextObject>() != null)
            //{
            //    if (controlledObject.GetComponent<StickToNextObject>().doIt == false)
            //    {
            //        objectCamGameObject.GetComponent<FreeLookCam>().SetTarget(null);
            //        controlledObject = null;
            //        spotlight.SetActive(false);
            //        currentState = State.FurnitureView;
            //    }
            //}
        }
    }

    public Quaternion CamRelativeRotate(float rotateLeftRight, float rotateUpDown, float rotateForwardBack)
    {
        float sensitivity = controlledObjectRotateSpeed * Time.deltaTime;

        Camera cam = objectCamera;
        // Get the world vector space for camera's up vector 
        Vector3 relativeUp = cam.transform.TransformDirection(Vector3.up);
        // Get world vector for space camera's right vector
        Vector3 relativeRight = cam.transform.TransformDirection(Vector3.right);
        // Get the world vector space for camera's forward vector 
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
