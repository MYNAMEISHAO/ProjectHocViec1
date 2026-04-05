using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDestroy), 1f);
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
 
