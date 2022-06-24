using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractionSound : MonoBehaviour
{
    private OVRHand[] ovrHands;

    private bool audioIsPlaying = false;

    void Start()
    {
        ovrHands = new OVRHand[]
        {
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRCustomHandPrefab_L").GetComponent<OVRHand>(),
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRCustomHandPrefab_R").GetComponent<OVRHand>()
        };
    }


    void Update()
    {
        PlayRIndexSound();
    }

    // Right Hand
    void PlayRIndexSound()
    {

        if (ovrHands[1].GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            if (audioIsPlaying == false)
            {
                AkSoundEngine.PostEvent("R_Index", gameObject);
                audioIsPlaying = true;
            }  
        }
        else
        {
            audioIsPlaying = false;
        }

    }
}
