using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float rotSpeed = 15f;
    public Sprite[] sprites;
    public float delay = 0.08f;

    SpriteRenderer rend;
    int idx = 0;
    float t = 0;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, v, 0).normalized;
        transform.position += move * speed * Time.deltaTime;

        if (move.sqrMagnitude > 0)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            t += Time.deltaTime;
            if (t >= delay)
            {
                t = 0;
                idx = (idx + 1) % sprites.Length;
                rend.sprite = sprites[idx];
            }
        }
        else
        {
            t = 0;
            idx = 0;
            rend.sprite = sprites[0];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("Player touched: " + collision.gameObject.name);

        
        if (collision.gameObject.name.Contains("Swatter") || collision.CompareTag("Hazard") || collision.transform.root.name.Contains("Hazard"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
            else
            {
                
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }

}