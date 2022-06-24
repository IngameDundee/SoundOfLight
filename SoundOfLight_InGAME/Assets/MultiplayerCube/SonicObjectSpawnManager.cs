using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SonicObjectSpawnManager : MonoBehaviour
{
    //Reference to Realtime to use to instantiate brush strokes
    [SerializeField]
    private Realtime _realtime;

    //Prefab to instantiate
    [SerializeField]
    private GameObject sonicObjectPrefab;

    //Oculus types
    private OVRHand[] ovrHands;

    public float spawnTimer;
    public float resetTimer;

    private bool isLMiddlePinching = false;
    private bool isRMiddlePinching = false;
    

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
        if (!_realtime.connected)
            return;

        isLMiddlePinching = ovrHands[0].GetFingerIsPinching(OVRHand.HandFinger.Middle);
        isRMiddlePinching = ovrHands[1].GetFingerIsPinching(OVRHand.HandFinger.Middle);

        if (isLMiddlePinching && isRMiddlePinching)
        {
        
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0)
            {

                GameObject soundSphere = Realtime.Instantiate(sonicObjectPrefab.name,
                    position: new Vector3(transform.position.x,
                    transform.position.y, transform.position.z),
                    rotation: transform.rotation,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);
                //effectsManager.AddFogIntensity(fogIntensity);
                spawnTimer = resetTimer;
            }
        }
        
    }

    //To show where to spawn the object in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }

}
