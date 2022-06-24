using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using Normal.Realtime;

/*
This script is based on Valem's tutorial video "How to detect a movement in VR" on Youtube - https://youtu.be/GRSOrkmasMM.
This script uses P RECOGNIZER - https://github.com/DaVikingCode/PDollar-Unity.
*/

public class HandMovementRecognizer : MonoBehaviour
{

    // Realtime
    [SerializeField]
    private Realtime _realtime;

    // Oculus Types
    private OVRHand[] ovrHands;
   
    public GameObject leftHandToTrack;
    public GameObject rightHandToTrack;
    public Transform movementSource;
    private bool isRIndexPinching = false;
    public bool isLMiddlePinching = false;

    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = false;
    public string newGestureName;

    public float recognitionThreshold = 0.8f;


    // Geseture training set
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent onRecognized;

    private List<Gesture> trainingSet = new List<Gesture>();
    public bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();

    private string defaultPath = Application.streamingAssetsPath;

    void Start()
    {
        // Read trained dataset from the /StreamingAssets/Gestures folder
        //string[] gestureFiles = Directory.GetFiles(defaultPath + string.Format("/Gestures/"), "*.xml");
        //foreach (var item in gestureFiles)
        //{
        //    trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        //}

        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/MyGesture/");
        foreach (TextAsset gestureXml in gesturesXml)
        {
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
        }
            
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

        // L Middle finger pinching
        isLMiddlePinching = ovrHands[0].GetFingerIsPinching(OVRHand.HandFinger.Middle);

        // R Index finger pinching
        isRIndexPinching = ovrHands[1].GetFingerIsPinching(OVRHand.HandFinger.Index);

        // Gesture recognition
        if (isLMiddlePinching)
        {
            // Start the movement
            if (!isMoving && isRIndexPinching)
            {
                StartMovement();
            }
            // End the movement
            else if (isMoving && !isRIndexPinching)
            {
                EndMovement();
            }
            // Update the movement
            else if (isMoving && isRIndexPinching)
            {

                UpdateMovement();
            }
        }

    }

    void StartMovement()
    {
        Debug.Log("Start Movement");
        isMoving = true;

        // reset position list when start
        positionsList.Clear();

        // Add the first position
        positionsList.Add(movementSource.position);

        
        // Instantiate the cube - cube prefab destoys after 3 sec 
        if (debugCubePrefab)
        {
            GameObject debugPrefab = Realtime.Instantiate(debugCubePrefab.name, movementSource.position, Quaternion.identity,
                ownedByClient: true,
                preventOwnershipTakeover: false,
                destroyWhenOwnerOrLastClientLeaves: true,
                useInstance: _realtime);
        }
        

    }

    void EndMovement()
    {
        Debug.Log("End Movement");
        isMoving = false;

        // Create the gesture from the position list
        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            // Point[] only needs X and Y value. So make it as Vector 2 value.
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0); // The third parameter is a stroke ID. We only have one stroke so that is why it's 0.
        }

        Gesture newGesture = new Gesture(pointArray);


        // Add a new gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            // Save trained gestures as a file
            //string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            string fileName = defaultPath + string.Format("/Gestures/{0}.xml", newGestureName);
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        // Recognise
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + result.Score);

            // When the recognition score exceeds the threshold 
            if (result.Score > recognitionThreshold)
            {
                onRecognized.Invoke(result.GestureClass);
            }
        }
    }

    void UpdateMovement()
    {
        Debug.Log("Update Movement");

        // Get the last position
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        // Add positions to the list when the distance between the last position and the new position exceeds the threshold
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            // Add positions
            positionsList.Add(movementSource.position);

            // Instantiate the cube and destroy after 3 sec
            if (debugCubePrefab)
            {
                GameObject debugPrefab = Realtime.Instantiate(debugCubePrefab.name, movementSource.position, Quaternion.identity,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);
            }

        }
    }
}
