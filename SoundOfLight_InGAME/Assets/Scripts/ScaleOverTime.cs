using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{

    Vector3 sphereOriginalScale;
    Vector3 sphereCurrentScale;
    Vector3 sphereOriginalPosition;
    Vector3 sphereCurrentPosition;

    [SerializeField] float scaleLimit = 5;
    [SerializeField] float positionLimit = 5;


    // upper limits!
    [SerializeField] float scaleOverTime = 1f;
    Vector3 scaleOverTimeVector;
    [SerializeField] float translateOverTime = 1f;
    Vector3 translateOverTimeVector;

    void Start()
    {
        sphereOriginalScale = transform.localScale;
        sphereOriginalPosition = transform.localPosition;

        scaleOverTimeVector = new Vector3(scaleOverTime / 72f, scaleOverTime / 72f, scaleOverTime / 72f);
        translateOverTimeVector = new Vector3(translateOverTime / 72f, translateOverTime / 72f, translateOverTime / 72f);
    }

    void Update()
    {
        if (transform.localScale.magnitude < scaleLimit * sphereOriginalScale.magnitude)
        {
            transform.localScale += scaleOverTimeVector;
        }

        if (transform.localPosition.magnitude < positionLimit)
        {
            transform.localPosition += translateOverTimeVector;
        }
    }
}
