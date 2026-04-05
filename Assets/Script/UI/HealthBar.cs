using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Vector3 offset;
    float hp;
    float maxHp;

    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, hp / maxHp, Time.deltaTime * 5f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;
        this.maxHp = maxHp;
        hp = maxHp;
        fill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }
}
