using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI kunaiText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
    }

    public void SetCoin(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }

    public void SetShield(int shieldCount)
    {
        shieldText.text = shieldCount.ToString();
    }

    public void SetKunai(int kunaiCount)
    {
        kunaiText.text = kunaiCount.ToString();
    }
}
