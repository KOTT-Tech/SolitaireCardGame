using System.Collections.Generic;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    // インスペクターからCSVファイルをドラッグ＆ドロップする
    public TextAsset csvFile;
    public List<CardData> deck = new List<CardData>();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        // CSVの内容を1行ごとに分割して配列に入れる
        string[] lines = csvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 1行目は見出しなので i = 1 からスタート
        for (int i = 1; i < lines.Length; i++)
        {
            // カンマで分割
            string[] values = lines[i].Split(',');

            // 🔴【修正点1】定義されている列数（18列）に満たない行はスキップ
            if (values.Length < 18) continue;

            // 🔴【修正点2】全18列の中に1つでも空文字（未入力）があればスキップする
            bool isAllFilled = true;
            for (int j = 0; j < 18; j++)
            {
                // トリム（文字の前後にある目に見えないスペースや改行コードを削除）して空チェック
                if (string.IsNullOrEmpty(values[j].Trim()))
                {
                    isAllFilled = false;
                    break; // 空欄が見つかったらこの行のチェックを終了
                }
            }

            // 1つでも空欄があれば、このカードデータは作成せずに次の行へ進む
            if (!isAllFilled) continue;


            // --- ここから下は、すべての列が埋まっている行だけが処理されます ---
            CardData card = ScriptableObject.CreateInstance<CardData>();
            
            card.cardID = int.Parse(values[0].Trim()); // カードID
            card.cardName = values[1].Trim(); // カード名
            card.conType1 = values[2].Trim(); // カード効果(1)の発動条件
            card.costTarget1 = values[3].Trim(); // カード効果(1)の発動コストの対象
            card.costTargetCount1 = int.Parse(values[4].Trim()); // カード効果(1)の発動コストの数
            card.costType1 = values[5].Trim(); // カード効果(1)の発動コストの種類
            card.effectTarget1 = values[6].Trim(); // カード効果(1)の効果対象の種類
            card.effectTargetCount1 = int.Parse(values[7].Trim()); // カード効果(1)の効果対象の数
            card.effectType1 = values[8].Trim(); // カード効果(1)の効果内容の種類
            card.effectText1 = values[9].Trim(); // 効果(1)のテキスト
            card.conType2 = values[10].Trim(); // カード効果(2)の発動条件
            card.costTarget2 = values[11].Trim(); // カード効果(2)の発動コストの対象
            card.costTargetCount2 = int.Parse(values[12].Trim()); // カード効果(2)の発動コストの数
            card.costType2 = values[13].Trim(); // カード効果(2)の発動コストの種類
            card.effectTarget2 = values[14].Trim(); // カード効果(2)の効果対象の種類
            card.effectTargetCount2 = int.Parse(values[15].Trim()); // カード効果(2)の効果対象の数
            card.effectType2 = values[16].Trim(); // カード効果(2)の効果内容の種類
            card.effectText2 = values[17].Trim(); // 効果(2)のテキスト

            // ▼ 文字列から日本語 enum への変換処理
            if (System.Enum.TryParse(card.conType1, out TriggerType t1)) card.triggerEnum1 = t1;
            if (System.Enum.TryParse(card.costType1, out CostType c1)) card.costEnum1 = c1;
            if (System.Enum.TryParse(card.effectType1, out EffectType e1)) card.effectEnum1 = e1;

            if (System.Enum.TryParse(card.conType2, out TriggerType t2)) card.triggerEnum2 = t2;
            if (System.Enum.TryParse(card.costType2, out CostType c2)) card.costEnum2 = c2;
            if (System.Enum.TryParse(card.effectType2, out EffectType e2)) card.effectEnum2 = e2;

            deck.Add(card);
        }
        Debug.Log("読み込み完了！不完全な行を除いた枚数: " + deck.Count);
    }
}