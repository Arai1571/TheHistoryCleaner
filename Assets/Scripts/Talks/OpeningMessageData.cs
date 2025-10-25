using UnityEngine;

[System.Serializable]
public class OpeningLine
{
    public string speaker;          // 話者名
    [TextArea(2, 5)] public string text; // 台詞
    public Sprite face;             // 表情スプライト
    public float zoomLevel = 1.0f;  // カメラズーム
    public AudioClip sfx;           // 効果音
    public float waitAfter = 0.5f;  // 次の台詞までの待機秒
}

[CreateAssetMenu(fileName = "NewOpening", menuName = "Opening/OpeningMessageData")]
public class OpeningMessageData : ScriptableObject
{
    public OpeningLine[] msgArray;
}
