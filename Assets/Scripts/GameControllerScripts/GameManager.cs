using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    Setup,      // 初期設定（デッキ構築など）
    PlayerTurn, // プレイヤーが操作可能な状態
    Processing, // 連鎖（チェーン）処理中
    GameClear,  // 勝利（デッキ0枚達成）
    GameOver    // 終了
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Manager References")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ChainSystem chainSystem;

    [Header("Game Rules")]
    [SerializeField] private int maxFieldSlots = 5;
    private int remainingSummonRights = 1; // 召喚権（基本1回）

    [Header("References")]
    [SerializeField] private CSVLoader csvLoader;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// ゲームの開始処理
    /// </summary>
    public void StartGame()
    {
        CurrentState = GameState.Setup;
        
        if (csvLoader != null && csvLoader.deck != null) 
        {
            var loadedCards = csvLoader.deck;
            deckManager.SetDeck(loadedCards);
        }
        else 
        {
            Debug.LogError("CSVLoader または Deckリスト が見つかりません！インスペクターを確認してください。");
        }

        // 2. シャッフルと初期ドロー
        deckManager.Shuffle();
        deckManager.DrawCards(5); // 5枚引く

        CurrentState = GameState.PlayerTurn;
        Debug.Log("ゲーム開始：手札を確認してください。");
    }

    /// <summary>
    /// プレイヤーが手札からカードを出す際の検知
    /// </summary>
    public bool CanNormalSummon()
    {
        return CurrentState == GameState.PlayerTurn && remainingSummonRights > 0;
    }

    /// <summary>
    /// 通常召喚の実行
    /// </summary>
    public void ExecuteNormalSummon(CardData card)
    {
        if (!CanNormalSummon()) return;

        remainingSummonRights--;
        Debug.Log($"{card.cardName} を通常召喚しました。残り召喚権: {remainingSummonRights}");

        // 連鎖（チェーン）システムにトリガーを送る
        // 例: OnSummon(召喚時)トリガーの効果をスタックに積む
        chainSystem.AddChain(card, TriggerType.OnSummon);
        
        CheckGameState();
    }

    /// <summary>
    /// 特殊召喚や効果解決によって場にカードが出る際の空きチェック
    /// </summary>
    public bool HasEmptySlot()
    {
        // 現在の場にいるカードリストの数をチェック（実装に応じて調整）
        // return currentFieldCards.Count < maxFieldSlots;
        return true; 
    }

    /// <summary>
    /// 毎アクション後に勝利条件をチェック
    /// </summary>
    public void CheckGameState()
    {
        if (deckManager.GetDeckCount() == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        CurrentState = GameState.GameClear;
        Debug.Log("デッキがなくなりました！");
        // ここにリザルト画面の表示処理などを追加
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }
}