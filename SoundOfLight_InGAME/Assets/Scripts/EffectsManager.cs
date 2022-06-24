using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is a public class that can be called by other scripts
to allow the level of fog to be adjusted by triggers in runtime
 */
[ExecuteInEditMode]
public class EffectsManager : MonoBehaviour
{
    public ParticleSystem groundFog, risingFog;

    public bool fogOn = true;
    private bool fogOnPrevious;

    private int fogIntensity = 100;
    private int fogIntensityPrevious;

    // Max and min fog intensity for aesthetic and performance
    public int maxFog = 2500;
    public int minFog = 50;

    void Start()
    {
        ToggleFog();
        fogOnPrevious = fogOn;
        fogIntensityPrevious = fogIntensity;
    }

    void Update()
    {
        // Check if fog has been turned off and toggle it
        if (fogOn != fogOnPrevious)
        {
            ToggleFog();
        }
        fogOnPrevious = fogOn;

        // Check if there has been any change in fog intensity
        if (fogOn && fogIntensityPrevious != fogIntensity || fogIntensity < minFog || fogIntensity > maxFog)
        {
            var groundFogEmission = groundFog.emission;
            var groundFogMain = groundFog.main;
            var risingFogEmission = risingFog.emission;
            var risingFogMain = risingFog.main;
            groundFogEmission.rateOverTime = fogIntensity;
            risingFogEmission.rateOverTime = fogIntensity/2;
            if (fogIntensity > minFog) 
            {
                if (fogIntensity > maxFog) // Ensure fog is less than/equal to the max value
                {
                    fogIntensity = maxFog;
                    groundFogMain.maxParticles = maxFog;
                    risingFogMain.maxParticles = maxFog / 2;
                }
                else if (fogIntensity < minFog) // Ensure fog is greater than/equal to the minimum value
                {
                    fogIntensity = minFog;
                    groundFogMain.maxParticles = minFog;
                    risingFogMain.maxParticles = minFog;
                }
                else // Update fog intensity in game to match parameter value changes
                {
                    groundFogMain.maxParticles = fogIntensity;
                    groundFogMain.maxParticles = fogIntensity / 2;
                }
            }
            else // Ensure fog is greater than/equal to the minimum value
            {
                fogIntensity = minFog;
            }
            
        }
        fogIntensityPrevious = fogIntensity;
    }

    // Public method to allow fog effects to be enabled/disabled by other scripts
    public void ToggleFog()
    {
        if (!fogOn)
        {
            groundFog.Stop();
            risingFog.Stop();
        }
        else
        {
            groundFog.Play();
            risingFog.Play();
        }
    }

    // Public method to allow fog intensity to be set by other scripts
    public void SetFogIntensity(int intensity)
    {
        fogIntensity = intensity;
    }

    // Public method to allow fog intensity to be incremented by other scripts
    public void AddFogIntensity(int intensity)
    {
        fogIntensity += intensity;
    }

    // Public method to return the current fog intensity value
    public int GetFogIntensity()
    {
        return fogIntensity;
    }
}
