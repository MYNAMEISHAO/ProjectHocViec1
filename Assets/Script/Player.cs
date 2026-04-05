using System;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private bool isGrounded;
    private bool isJumping;
    private bool isAttack;
    private bool isDead = false;

    private float horizontal;
    public int coinCount = 0;
    public int shieldCount = 0;

    [SerializeField] private Kunai KunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject AttackArea;

    private Vector3 savePoint;
    // Update is called once per frame
    private void Awake()
    {
        coinCount = PlayerPrefs.GetInt("CoinCount", 0);
        shieldCount = PlayerPrefs.GetInt("ShieldCount", 0);

    }
    void Update()
    {
        if(isDeath || isDead)
        {
            return;
        }
        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");

        if(isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            //jump
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                Jump();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.K))
            {
                Throw();
                return;
            }
            //attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                return;
            }
            //run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("Run");
            }
            else if (isGrounded && !isJumping && !isAttack)
            {
                ChangeAnim("Idle");
                rb.linearVelocity = Vector2.zero;
            }
        }
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            ChangeAnim("Fall");
            isJumping = false;
        }
        //Moving
        if (Mathf.Abs(horizontal) > 0.1f && !isAttack)
        {
            rb.linearVelocity = new Vector2(horizontal * Time.fixedDeltaTime * moveSpeed, rb.linearVelocity.y);
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0);
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        isJumping = false;
        isDead = false;

        transform.position = savePoint;
        ChangeAnim("Idle");
        DeactiveAttackArea();
        UIManager.instance.SetCoin(coinCount);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Invoke(nameof(OnInit), 2f);
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position +  Vector3.down * 1.1f, Color.red);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return raycastHit2D.collider != null;
    }
    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("Idle");
    }
    public void Attack()
    {
        isAttack = true;
        rb.linearVelocity = Vector2.zero;
        ChangeAnim("Attack");
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttackArea();
        Invoke(nameof(DeactiveAttackArea), 0.5f);
    }
    public void Throw()
    {
        isAttack = true;
        rb.linearVelocity = Vector2.zero;
        ChangeAnim("Throw");
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(KunaiPrefab, throwPoint.position, transform.rotation);
    }
    public void Jump()
    {
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(Vector2.up * jumpForce);
    }
    public void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttackArea()
    {
        AttackArea.SetActive(true);
    }

    private void DeactiveAttackArea()
    {
        AttackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public void AddShield(int count)
    {
        if (SpendCoin(5))
        {
            shieldCount += count;
            PlayerPrefs.SetInt("ShieldCount", shieldCount);
            UIManager.instance.SetShield(shieldCount);
        }

    }

    public void AddHP(float hp)
    {
        if (SpendCoin(10))
        {

            hp += hp;
            base.OnHit(-hp);
        }
    }
    public bool SpendCoin(int coin)
    {
        if (coinCount < coin) return false;
        else
        {
            coinCount -= coin;
            PlayerPrefs.SetInt("CoinCount", coinCount);
            UIManager.instance.SetCoin(coinCount);
            return true;
        }
        
    }
    public override void OnHit(float damage)
    {
        if (shieldCount > 0)
        {
            shieldCount--;
           
            UIManager.instance.SetShield(shieldCount);
            return;
        }
        base.OnHit(damage);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coinCount++;
            PlayerPrefs.SetInt("CoinCount", coinCount);
            UIManager.instance.SetCoin(coinCount);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Death")
        {
            isDead = true;
            ChangeAnim("Death");
            Invoke(nameof(OnInit), 1f);
        }
    }
}
