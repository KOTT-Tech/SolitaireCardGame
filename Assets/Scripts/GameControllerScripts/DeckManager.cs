using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckManager : MonoBehaviour
{
    public List<CardData> mainDeck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();

    [Header("UI設定")]
    public GameObject cardPrefab; // さきほど作ったカードのプレハブ
    public Transform handParent;  // Horizontal Layout Groupをつけたエリア

    // 🔴 追加：Unityエディタから「デッキ枚数表示用」のテキストを紐付ける枠
    [SerializeField] private TextMeshProUGUI deckCountText;

    private void Start()
    {
        // ゲーム開始時に一度、表示を最新にする（初期化）
        UpdateDeckUI();
    }

    public int GetDeckCount()
    {
        return mainDeck.Count;
    }

    public void SetDeck(List<CardData> cards)
    {
        // CSVからのデータをそのまま受け取り
        mainDeck = new List<CardData>(cards);
        
        // デッキがセットされたのでUIを更新
        UpdateDeckUI();
    }

    public void Shuffle()
    {
        for (int i = 0; i < mainDeck.Count; i++)
        {
            CardData temp = mainDeck[i];
            int randomIndex = Random.Range(i, mainDeck.Count);
            mainDeck[i] = mainDeck[randomIndex];
            mainDeck[randomIndex] = temp;
        }
    }

    public void DrawCards(int count)
    {
        // count回繰り返すが、デッキが空になったら直ちに break で抜ける
        for (int i = 0; i < count; i++)
        {
            if (mainDeck.Count <= 0) 
            {
                Debug.LogWarning("デッキが空なのでドローを中断しました");
                break; // これが重要！
            }

            CardData data = mainDeck[0];
            mainDeck.RemoveAt(0);
            hand.Add(data);

            GameObject newCard = Instantiate(cardPrefab, handParent);
            newCard.GetComponent<CardDisplay>().Setup(data);
        }

        // 🔴 カードを引き終わったので画面の枚数表示を更新する
        UpdateDeckUI();
    }

    /// <summary>
    /// 【効果処理用】手札に加えずに、デッキの一番上からカードデータを1枚抜き取って返す
    /// デッキが空の場合は null を返します
    /// </summary>
    public CardData DrawCardDataWithoutAddingToHand()
    {
        if (mainDeck == null || mainDeck.Count == 0)
        {
            Debug.LogWarning("DeckManager: デッキが空のため、カードを抜き取れませんでした。");
            return null;
        }

        // 2. デッキの一番上（インデックス0番）のカードデータを取得
        CardData topCard = mainDeck[0];

        // 3. デッキ（山札）からそのカードを削除して、山札を減らす
        mainDeck.RemoveAt(0);

        // 🔴 デッキからカードが減ったので画面の枚数表示を更新する
        UpdateDeckUI();

        // 4. 連鎖的に勝利条件などをチェックするため、GameManagerに通知（念のため）
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckGameState();
        }

        // 5. 抜き取ったカードのデータを呼び出し元（ChainSystemなど）に返す
        return topCard;
    }

    /// <summary>
    /// 🔴 新設：画面のデッキ残り枚数テキストを更新するメソッド
    /// </summary>
    public void UpdateDeckUI()
    {
        if (deckCountText != null)
        {
            // mainDeckの現在の要素数を文字にして代入
            deckCountText.text = $"デッキ: {mainDeck.Count} 枚";
        }
    }
}