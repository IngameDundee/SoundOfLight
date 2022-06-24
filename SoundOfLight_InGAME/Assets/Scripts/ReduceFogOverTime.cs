using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceFogOverTime : MonoBehaviour
{


    private GameObject localFog;
    private EffectsManager effectsManager;
    public int fogRemoved = 250;


    // Start is called before the first frame update
    void Start()
    {

        localFog = GameObject.Find("LocalFog");
        effectsManager = localFog.GetComponent<EffectsManager>();
        InvokeRepeating("ReduceFog", 1f, 1f);
    }


    void ReduceFog()
    {
        effectsManager.AddFogIntensity(-fogRemoved);
    }
}
