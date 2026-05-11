using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject detailPanel;
    public TextMeshProUGUI detailNameText;
    public TextMeshProUGUI detailEffectText1;
    public TextMeshProUGUI detailEffectText2;
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
        detailEffectText1.text = data.effectText1;
        detailEffectText2.text = data.effectText2;
    }

    // アクションボタンを表示
    public void ShowActionButtons(GameObject card, CardData data)
    {
        actionPanel.SetActive(true);
        actionPanel.transform.position = card.transform.position + new Vector3(0, 170, 0);
    }
}