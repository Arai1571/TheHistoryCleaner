using UnityEngine;

[System.Serializable]
public class Message
{
    public string name;
    [TextArea(3, 10)]//最小3行、最大10行でstringを表示
    public string message;
}

[CreateAssetMenu(fileName = "MsgData", menuName = "MessageScripts/MessageData")]
public class MessageData : ScriptableObject
{
    public Message[] msgArray; //Messageクラスの配列
}


