using UnityEngine;

public class ShopSign : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Vector3 offSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerStay2D(Collider2D collision)
    {
        shopPanel.SetActive(true);
        shopPanel.transform.position = transform.position + offSet;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        shopPanel.SetActive(false);
    }
}
