using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class SimpleSFX : MonoBehaviour
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    public float startDelay = 0f;

    public bool playOnStart;
    public bool playOnDestroy;
    public bool playOnPlayerHit;

    static AudioSource sfx;

    void Awake()
    {
        if (!sfx)
        {
            var obj = new GameObject("SFX");
            sfx = obj.AddComponent<AudioSource>();
            DontDestroyOnLoad(obj);
        }
    }

    void Start()
    {
        if (playOnStart && clip)
        {
            StartCoroutine(PlayDelayed());
        }
    }

    void OnDestroy()
    {
        if (playOnDestroy && clip && gameObject.scene.isLoaded)
        {
            sfx.PlayOneShot(clip, volume);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playOnPlayerHit && clip && other.CompareTag("Player"))
        {
            sfx.PlayOneShot(clip, volume);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (playOnPlayerHit && clip && col.gameObject.CompareTag("Player"))
        {
            sfx.PlayOneShot(clip, volume);
        }
    }

    IEnumerator PlayDelayed()
    {
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);

        if (clip)
        {
            sfx.PlayOneShot(clip, volume);
        }
    }
}