using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class RemoteFogController : MonoBehaviour
{
    // Fog Effects Initialization
    [Range(0, 1000)] public float fogIntesity = 500;
    private int intensity;
    private GameObject localFog;
    private EffectsManager effectsManager;

    // Bloom Effects Initialization
    [Range(0.0f, 10.0f)] public float bloomIntesity = 8f;
    [Range(0.000f, 2.000f)] public float bloomThreshold = 0.8f;
    private float bloomIntensityV;
    private float bloomThresholdV;
    private GameObject postProcessingGO;
    Bloom bloomLayer = null;
    PostProcessVolume volume;

    // Start is called before the first frame update
    void Start()
    {
        localFog = GameObject.Find("LocalFog");
        effectsManager = localFog.GetComponent<EffectsManager>();
        intensity = (int)fogIntesity;

        postProcessingGO = GameObject.Find("PostProcessing");
        volume = postProcessingGO.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloomLayer);
        bloomIntensityV = bloomIntesity;
        bloomThresholdV = bloomThreshold;

        SetEffects();
    }

    // Update is called once per frame
    void SetEffects()
    {
        if (intensity != (int)fogIntesity)
        {
            intensity = (int)fogIntesity;
            effectsManager.SetFogIntensity(intensity);
        }

        if (bloomIntensityV != bloomIntesity)
        {
            bloomIntensityV = bloomIntesity;
            bloomLayer.intensity.value = bloomIntensityV;
        }

        if (bloomThresholdV != bloomThreshold)
        {
            bloomThresholdV = bloomThreshold;
            bloomLayer.threshold.value = bloomThresholdV;
        }
    }
}
