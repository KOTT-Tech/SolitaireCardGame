using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardData> mainDeck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();

    public int GetDeckCount() { return 2; } // 仮の数値

    [Header("UI設定")]
    public GameObject cardPrefab; // さきほど作ったカードのプレハブ
    public Transform handParent;  // Horizontal Layout Groupをつけたエリア

    public void SetDeck(List<CardData> cards)
    {
        // CSVからのデータをそのまま受け取り（40枚固定）
        mainDeck = new List<CardData>(cards);
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
    }
}