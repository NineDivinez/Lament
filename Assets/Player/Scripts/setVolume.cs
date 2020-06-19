using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setVolume : MonoBehaviour
{
    public AudioMixer volumeController;

    public void volumeMaster(float volume)
    {
        volumeController.SetFloat("exposedMaster", volume);
    }
    public void volumePlayer(float volume)
    {
        volumeController.SetFloat("exposedPlayerSFX", volume);
    }
    public void volumeEnvironmentSound(float volume)
    {
        volumeController.SetFloat("exposedEnvironment", volume);
    }
    public void volumeMenu(float volume)
    {
        volumeController.SetFloat("exposedMenu", volume);
    }
}
