using UnityEngine;

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
}
