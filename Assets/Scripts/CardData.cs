using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card/Create CardData")]
public class CardData : ScriptableObject
{
    public int cardID; // カードID
    public string cardName; // カード名
    public string actConType1; // カード効果(1)の発動条件の種類
    public string actConTarget1; // カード効果(1)の発動コストの対象
    public string actCostType1; // カード効果(1)の発動コストの種類
    public int actCost1; // カード効果(1)の発動コストの数
    public string effectType1; // カード効果(1)の効果内容の種類
    public int effect1; // カード効果(1)の効果内容の数
    public string actConType2; // カード効果(2)の発動条件の種類
    public string actConTarget2; // カード効果(2)の発動コストの対象
    public string actCostType2; // カード効果(2)の発動コストの種類
    public int actCost2; // カード効果(2)の発動コストの数
    public string effectType2; // カード効果(2)の効果内容の種類
    public int effect2; // カード効果(2)の効果内容の数
    public string effectDescription; // 効果テキスト
}
