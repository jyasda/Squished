using System.Collections;
using UnityEngine;

public class MenuIntroController : MonoBehaviour
{
    public RectTransform title;
    public UnityEngine.UI.Image fadeOverlay;

    public float dropOffset = 800f;
    public float fallDuration = 0.35f;
    public float fadeDuration = 0.4f;

    void Start()
    {
        StartCoroutine(AnimateIntro());
    }

    IEnumerator AnimateIntro()
    {
        Vector2 endPos = title.anchoredPosition;
        Vector2 startPos = endPos + new Vector2(0, dropOffset);
        title.anchoredPosition = startPos;

        if (fadeOverlay != null)
        {
            Color c = fadeOverlay.color;
            c.a = 1f;
            fadeOverlay.color = c;
            fadeOverlay.gameObject.SetActive(true);
        }

        float t = 0;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float progress = t / fallDuration;
            float curve = Mathf.Sin(progress * Mathf.PI * 0.5f);
            title.anchoredPosition = Vector2.Lerp(startPos, endPos, curve);
            yield return null;
        }
        title.anchoredPosition = endPos;

        if (fadeOverlay != null)
        {
            t = 0;
            Color startColor = fadeOverlay.color;
            Color endColor = startColor;
            endColor.a = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeOverlay.color = Color.Lerp(startColor, endColor, t / fadeDuration);
                yield return null;
            }

            fadeOverlay.color = endColor;
            fadeOverlay.gameObject.SetActive(false);
        }
    }
}