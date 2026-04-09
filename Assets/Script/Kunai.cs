using UnityEngine;

public class Kunai : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject hitVFX;
    public Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        rb.linearVelocity = transform.right * speed;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void DestroyVFX(GameObject vfx)
    {
        Destroy(vfx);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Character>().OnHit(30);
            GameObject go = Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(go, 1f);
            OnDespawn();
        }
        if (collision.CompareTag("Ground"))
        {
            OnDespawn();
        }

    }
}
