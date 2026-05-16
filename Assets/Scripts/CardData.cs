using UnityEngine;

// CSVの「発動条件種類 / 発動条件」の列に対応
public enum TriggerType
{
    None,
    手札に居るとき,
    召喚したとき,
    ドローしたとき,
    特殊召喚したとき,
    墓地に居るとき,
    フィールドに居るとき,
    手札に加えた時
}

// CSVの「発動コスト種類」の列に対応
public enum CostType
{
    None,
    なし,
    墓地に送る,
    デッキに戻す
}

// CSVの「効果内容種類」の列に対応
public enum EffectType
{
    None,
    ドローする,
    墓地に送る,
    フィールドに特殊召喚する,
    墓地から手札に加える,
    手札のカードを墓地に送る,
    デッキに戻す
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card/Create CardData")]
public class CardData : ScriptableObject
{
    public int cardID; // カードID
    public string cardName; // カード名
    public string conType1; // カード効果(1)の発動条件の種類
    public string costTarget1; // カード効果(1)の発動コストの対象
    public int costTargetCount1; // カード効果(1)の発動コストの数
    public string costType1; // カード効果(1)の発動コストの種類
    public string effectTarget1; // カード効果(1)の効果対象
    public int effectTargetCount1; // カード効果(1)の効果対象の数
    public string effectType1; // カード効果(1)の効果内容の種類
    public string effectText1; // 効果(1)のテキスト
    
    public string conType2; // カード効果(2)の発動条件の種類
    public string costTarget2; // カード効果(2)の発動コストの対象
    public int costTargetCount2; // カード効果(2)の発動コストの数
    public string costType2; // カード効果(2)の発動コストの種類
    public string effectTarget2; // カード効果(2)の効果対象
    public int effectTargetCount2; // カード効果(2)の効果対象の数
    public string effectType2; // カード効果(2)の効果内容の種類
    public string effectText2; // 効果(2)のテキスト

    // ▼▼▼ プログラムでの判定用に自動変換された enum を格納する変数 ▼▼▼
    [System.NonSerialized] public TriggerType triggerEnum1;
    [System.NonSerialized] public CostType costEnum1;
    [System.NonSerialized] public EffectType effectEnum1;

    [System.NonSerialized] public TriggerType triggerEnum2;
    [System.NonSerialized] public CostType costEnum2;
    [System.NonSerialized] public EffectType effectEnum2;
}