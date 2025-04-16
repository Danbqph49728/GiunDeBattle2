using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendBtn;
    public TextMeshProUGUI segContent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sendBtn.onClick.AddListener(SendMessage);
    }

    void SendMessage()
    {
        string message = inputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            ChatManager.instance.SendNuteMessage(message);
            inputField.text = "";
        }
    }
}
