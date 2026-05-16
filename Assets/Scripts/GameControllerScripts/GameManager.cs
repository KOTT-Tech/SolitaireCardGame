using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    Setup,
    PlayerTurn,
    Processing,
    GameClear,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Manager References")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ChainSystem chainSystem;
    [SerializeField] private CSVLoader csvLoader;

    [Header("UI & Fields")]
    // 🔴 変更：インスペクターから、あらかじめ作成した5つの「マス目オブジェクト（Slot1〜5）」を順番に登録する
    [SerializeField] private List<Transform> fieldSlots = new List<Transform>(); 
    [SerializeField] private GameObject fieldCardPrefab; 

    [Header("Game Rules")]
    [SerializeField] private int maxFieldSlots = 5;
    private int remainingSummonRights = 1;

    // 🔴 変更：現在どのマス目にどのカードが置かれているかを管理する配列（5枠固定）
    // 中身が null の場所は「空きマス」と判定します
    private CardData[] placedCards = new CardData[5];

    private GameState currentstate;

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
        currentstate = GameState.Setup;
        
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

        currentstate = GameState.PlayerTurn;
        Debug.Log("ゲーム開始：手札を確認してください。");
    }

    /// <summary>
    /// プレイヤーが手札からカードを出す際の検知
    /// </summary>
    public bool CanNormalSummon()
    {
        // フィールドの空き枠チェックも連動させる
        return currentstate == GameState.PlayerTurn && remainingSummonRights > 0 && HasEmptySlot();
    }

    /// <summary>
    /// 通常召喚の実行
    /// </summary>
    public void ExecuteNormalSummon(CardData card)
    {
        if (!CanNormalSummon()) return;

        ChangeState(GameState.Processing);
        remainingSummonRights--;
        Debug.Log($"{card.cardName} を通常召喚しました。");

        // 🔴 変更：左から空いているマス目を探してカードの見た目を生成する
        SpawnCardOnFirstEmptySlot(card);

        // 連鎖（チェーン）システムにトリガーを送る
        chainSystem.AddChain(card, TriggerType.召喚したとき);
        
        if (currentstate == GameState.Processing)
        {
            ChangeState(GameState.PlayerTurn);
        }
        CheckGameState();
    }

    /// <summary>
    /// 🔴 新設：左から見て最初に空いているマス目にカードを生成してはめ込む
    /// </summary>
    private void SpawnCardOnFirstEmptySlot(CardData card)
    {
        if (fieldSlots == null || fieldSlots.Count < maxFieldSlots || fieldCardPrefab == null)
        {
            Debug.LogError("GameManager: fieldSlots(5要素必要) または fieldCardPrefab が正しくセットされていません！");
            return;
        }

        // 左（インデックス0番）から順番に、データが null（空きマス）の場所を探す
        for (int i = 0; i < fieldSlots.Count; i++)
        {
            if (placedCards[i] == null)
            {
                // 空きマスを発見
                Transform targetSlot = fieldSlots[i];

                // 1. そのマス目の真下（子要素）としてカードを生成
                GameObject spawnedCard = Instantiate(fieldCardPrefab, targetSlot);

                // 2. カードのサイズと位置をマス目にぴったりフィットさせる（アンカーがストレッチ前提）
                RectTransform rect = spawnedCard.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchoredPosition = Vector2.zero;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }

                // 3. 生成したカードにデータをセット
                CardDisplay cardDisplay = spawnedCard.GetComponent<CardDisplay>();
                if (cardDisplay != null)
                {
                    cardDisplay.Setup(card);
                }

                // 4. データ管理用の配列に記憶させ、このマスを「使用中」にする
                placedCards[i] = card;

                Debug.Log($"マス目 {i + 1} に {card.cardName} を配置しました。");
                return; // 配置完了したためループを抜ける
            }
        }
    }

    /// <summary>
    /// 特殊召喚や効果解決によって場にカードが出る際の空きチェック
    /// </summary>
    public bool HasEmptySlot()
    {
        // 🔴 変更：配列の中に1つでも null（空きマス）があれば召喚可能とする
        for (int i = 0; i < placedCards.Length; i++)
        {
            if (placedCards[i] == null) return true;
        }
        return false;
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
        currentstate = GameState.GameClear;
        Debug.Log("デッキがなくなりました！ゲームクリア！");
    }

    public void ChangeState(GameState newState)
    {
        currentstate = newState;
    }

    public void ResetSummonRights()
    {
        remainingSummonRights = 1;
    }
}