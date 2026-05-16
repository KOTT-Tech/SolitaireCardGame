using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CemeteryManager : MonoBehaviour
{
    public static CemeteryManager Instance;
    private List<CardData> cemeteryList = new List<CardData>();

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cemeteryCountText;
    [SerializeField] private GameObject cemeteryViewPanel; // 閲覧パネル
    [SerializeField] private Transform scrollContent;      // ScrollViewのContent
    [SerializeField] private GameObject cardPrefab;        // カードの見た目プレハブ

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // ゲーム開始時に一度枚数表示を初期化
        UpdateCemeteryUI();
    }

    // 墓地オブジェクト（またはボタン）をクリックしたときに実行する関数
    public void OnClickCemetery()
    {
        if (cemeteryList.Count == 0) return;

        cemeteryViewPanel.SetActive(true);
        RefreshCemeteryView();
    }

    private void RefreshCemeteryView()
    {
        // 1. すでに並んでいる古いカードを削除（掃除）
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        // 2. 墓地のリストを順番に生成してContentの中に並べる
        foreach (CardData data in cemeteryList)
        {
            GameObject cardObj = Instantiate(cardPrefab, scrollContent);
            
            // プレハブの元サイズを維持（念のため）
            cardObj.transform.localScale = Vector3.one;

            CardDisplay display = cardObj.GetComponent<CardDisplay>();
            if (display != null)
            {
                display.Setup(data);
            }
        }
    }

    public void CloseCemeteryView()
    {
        cemeteryViewPanel.SetActive(false);
    }

    public void SendToCemetery(CardData card)
    {
        if (card == null) return;

        cemeteryList.Add(card);
        UpdateCemeteryUI();
    }

    /// <summary>
    /// 🔴 新設：墓地から特定のカードデータを取り除く（効果発動による除外など）
    /// </summary>
    public void RemoveFromCemetery(CardData card)
    {
        if (card == null) return;

        // リストの中に指定されたカードが存在するかチェックして削除
        if (cemeteryList.Contains(card))
        {
            cemeteryList.Remove(card);
            Debug.Log($"CemeteryManager: {card.cardName} を墓地データから削除しました。");
            
            // 🔴 墓地から減ったので、画面の枚数テキストを更新する
            UpdateCemeteryUI();
        }
    }

    private void UpdateCemeteryUI()
    {
        if (cemeteryCountText != null)
        {
            cemeteryCountText.text = $"墓地: {cemeteryList.Count} 枚";
        }
    }

    public List<CardData> GetCemeteryList() { return cemeteryList; }
}