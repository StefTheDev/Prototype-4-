using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundClip
{
    public string name;
    public AudioClip sound;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;

    public static AudioManager Instance {  get { return instance; } }

    public SoundClip[] soundClips;

    private Dictionary<string, AudioClip> audioClipsDict;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();

        audioClipsDict = new Dictionary<string, AudioClip>();

        for (int i = 0; i < soundClips.Length; i++)
        {
            audioClipsDict.Add(soundClips[i].name, soundClips[i].sound);
        }
    }

    public void PlaySound(string _soundName, float _volume = 1.0f, float _pitch = 1.0f)
    {
        float oldPitch = audioSource.pitch;

        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(audioClipsDict[_soundName], _volume);
        audioSource.pitch = oldPitch;
    }
}
