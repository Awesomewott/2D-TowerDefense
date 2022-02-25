using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    static AudioManager instance = null;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    public AudioSource GrabAudioByName(string name, Transform parent)
    {
        AudioSource newAudio = new AudioSource();

        foreach (AudioSource audio in audioSources)
        {
            if (audio.name.ToLower().Equals(name.ToLower()))
            {
                newAudio = Instantiate(audio, parent);
            }
        }

        return newAudio;
    }

    public void PlayAudioByName(string name)
    {
        bool found = false;

        foreach (AudioSource audio in audioSources)
        {
            if (audio.name.ToLower().Equals(name.ToLower()))
            {
                audio.Play();
                found = true;
                break;
            }
        }

        if (!found) Debug.LogWarning($"{name} Audio Not Found");
    }

    public void PlayAudioByName(string name, Vector3 position)
    {
        bool found = false;

        foreach (AudioSource audio in audioSources)
        {
            if (audio.name.ToLower().Equals(name.ToLower()))
            {
                //AudioSource.PlayClipAtPoint(audio.clip, position);
                audio.transform.position = position;
                audio.Play();
                found = true;
                break;
            }
        }

        if (!found) Debug.LogWarning($"{name} Audio Not Found");
    }

    public static void PlayAudioByName(AudioSource[] audios, string name)
    {
        bool found = false;

        foreach (AudioSource audio in audios)
        {
            if (audio.name.ToLower().Equals(name.ToLower()))
            {
                audio.Play();
                found = true;
                break;
            }
        }

        if (!found) Debug.LogWarning($"{name} Audio Not Found");
    }

    public static void PlayAudioByName(AudioSource[] audios, string name, Vector3 position)
    {
        bool found = false;

        foreach (AudioSource audio in audios)
        {
            if (audio.name.ToLower().Equals(name.ToLower()))
            {
                AudioSource.PlayClipAtPoint(audio.clip, position);
                found = true;
                break;
            }
        }

        if (!found) Debug.LogWarning($"{name} Audio Not Found");
    }
}
