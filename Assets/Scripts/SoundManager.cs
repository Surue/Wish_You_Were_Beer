using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField]
    AudioClip pipi;
    [SerializeField]
    AudioClip bierePosee;
    [SerializeField]
    AudioClip flush;
    [SerializeField]
    AudioClip exclamation;
    [SerializeField]
    AudioClip interrogation;
    [SerializeField]
    AudioClip punch;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Multiple instances of SoundEffects");
        }
        Instance = this;
    }

    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }

    public void Pee()
    {
        MakeSound(pipi);
    }

    public void BruitBiere()
    {
        MakeSound(bierePosee);
    }

    public void ChasseEau()
    {
        MakeSound(flush);
    }

    public void Facher()
    {
        MakeSound(exclamation);
    }

    public void Question()
    {
        MakeSound(interrogation);
    }

    public void Coup()
    {
        MakeSound(punch);
    }
}
