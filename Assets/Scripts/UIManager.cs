using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private CardDisplay selectedCardDisplay;
    private CardData selectedCardData;

    [Header("UI Panels")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject actionButtonPanel;
    [SerializeField] private GameObject bgCloseButton;

    // 🔴 追加：効果選択用のポップアップパネルと、効果1・効果2の選択ボタン
    [Header("Effect Selection Panel")]
    [SerializeField] private GameObject effectSelectPanel; 
    [SerializeField] private Button effect1SelectButton;
    [SerializeField] private TextMeshProUGUI effect1ButtonText;
    [SerializeField] private Button effect2SelectButton;
    [SerializeField] private TextMeshProUGUI effect2ButtonText;

    [Header("Action Buttons")]
    [SerializeField] private GameObject summonButton; // 召喚ボタンのゲームオブジェクト
    [SerializeField] private GameObject effectButton; // 効果発動ボタンのゲームオブジェクト

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;       // 左側のカード名テキスト
    [SerializeField] private TextMeshProUGUI effectText1;   // 左側の効果1テキスト
    [SerializeField] private TextMeshProUGUI effectText2;

    /// <summary>
    /// 左側のテキスト表示を更新する
    /// </summary>
    public void ShowCardDetail(CardData data)
    {
        if (data == null) return;

        // 🔴 インスペクターでセットしたテキストコンポーネントに文字を代入
        if (nameText != null) nameText.text = data.cardName;
        if (effectText1 != null) effectText1.text = data.effectText1;
        if (effectText2 != null) effectText2.text = data.effectText2;

        if (detailPanel != null) detailPanel.SetActive(true);
    }

    public void ShowActionButtons(GameObject cardObject, CardData data)
    {
        selectedCardDisplay = cardObject.GetComponent<CardDisplay>();
        selectedCardData = data;

        if (actionButtonPanel != null)
        {
            actionButtonPanel.SetActive(true);
            actionButtonPanel.transform.position = cardObject.transform.position + new Vector3(0, 150, 0);

            // 🔴 判定を拡張：今、墓地閲覧パネルが開いているかどうか
            bool isCemeteryViewOpen = CemeteryManager.Instance.GetCemeteryList().Count > 0 && 
                                    actionButtonPanel.transform.root.GetComponentInChildren<ScrollRect>() != null; 
                                    // ※簡易的な判定です。より確実には「引数」で場所を渡すのがベスト。

            // 召喚ボタン：手札からクリックされた時だけ表示
            if (summonButton != null) 
                summonButton.SetActive(!isCemeteryViewOpen); 

            // 効果ボタン：手札用 or 墓地用の条件に合致すれば表示
            bool canUseEffect = false;
            if (!isCemeteryViewOpen) {
                // 手札にいる時
                canUseEffect = (data.triggerEnum1 == TriggerType.手札に居るとき || data.triggerEnum2 == TriggerType.手札に居るとき);
            } else {
                // 墓地にいる時
                canUseEffect = (data.triggerEnum1 == TriggerType.墓地に居るとき || data.triggerEnum2 == TriggerType.墓地に居るとき);
            }

            if (effectButton != null) effectButton.SetActive(canUseEffect);
        }

        if (bgCloseButton != null) bgCloseButton.SetActive(true);
    }

    public void HideCardDetailAndButtons()
    {
        if (detailPanel != null) detailPanel.SetActive(false);
        if (actionButtonPanel != null) actionButtonPanel.SetActive(false);
        if (bgCloseButton != null) bgCloseButton.SetActive(false);

        selectedCardDisplay = null;
        selectedCardData = null;
    }

    /// <summary>
    /// UIに配置した「召喚ボタン」から呼び出すメソッド
    /// </summary>
    public void OnClickSummonButton()
    {
        if (selectedCardData == null) return;

        // GameManagerの召喚処理を実行
        if (GameManager.Instance.CanNormalSummon())
        {
            GameManager.Instance.ExecuteNormalSummon(selectedCardData);

            // 【重要】召喚に成功したら、手札のカードオブジェクト（GameObject）を削除する
            if (selectedCardDisplay != null)
            {
                Destroy(selectedCardDisplay.gameObject);
            }

            // UIを閉じる
            HideCardDetailAndButtons();
        }
        else
        {
            Debug.LogWarning("召喚権がない、または現在は召喚できません。");
        }
    }

    public void OnClickEffectButton()
    {
        if (selectedCardData == null) return;

        // 元のアクションボタン（召喚/効果）は一度非表示にする
        if (actionButtonPanel != null) actionButtonPanel.SetActive(false);

        // 効果選択パネルを表示
        if (effectSelectPanel != null) effectSelectPanel.SetActive(true);

        // 🔴 効果1が現在の状況（手札）で発動できるかチェック
        bool canUseEffect1 = (selectedCardData.triggerEnum1 == TriggerType.手札に居るとき);
        if (effect1SelectButton != null)
        {
            effect1SelectButton.gameObject.SetActive(canUseEffect1);
            if (canUseEffect1 && effect1ButtonText != null)
            {
                // ボタンの文字をCSVの効果テキストにする
                effect1ButtonText.text = selectedCardData.effectText1;
            }
        }

        // 🔴 効果2が現在の状況（手札）で発動できるかチェック
        bool canUseEffect2 = (selectedCardData.triggerEnum2 == TriggerType.手札に居るとき);
        if (effect2SelectButton != null)
        {
            effect2SelectButton.gameObject.SetActive(canUseEffect2);
            if (canUseEffect2 && effect2ButtonText != null)
            {
                // ボタンの文字をCSVの効果テキストにする
                effect2ButtonText.text = selectedCardData.effectText2;
            }
        }
    }

    /// <summary>
    /// UIに配置した「効果発動ボタン」から呼び出すメソッド
    /// </summary>
    public void OnSelectEffect1()
    {
        ExecuteSelectedEffect(1);
    }

    /// <summary>
    /// 効果2の選択ボタンが押されたとき（Unityのインスペクターからイベント登録します）
    /// </summary>
    public void OnSelectEffect2()
    {
        ExecuteSelectedEffect(2);
    }

    /// <summary>
    /// 実際に選ばれた番号の効果を処理する
    /// </summary>
    private void ExecuteSelectedEffect(int effectNumber)
    {
        if (selectedCardData == null) return;

        // 1. コスト支払い処理（共通化）
        string costTarget = (effectNumber == 1) ? selectedCardData.costTarget1 : selectedCardData.costTarget2;
        // 例: 「このカード」を墓地に送るコストの場合
        if (costTarget == "このカード" && CemeteryManager.Instance != null)
        {
            CemeteryManager.Instance.SendToCemetery(selectedCardData);
            if (selectedCardDisplay != null) Destroy(selectedCardDisplay.gameObject);
        }

        // 2. チェーンシステムに選んだ方の効果だけをトリガーとして伝える
        if (ChainSystem.Instance != null)
        {
            // 🔴 確実に選んだ方の効果だけを動かすため、新しく作った「AddChainSpecific」を呼ぶ（後述）
            ChainSystem.Instance.AddChainSpecific(selectedCardData, effectNumber);
        }

        // 全てのUIパネルを閉じる
        CloseAllPanels();
    }

    public void CloseAllPanels()
    {
        if (actionButtonPanel != null) actionButtonPanel.SetActive(false);
        if (effectSelectPanel != null) effectSelectPanel.SetActive(false);
        if (bgCloseButton != null) bgCloseButton.SetActive(false);
        selectedCardDisplay = null;
        selectedCardData = null;
    }
}