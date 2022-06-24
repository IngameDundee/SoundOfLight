using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class FingerTipsObjectsManager : MonoBehaviour
{
    //Reference to Realtime to use to instantiate brush strokes
    [SerializeField]
    private Realtime _realtime;

    //Prefab to instantiate
    [SerializeField]
    private GameObject fingerTipsObjectsPrefab;

    [SerializeField]
    private GameObject fingerTipsObjectsNoSoundPrefab;

    [SerializeField]
    private GameObject handToTrack;

    [SerializeField]
    private GameObject thumbBoneToTrack;

    [SerializeField]
    private GameObject indexBoneToTrack;

    [SerializeField]
    private GameObject middleBoneToTrack;

    [SerializeField]
    private GameObject ringBoneToTrack;

    [SerializeField]
    private GameObject pinkyBoneToTrack;

    [SerializeField] float positionOffset = 0.1f;

    //Oculus types
    private OVRHand[] ovrHands;
    
    private GameObject localFingerTipsObjects;

    bool isLRingPinching;

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

        // Index finger 
        isLRingPinching = ovrHands[0].GetFingerIsPinching(OVRHand.HandFinger.Ring);

        if (isLRingPinching)
        {
            if (!localFingerTipsObjects)
            {
                localFingerTipsObjects = Realtime.Instantiate(fingerTipsObjectsNoSoundPrefab.name,
                    position: new Vector3(thumbBoneToTrack.transform.position.x,
                    thumbBoneToTrack.transform.position.y,
                    thumbBoneToTrack.transform.position.z),
                    rotation: thumbBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);

                localFingerTipsObjects = Realtime.Instantiate(fingerTipsObjectsPrefab.name,
                    position: new Vector3(indexBoneToTrack.transform.position.x,
                    indexBoneToTrack.transform.position.y,
                    indexBoneToTrack.transform.position.z),
                    rotation: indexBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);

                localFingerTipsObjects = Realtime.Instantiate(fingerTipsObjectsNoSoundPrefab.name,
                    position: new Vector3(middleBoneToTrack.transform.position.x,
                    middleBoneToTrack.transform.position.y,
                    middleBoneToTrack.transform.position.z),
                    rotation: middleBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);

                localFingerTipsObjects = Realtime.Instantiate(fingerTipsObjectsNoSoundPrefab.name,
                    position: new Vector3(ringBoneToTrack.transform.position.x,
                    ringBoneToTrack.transform.position.y,
                    ringBoneToTrack.transform.position.z),
                    rotation: ringBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);

                localFingerTipsObjects = Realtime.Instantiate(fingerTipsObjectsNoSoundPrefab.name,
                    position: new Vector3(pinkyBoneToTrack.transform.position.x,
                    pinkyBoneToTrack.transform.position.y,
                    pinkyBoneToTrack.transform.position.z),
                    rotation: pinkyBoneToTrack.transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);

            }
           


        }
       

    }
}
