using TMPro;
using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public GameObject[] frames;
    public TMP_Text scoreText;
    public float frameRate = 0.15f;
    public int scoreValue = 1;

    public Vector2 minBounds = new Vector2(-8f, -4f);
    public Vector2 maxBounds = new Vector2(8f, 4f);

    private int score;
    private GameObject currentInstance;
    private int frameIndex;
    private float timer;

    void Start()
    {
        SpawnNew();
    }

    void Update()
    {
        if (frames == null || frames.Length == 0 || !currentInstance) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0;
            frameIndex = (frameIndex + 1) % frames.Length;

            Vector3 pos = currentInstance.transform.position;
            Destroy(currentInstance);

            currentInstance = Instantiate(frames[frameIndex], pos, Quaternion.identity);
            SetupCollider(currentInstance);
        }
    }

    void SpawnNew()
    {
        float x = Random.Range(minBounds.x, maxBounds.x);
        float y = Random.Range(minBounds.y, maxBounds.y);
        Vector3 spawnPos = new Vector3(x, y, 0);

        frameIndex = 0;
        currentInstance = Instantiate(frames[0], spawnPos, Quaternion.identity);
        SetupCollider(currentInstance);
    }

    void SetupCollider(GameObject obj)
    {
        var col = obj.GetComponent<Collider2D>();
        if (!col) col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        var trigger = obj.GetComponent<CollectibleTrigger>();
        if (!trigger) trigger = obj.AddComponent<CollectibleTrigger>();
        trigger.manager = this;
    }

    public void OnCollected()
    {
        score += scoreValue;
        if (scoreText) scoreText.text = score.ToString();

        if (currentInstance) Destroy(currentInstance);
        SpawnNew();
        
        if (ScoreManager.inst)
            ScoreManager.inst.AddScore(scoreValue);

        if (currentInstance) Destroy(currentInstance);
        SpawnNew();
    }
}

public class CollectibleTrigger : MonoBehaviour
{
    [HideInInspector] public CollectibleManager manager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && manager)
        {
            manager.OnCollected();
        }
    }
}