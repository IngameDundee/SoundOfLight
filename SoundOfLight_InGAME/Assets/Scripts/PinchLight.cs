using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchLight : MonoBehaviour
{
    OVRHand ovrHand;
    bool isIndexPinching;
    Vector3 sphereOriginalScale;
    Vector3 sphereCurrentScale;
    Vector3 sphereOriginalPosition;
    Vector3 sphereCurrentPosition;

    [SerializeField] float scaleLimit = 5;
    [SerializeField] float positionLimit = 5;


    // upper limits!

    [SerializeField] GameObject lightPinch;
    [SerializeField] float scaleOverTime = 1f;
    Vector3 scaleOverTimeVector;
    [SerializeField] float translateOverTime = 1f;
    Vector3 translateOverTimeVector;


    void Start()
    {
        ovrHand = gameObject.GetComponent<OVRHand>();
        
        sphereOriginalScale = lightPinch.transform.localScale;
        sphereOriginalPosition = lightPinch.transform.localPosition;

        scaleOverTimeVector = new Vector3(scaleOverTime/72f, scaleOverTime / 72f, scaleOverTime / 72f);
        translateOverTimeVector = new Vector3(translateOverTime / 72f, translateOverTime / 72f, translateOverTime / 72f);
        
    }

    void FixedUpdate()
    {
        isIndexPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        
        if (isIndexPinching)
        {
            lightPinch.SetActive(true);
            if(lightPinch.transform.localScale.magnitude < scaleLimit)
            {
                lightPinch.transform.localScale += scaleOverTimeVector;
            }

            if (lightPinch.transform.localPosition.magnitude < positionLimit)
            {
                lightPinch.transform.localPosition += translateOverTimeVector;
            }
            
        }
        else
        {
            lightPinch.transform.localScale = sphereOriginalScale;
            lightPinch.transform.localPosition = sphereOriginalPosition;
            lightPinch.SetActive(false);
            
        }
        
    }
}
