using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM inst;

    public AudioClip musicClip;
    public float volumeLevel = 1f;

    void Awake()
    {
        if (inst != null && inst != this)
        {
            Destroy(gameObject);
            return;
        }

        inst = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        var src = gameObject.AddComponent<AudioSource>();
        src.clip = musicClip;
        src.loop = true;
        src.volume = volumeLevel;
        src.Play();
    }
}