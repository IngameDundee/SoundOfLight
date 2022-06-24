using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RealtimeDestroy : MonoBehaviour
{
    private RealtimeView realtimeView;

    public float destroyTimer = 15;

    private void OnEnable()
    {
        realtimeView = GetComponent<RealtimeView>();
    }

    void Start()
    {

        if (realtimeView.isOwnedLocallySelf)
        {
            StartCoroutine(SelfDestruct());
        }
        
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(destroyTimer);
        Realtime.Destroy(gameObject);
    }
}
