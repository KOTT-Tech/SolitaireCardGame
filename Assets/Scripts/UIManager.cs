using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject detailPanel;
    public TextMeshProUGUI detailNameText;
    public TextMeshProUGUI detailEffectText;
    [SerializeField]
    private GameObject actionPanel; // 「召喚」「発動」ボタンをまとめた親パネル

    void Start()
    {
        detailPanel.SetActive(false);
        actionPanel.SetActive(false);
    }

    // カード詳細を表示
    public void ShowCardDetail(CardData data)
    {
        detailPanel.SetActive(true);
        detailNameText.text = data.cardName;
        detailEffectText.text = data.effectDescription; // CSVに効果列がある前提
    }

    // アクションボタンを表示
    public void ShowActionButtons(GameObject card, CardData data)
    {
        actionPanel.SetActive(true);
        actionPanel.transform.position = card.transform.position + new Vector3(0, 170, 0);
    }
}