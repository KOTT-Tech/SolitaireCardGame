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

            //もし1列目（IDや名前）が空っぽだったら、その行は飛ばす
            if (string.IsNullOrEmpty(values[0])) continue; 

            // 新しいカードデータを作成し、値を代入（インデックスはExcelの列順に合わせる）
            CardData card = ScriptableObject.CreateInstance<CardData>();
            card.cardID = values[0]; // カードIDを取得
            card.actConType1 = values[1]; // カード効果(1)の発動条件
            card.actCostType1 = values[2]; // カード効果(1)の発動コストの種類
            card.actCost1 = values[3]; // カード効果(1)の発動コストの数
            card.effectType1 = values[4]; // カード効果(1)の効果内容の種類
            card.effect1 = values[5]; // カード効果(1)の効果内容の数
            card.actConType2 = values[6]; // カード効果(2)の発動条件
            card.actCostType2 = values[7]; // カード効果(2)の発動コストの種類
            card.actCost2 = values[8]; // カード効果(2)の発動コストの数
            card.effectType2 = values[9]; // カード効果(2)の効果内容の種類
            card.effect2 = values[10]; // カード効果(2)の効果内容の数

            deck.Add(card);
        }
        Debug.Log("読み込み完了！枚数: " + deck.Count);
    }
}
