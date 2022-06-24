using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseFogRTPC : MonoBehaviour
{

    private GameObject localFog;
    private EffectsManager effectsManager;
    private int fogIntensity;

    [Header("Wwise Events")]
    public AK.Wwise.RTPC fogAmount;

    // Start is called before the first frame update
    void Start()
    {
        localFog = GameObject.Find("LocalFog");
        effectsManager = localFog.GetComponent<EffectsManager>();

        fogAmount.SetGlobalValue(0);
    }

    // Update is called once per frame
    void Update()
    {
        fogIntensity = effectsManager.GetFogIntensity();
        fogAmount.SetGlobalValue(fogIntensity);
    }


}
