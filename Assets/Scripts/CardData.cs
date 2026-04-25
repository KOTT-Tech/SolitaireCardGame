using UnityEngine;

// [重要] クラス名はファイル名(CardData.cs)と完全に一致させる必要があります
[CreateAssetMenu(fileName = "NewCard", menuName = "Card/Create CardData")]
public class CardData : ScriptableObject
{
    // CSVの列に合わせて項目を定義します
    public string cardID; // カードID
    public string actConType1; // カード効果(1)の発動条件の種類
    public string actCostType1; // カード効果(1)の発動コストの種類
    public string actCost1; // カード効果(1)の発動コストの数
    public string effectType1; // カード効果(1)の効果内容の種類
    public string effect1; // カード効果(1)の効果内容の数
    public string actConType2; // カード効果(2)の発動条件の種類
    public string actCostType2; // カード効果(2)の発動コストの種類
    public string actCost2; // カード効果(2)の発動コストの数
    public string effectType2; // カード効果(2)の効果内容の種類
    public string effect2; // カード効果(2)の効果内容の数
    // 必要に応じて hp, cost, icon などを追加してください
}
