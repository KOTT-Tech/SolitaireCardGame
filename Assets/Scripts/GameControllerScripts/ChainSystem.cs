using UnityEngine;

// TriggerTypeがないとエラーになるので定義
public enum TriggerType { OnSummon, OnToGrave, OnSpecialSummon }

public class ChainSystem : MonoBehaviour
{
    // BoardManagerから呼ばれるメソッドの「枠」
    public void AddChain(CardData card, TriggerType trigger) 
    { 
        Debug.Log($"Chain Added: {card.cardID} by {trigger}"); 
    }
}