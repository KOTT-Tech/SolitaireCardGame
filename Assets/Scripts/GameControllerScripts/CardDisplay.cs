using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI effectText1;
    [SerializeField]
    private TextMeshProUGUI effectText2;
    private CardData cardData;
    private Vector3 originalPosition;
    private bool isSelected = false;

    // 手札の親オブジェクト（GameManagerなど）への参照
    private UIManager uiManager;

    void Start()
    {
        uiManager = Object.FindAnyObjectByType<UIManager>();
    }

    public void Setup(CardData data)
    {
        cardData = data;
        nameText.text = data.cardName;
        effectText1.text = data.effectText1;
        effectText2.text = data.effectText2;
    }

    // マウスが乗った時：少し上に移動
    public void OnPointerEnter(PointerEventData eventData)
    {
        originalPosition = transform.localPosition;
        transform.localPosition += new Vector3(0, 30, 0); // 30ピクセル上に
    }

    // マウスが離れた時：元の位置に戻る
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition = originalPosition;
    }

    // クリックした時：ボタンを表示
    public void OnPointerClick(PointerEventData eventData)
    {
        // 左側のテキスト表示を更新
        uiManager.ShowCardDetail(cardData);
        uiManager.ShowActionButtons(this.gameObject, cardData);
    }
}