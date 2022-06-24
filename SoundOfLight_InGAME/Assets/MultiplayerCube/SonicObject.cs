using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SonicObject : MonoBehaviour
{

    public float destroyTimer;

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(destroyTimer);
        Realtime.Destroy(gameObject);
    }
}
