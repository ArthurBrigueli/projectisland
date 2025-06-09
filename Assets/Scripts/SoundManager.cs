using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
