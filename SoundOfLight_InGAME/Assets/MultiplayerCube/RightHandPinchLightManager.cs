using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RightHandPinchLightManager : MonoBehaviour
{
    //Reference to Realtime to use to instantiate brush strokes
    [SerializeField]
    private Realtime _realtime;

    //Prefab to instantiate
    [SerializeField]
    private GameObject pinchLightPrefab;

    //Set which hand to track
    [SerializeField]
    private GameObject handToTrack;

    //Set which bone to track
    [SerializeField]
    private GameObject indexBoneToTrack;

    //Set position for pinchLightPrefab from the bone
    [SerializeField]
    float positionOffset = 0.1f;

    //Oculus types
    private OVRHand[] ovrHands;
    
    private GameObject localPinchLight;

    bool isIndexPinching;
    bool isLMiddlePinching;


    private void Awake()
    {
        ovrHands = new OVRHand[]
        {
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRCustomHandPrefab_L").GetComponent<OVRHand>(),
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRCustomHandPrefab_R").GetComponent<OVRHand>()
        };
    }

    void Update()
    {
        if (!_realtime.connected)
            return;

        // Do not execute anything below if the Oculus System Gesture is active
        if (ovrHands[1].IsSystemGestureInProgress)
            return;

        // Right Index finger 
        isIndexPinching = ovrHands[1].GetFingerIsPinching(OVRHand.HandFinger.Index);

        // Left middle finger
        isLMiddlePinching = ovrHands[0].GetFingerIsPinching(OVRHand.HandFinger.Middle);

        if (!isLMiddlePinching && isIndexPinching)
        {
            if (!localPinchLight)
            {
                localPinchLight = Realtime.Instantiate(pinchLightPrefab.name,
                    position: new Vector3(indexBoneToTrack.transform.position.x,
                    indexBoneToTrack.transform.position.y,
                    indexBoneToTrack.transform.position.z),
                    rotation: indexBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);
            }
            else
            {
                localPinchLight.transform.position = indexBoneToTrack.transform.position + (indexBoneToTrack.transform.up * positionOffset);
                localPinchLight.transform.rotation = indexBoneToTrack.transform.rotation;
                //lineDrawingObject.SetActive(false);
            }

        }
        else
        {
            
            if (localPinchLight)
            {
                Realtime.Destroy(localPinchLight);
            }
        }

    }

}
