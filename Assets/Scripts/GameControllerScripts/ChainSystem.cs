using UnityEngine;

public class ChainSystem : MonoBehaviour
{
    public static ChainSystem Instance;
    [SerializeField] private DeckManager deckManager;

    private void Awake()
    {
        // インスタンス化（シングルトン）の初期設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // もしシーン内に2つ目が存在してしまったら重複エラーを防ぐために削除
            Destroy(gameObject);
            return;
        }

        if (deckManager == null)
        {
            deckManager = Object.FindAnyObjectByType<DeckManager>();
            
            // それでも見つからない場合は警告を出す
            if (deckManager == null)
            {
                Debug.LogError("ChainSystem: シーン内に DeckManager が見つかりません！ヒエラルキーを確認してください。");
            }
        }
    }

    public void AddChain(CardData card, TriggerType currentTrigger)
    {
        // --- 既存の AddChain 処理 ---
        if (card.triggerEnum1 == currentTrigger && card.triggerEnum1 != TriggerType.None)
        {
            ExecuteEffect(card.effectEnum1, card.effectTargetCount1, card.effectTarget1);
        }
        // ...
    }

    private void ExecuteEffect(EffectType type, int count, string target)
    {
        switch (type)
        {
            case EffectType.ドローする:
                if (deckManager == null)
                {
                    Debug.LogError("deckManager が Null のため、ドロー効果を実行できませんでした。");
                    break;
                }

                Debug.Log($"【効果解決】{target}を {count} 枚ドローします。");
                deckManager.DrawCards(count);
                break;

            case EffectType.墓地に送る:
                if (target == "デッキのカード")
                {
                    Debug.Log($"【効果解決】デッキからカードを {count} 枚墓地に送ります。");
                    
                    for (int i = 0; i < count; i++)
                    {
                        CardData topCard = deckManager.DrawCardDataWithoutAddingToHand();
                        
                        if (topCard != null)
                        {
                            CemeteryManager.Instance.SendToCemetery(topCard);
                        }
                    }
                }
                break;
        }

        // GameManagerの状態チェック
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckGameState();
        }
    }

    public void AddChainSpecific(CardData card, int effectNumber)
    {
        if (card == null) return;

        // 🔴 追加：発動した効果の「条件」が何かを特定する
        TriggerType currentTrigger = (effectNumber == 1) ? card.triggerEnum1 : card.triggerEnum2;

        if (effectNumber == 1)
        {
            Debug.Log($"【効果1発動】{card.cardName} の効果(1)を解決します。");
            ExecuteEffect(card.effectEnum1, card.effectTargetCount1, card.effectTarget1);
        }
        else if (effectNumber == 2)
        {
            Debug.Log($"【効果2発動】{card.cardName} の効果(2)を解決します。");
            ExecuteEffect(card.effectEnum2, card.effectTargetCount2, card.effectTarget2);
        }

        // 🔴 追加：もし発動した場所が「墓地」だった場合、効果解決後にそのカードを墓地データから除外する
        // (遊戯王の墓地から除外して発動する効果などのイメージです)
        if (currentTrigger == TriggerType.墓地に居るとき)
        {
            if (CemeteryManager.Instance != null)
            {
                CemeteryManager.Instance.RemoveFromCemetery(card);
                Debug.Log($"【墓地除外】{card.cardName} は墓地での効果を発動したため、墓地から除外されました。");
            }
        }
    }
}