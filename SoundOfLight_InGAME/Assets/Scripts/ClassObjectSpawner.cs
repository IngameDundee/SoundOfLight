using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

/*
This script is based on Valem's tutorial video "How to detect a movement in VR" on Youtube - https://youtu.be/GRSOrkmasMM.
*/

public class ClassObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private Realtime _realtime;

    public List<GameObject> classObjects;
    public GameObject leftWrist;

    public void Spawn(string objectName)
    {

        foreach (var item in classObjects)
        {
            if(objectName == item.name)
            {
                GameObject classprefab = Realtime.Instantiate(item.name,
                    //item.transform.position,
                    leftWrist.transform.position,
                    Quaternion.identity,
                    ownedByClient: true,
                    preventOwnershipTakeover: false,
                    destroyWhenOwnerOrLastClientLeaves: true,
                    useInstance: _realtime);
                classprefab.transform.rotation = leftWrist.transform.rotation;
            }
        
        }

    }
}
