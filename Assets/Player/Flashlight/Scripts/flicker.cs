using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class flicker : MonoBehaviour
{
    //Variables
    public int aggro;
    public int useAggro = 0;
    public int flickers = 0;
    public int lastFlicker = 0;
    public int lastPerformed = 0;
    public bool unable = false;

    //Game Objects
    public Light light;
    public AudioSource sfx;

    public enum aggressionTier {low = 1, med = 2, high = 3, scary = 4};

    void Update()
    {
        aggro = Behavior.aggressionLevel;
        useAggro = 0;
        int aggressionLevel = 0;

        if (aggro > 124)
        {
            int sub = aggro /100;
            sub *= 100;
            aggressionLevel = (aggro -sub) / 25;
        }
        else
        {
            aggressionLevel = aggro / 25;
        }
        
        if (aggressionLevel == 1)
            useAggro = (int)aggressionTier.low;
        else if (aggressionLevel == 2)
            useAggro = (int)aggressionTier.med;
        else if (aggressionLevel == 3)
            useAggro = (int)aggressionTier.high;
        else if (aggressionLevel == 4)
            useAggro = (int)aggressionTier.scary;

        switch (useAggro)
        {
            case 1:
                if (useAggro != lastPerformed)
                {
                    initiateFlicker(useAggro);
                }
                break;
            case 2:
                if (useAggro != lastPerformed)
                {
                    initiateFlicker(useAggro);
                }
                break;
            case 3:
                if (useAggro != lastPerformed)
                {
                    initiateFlicker(useAggro);
                }
                break;
            case 4:
                if (useAggro != lastPerformed)
                {
                    initiateFlicker(useAggro);
                }
                break;
        }
    }

    void initiateFlicker(int useAggro)
    {
        if (light.intensity < 1f)
        {
            unable = true;
        }
        else
        {
            lastPerformed = useAggro;
            bool scaryAngry = false;
            if (useAggro <= 3)
            {
                if (unable)
                {
                    flickers = Random.Range(3, 8);
                    unable = false;
                } 
                else
                    flickers = Random.Range(1, 4);
            }
            else
            {
                flickers = Random.Range(1, 4);
                scaryAngry = true;
            }
            StartCoroutine(flickerLight(scaryAngry));
        }
    }

    IEnumerator flickerLight(bool scaryAngry)
    {
        if (scaryAngry == false)
        {
            float seconds = Random.Range(0.5f, 1f);
            print("Waiting " + seconds);
            for (int i = 0; i <= flickers; i++)
            {
                if (light.intensity > 0)
                {
                    float rate = 0f;
                    if (unable)
                        rate = Random.Range(0.3f, 1f);
                    else
                        rate = Random.Range(0.8f, 1f);
                    light.intensity -= rate;
                }
                yield return new WaitForSeconds(seconds);
                light.intensity = 1f;
            }
        }
        else
        {
            float seconds = Random.Range(1f, 3f);
            print("Turning off light for: " + seconds);

            light.intensity = 0f;
            yield return new WaitForSeconds(seconds);
            light.intensity = 1f;
        }
    }
}