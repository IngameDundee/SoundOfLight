using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    
    public List<GameObject> classObjects;

    public void Spawn(string objectName)
    {

        foreach (var item in classObjects)
        {
            item.SetActive(objectName == item.name);
        }

    }
}
