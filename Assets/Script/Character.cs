using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] private CombatText combatTextPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float hp;
    private string currentAnim;
    public bool isDeath => hp <= 0;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(hp,transform);
    }

    public virtual void OnDespawn()
    {
    }

    protected virtual void OnDeath()
    {
        ChangeAnim("Death");
        Debug.Log($"{gameObject.name} died");
        Invoke(nameof(OnDespawn), 2f);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }

    }

    public void OnHit(float damage)
    {
        Debug.Log($"{gameObject.name} hit with {damage} damage , current hp" + hp);
        if (!isDeath) 
        {
            hp -= damage;
            Instantiate(combatTextPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity).OnInit(damage);
            if (isDeath)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
        }
    }

    
}
