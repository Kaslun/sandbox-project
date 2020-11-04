using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer master;

    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public float ambienceVolume;

    private void FixedUpdate()
    {
        master.SetFloat("MasterVolume", masterVolume);
        master.SetFloat("SFXVolume", sfxVolume);
        master.SetFloat("MusicVolume", musicVolume);
        master.SetFloat("AmbienceVolume", ambienceVolume);
    }

    //Used for example as a randomizer for steps
    public static AudioClip RandomClip(AudioClip[] clip)
    {
        int i = Random.Range(0, clip.Length);

        return clip[i];
    }

    //Get soundclip based on pathname in Resources folder
    public static AudioClip GetClipByPath(string filename)
    {
        AudioClip clip = Resources.Load("Sound/" + filename) as AudioClip;

        return clip;
    }
}
