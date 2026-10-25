using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Sprite shadow;
    public Sprite swatter;
    public float rate = 2f;
    public float minRate = 0.4f;
    public float ramp = 0.05f;

    public Transform[] spawnPoints;

    float t;
    List<Transform> busyPoints = new List<Transform>();
    Transform lastSpawnPoint;

    void Update()
    {
        t += Time.deltaTime;

        if (t >= rate)
        {
            t = 0;
            SpawnHazard();

            if (rate > minRate)
            {
                rate -= ramp;
            }
        }
    }

    void SpawnHazard()
    {
        if (!prefab || spawnPoints == null || spawnPoints.Length == 0) return;
        if (busyPoints.Count >= 4) return;

        List<Transform> availablePoints = new List<Transform>();
        foreach (var pt in spawnPoints)
        {
            if (pt != null && !busyPoints.Contains(pt) && pt != lastSpawnPoint)
            {
                availablePoints.Add(pt);
            }
        }

        if (availablePoints.Count == 0)
        {
            foreach (var pt in spawnPoints)
            {
                if (pt != null && !busyPoints.Contains(pt))
                {
                    availablePoints.Add(pt);
                }
            }
        }

        if (availablePoints.Count == 0) return;

        Transform chosenPoint = availablePoints[Random.Range(0, availablePoints.Count)];

        lastSpawnPoint = chosenPoint;
        busyPoints.Add(chosenPoint);

        Vector3 startPos = chosenPoint.position - (chosenPoint.up * 1.5f);
        GameObject obj = Instantiate(prefab, startPos, chosenPoint.rotation);

        StartCoroutine(DropAndSmash(obj, chosenPoint, startPos));
    }

    IEnumerator DropAndSmash(GameObject obj, Transform targetTransform, Vector3 startPos)
    {
        SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
        Collider2D col = obj.GetComponent<Collider2D>();

        if (col) col.enabled = false;
        if (sr && shadow) sr.sprite = shadow;

        float timer = 0f;
        float warningDuration = 1.5f;
        Color c = sr ? sr.color : Color.white;

        while (timer < warningDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / warningDuration;

            if (obj)
            {
                obj.transform.position = Vector3.Lerp(startPos, targetTransform.position, progress);
            }

            if (sr)
            {
                float a = Mathf.Sin(timer * 8f);
                c.a = Mathf.Lerp(0.2f, 0.8f, (a + 1f) / 2f);
                sr.color = c;
            }
            yield return null;
        }

        if (obj) obj.transform.position = targetTransform.position;

        if (sr && swatter)
        {
            sr.sprite = swatter;
            c.a = 1f;
            sr.color = c;
        }

        if (col) col.enabled = true;

        StartCoroutine(ScreenShake(0.15f, 0.2f));

        yield return new WaitForSeconds(0.3f);

        busyPoints.Remove(targetTransform);
        if (obj) Destroy(obj);
    }

    IEnumerator ScreenShake(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        if (!cam) yield break;

        Vector3 originalPos = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originalPos;
    }
}