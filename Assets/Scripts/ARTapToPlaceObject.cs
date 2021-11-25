using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject goalToPlace;
    public GameObject ballToPlace;
    public GameObject planeToPlace;
    public GameObject goalLine;
    public GameObject placementIndicator;

    public Vector3 ballPosition;
    private Vector3 goalPosition;
    private Vector3 planePosition;

    private bool isGoalPlaced = false;
    private bool isBallPlaced = false;
    private bool isPlanePlaced = false;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arRaycastManager;

    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycastManager = arOrigin.GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (!isPlanePlaced)
        {
            // save plane position to place goal and ball above it
            planePosition = placementPose.position;
            // set plane position
            planeToPlace.transform.position = planePosition;
            // mark plane as placed
            isPlanePlaced = true;
        }
        else if (!isGoalPlaced)
        {
            // rotate goal
            placementPose.rotation.y = -180;
            // place goal on the same level as plane
            placementPose.position.y = planePosition.y;
            // save goal position
            goalPosition = placementPose.position;
            // set goal rotation and position
            goalToPlace.transform.rotation = placementPose.rotation;
            goalToPlace.transform.position = goalPosition;
            // mark goal as placed
            isGoalPlaced = true;
            // add goal line
            // var goalLinePostion = goalPosition;
            // goalLinePostion.y += 0.25f;
            // goalLine.transform.position = goalLinePostion;
        }
        else if (!isBallPlaced)
        {
            // set ball position above plane
            placementPose.position.y = planePosition.y + 0.1f;
            // save ball position
            ballPosition = placementPose.position;
            // set ball position
            ballToPlace.transform.position = ballPosition;
            // mark ball as placed
            isBallPlaced = true;
        }
    }

    private void UpdatePlacementIndicator()
    {
        // hide placement indicator after placing objects
        if (isPlanePlaced && isGoalPlaced && isBallPlaced)
        {
            placementIndicator.SetActive(false);
            return;
        }

        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}

