using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
TeleportationManager is attached to the hand intended to have teleportation fnctionality
This is designed for use on the right hand Game Object (OVRCustomHandPrefab_R)
Controls are: - Ring Finger Pinch and hold to aim teleportation
    - Release Ring Finger Pinch to teleport to that location
    - Teleportation Cooldown input box on the GO is there to stop accidentally teleporting multiple times
*/

public class TeleportationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player_rig;
    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private float teleportCooldown = 3f;

    private bool is_aiming = false;
    private bool is_aiming_previous = false;
    private GameObject currentDestination;
    private float teleportCooldownTimer = 0f;

    [SerializeField]
    private GameObject handToTrack;

    private OVRHand ovrHand;

    private bool isRingFingerPinching;
    private bool isMiddleFingerPinching;

    // Curve Rendering
    private Vector3 curveTopPoint;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int vertexCount = 12;

    Quaternion handFacing;
    Quaternion localHandFacing;
    [SerializeField]
    private GameObject laserStart;

    void Start()
    {
        ovrHand = handToTrack.GetComponent<OVRHand>();
        currentDestination = Instantiate(destination, transform.position, Quaternion.identity);
    }

    void Update()
    {

        teleportCooldownTimer += Time.deltaTime;

        if (is_aiming)
        {
            CheckForDestination();
        }

        isRingFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Ring);
        isMiddleFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Middle);

        if (isRingFingerPinching)
        {
            lineRenderer.enabled = true;
            is_aiming = true;
            is_aiming_previous = true;
        }
        else
        {
            lineRenderer.enabled = false;
            is_aiming = false;
            currentDestination.SetActive(false);

            if (is_aiming != is_aiming_previous && teleportCooldownTimer > teleportCooldown)
            {
                Teleport();
                teleportCooldownTimer = 0f;
            }
            is_aiming_previous = false;
        }

    }

    private void CheckForDestination()
    {
        Vector3 midpoint;
        handFacing = laserStart.transform.rotation;
        localHandFacing = transform.localRotation;

        Ray ray = new Ray(transform.position, handFacing * (-Vector3.up + Vector3.right));

        RaycastHit hit;

        bool b = Physics.Raycast(ray, out hit, 5, 1 << 8);

        if (b)
        {
            currentDestination.transform.position = hit.point;
            currentDestination.SetActive(true);
            midpoint = GetMidPoint(transform.position, currentDestination.transform.position);
            DrawCurve(transform.position, midpoint, currentDestination.transform.position);
        }
        else
        {
            currentDestination.SetActive(false);
            midpoint = GetMidPoint(transform.position, (transform.rotation * (-Vector3.up + Vector3.right) * 3));
            DrawCurve(transform.position, midpoint, transform.position + (transform.rotation * (-Vector3.up + Vector3.right) * 3));
        }


    }

    public void DrawCurve(Vector3 point1, Vector3 midpoint, Vector3 point3)
    {
        curveTopPoint = new Vector3(midpoint.x, midpoint.y + 1f, midpoint.z);
        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1, curveTopPoint, ratio);
            var tangentLineVertex2 = Vector3.Lerp(curveTopPoint, point3, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    private Vector3 GetMidPoint(Vector3 startPoint, Vector3 endpoint)
    {
        Vector3 midpoint = new Vector3(
            (startPoint.x + endpoint.x) * 0.5f,
            (startPoint.y + endpoint.y) * 0.5f,
            (startPoint.z + endpoint.z) * 0.5f
            );

        return midpoint;
    }

    public void Teleport()
    {
        player_rig.transform.position = currentDestination.transform.position;
        currentDestination.SetActive(false);
    }




}
