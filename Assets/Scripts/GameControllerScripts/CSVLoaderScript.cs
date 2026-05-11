using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVLoader : MonoBehaviour
{
    // インスペクターからCSVファイルをドラッグ＆ドロップする
    public TextAsset csvFile;
    public List<CardData> deck = new List<CardData>();

    void Start()
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

            bool isAllFilled = true; // すべて埋まっているかどうかのフラグ

            for (int j = 0; j < values.Length; j++)
            {
                if (string.IsNullOrEmpty(values[j]))
                {
                    isAllFilled = false; // 1つでも空があれば「未完成」にする
                    break; // これ以上チェックする必要はないのでループを抜ける
                }
            }

            if (!isAllFilled) continue;

            // 新しいカードデータを作成し、値を代入（インデックスはExcelの列順に合わせる）
            CardData card = ScriptableObject.CreateInstance<CardData>();
            card.cardID = int.Parse(values[0]); // カードID
            card.cardName = values[1]; // カード名
            card.conType1 = values[2]; // カード効果(1)の発動条件
            card.costTarget1 = values[3]; // カード効果(1)の発動コストの対象
            card.costTargetCount1 = int.Parse(values[4]); // カード効果(1)の発動コストの数
            card.costType1 = values[5]; // カード効果(1)の発動コストの種類
            card.effectTarget1 = values[6]; // カード効果(1)の効果対象の種類
            card.effectTargetCount1 = int.Parse(values[7]); // カード効果(1)の効果対象の数
            card.effectType1 = values[8]; // カード効果(1)の効果内容の種類
            card.effectText1 = values[9]; // 効果(1)のテキスト
            card.conType2 = values[10]; // カード効果(2)の発動条件
            card.costTarget2 = values[11]; // カード効果(2)の発動コストの対象
            card.costTargetCount2 = int.Parse(values[12]); // カード効果(2)の発動コストの数
            card.costType2 = values[13]; // カード効果(2)の発動コストの種類
            card.effectTarget2 = values[14]; // カード効果(2)の効果対象の種類
            card.effectTargetCount2 = int.Parse(values[15]); // カード効果(2)の効果対象の数
            card.effectType2 = values[16]; // カード効果(2)の効果内容の種類
            card.effectText2 = values[17]; // 効果(2)のテキスト

            deck.Add(card);
        }
        Debug.Log("読み込み完了！枚数: " + deck.Count);
    }
}
