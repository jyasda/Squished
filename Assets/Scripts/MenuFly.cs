using UnityEngine;
using System.Collections.Generic;

public class MenuFly : MonoBehaviour
{
    public Sprite[] flySprites;
    public float animSpeed = 0.1f;
    public float speed = 3f;
    public int maxFlies = 2;
    public float spawnInterval = 2.5f;

    class FlyData
    {
        public GameObject go;
        public SpriteRenderer sr;
        public Vector2 a;
        public Vector2 b;
        public float t;
        public float animT;
        public int idx;
    }

    List<FlyData> flies = new List<FlyData>();
    float timer;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (flies.Count < maxFlies && timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }

        for (int i = flies.Count - 1; i >= 0; i--)
        {
            FlyData f = flies[i];
            if (f.go == null)
            {
                flies.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(f.a, f.b);
            f.t += (speed * Time.deltaTime) / dist;
            f.go.transform.position = Vector2.Lerp(f.a, f.b, f.t);

            f.animT += Time.deltaTime;
            if (f.animT >= animSpeed && f.sr != null && flySprites != null && flySprites.Length > 0)
            {
                f.animT = 0f;
                f.idx = (f.idx + 1) % flySprites.Length;
                f.sr.sprite = flySprites[f.idx];
            }

            if (f.t >= 1f)
            {
                Destroy(f.go);
                flies.RemoveAt(i);
            }
        }
    }

    void Spawn()
    {
        if (!cam || flySprites == null || flySprites.Length == 0) return;

        GameObject go = new GameObject("FlyInstance");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = flySprites[0];

        int side = Random.Range(0, 4);
        float z = Mathf.Abs(cam.transform.position.z);
        Vector2 start = Vector2.zero;
        Vector2 end = Vector2.zero;

        if (side == 0)
        {
            start = cam.ViewportToWorldPoint(new Vector3(-0.1f, Random.value, z));
            end = cam.ViewportToWorldPoint(new Vector3(1.1f, Random.value, z));
        }
        else if (side == 1)
        {
            start = cam.ViewportToWorldPoint(new Vector3(1.1f, Random.value, z));
            end = cam.ViewportToWorldPoint(new Vector3(-0.1f, Random.value, z));
        }
        else if (side == 2)
        {
            start = cam.ViewportToWorldPoint(new Vector3(Random.value, -0.1f, z));
            end = cam.ViewportToWorldPoint(new Vector3(Random.value, 1.1f, z));
        }
        else
        {
            start = cam.ViewportToWorldPoint(new Vector3(Random.value, 1.1f, z));
            end = cam.ViewportToWorldPoint(new Vector3(Random.value, -0.1f, z));
        }

        go.transform.position = start;

        Vector2 dir = (end - start).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        flies.Add(new FlyData
        {
            go = go,
            sr = sr,
            a = start,
            b = end,
            t = 0f,
            animT = 0f,
            idx = 0
        });
    }
}