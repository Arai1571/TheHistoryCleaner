using UnityEngine;

[System.Serializable]
public class Message
{
    public string name;
    [TextArea(3, 10)]//最小3行、最大10行でstringを表示
    public string message;
    public Sprite face;
    public AudioClip sfx;
    public float zoomLevel;
}

[CreateAssetMenu(fileName = "MsgData", menuName = "MessageScripts/MessageData")]
public class MessageData : ScriptableObject
{
    public Message[] msgArray; //Messageクラスの配列
}


