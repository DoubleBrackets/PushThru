using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTextManager : MonoBehaviour
{
    public static DialogueTextManager instance;

    private List<string> messageQueue = new List<string>();

    public CanvasGroup dialogueGroup;
    public CanvasGroup skipText;
    public TextMeshProUGUI text;
    

    private bool isDisplayingMessage;

    private Coroutine currentCorout;
    private bool isWaitingForNext;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SkipKeyPressed();
        }
    }
    [ContextMenu("Test")]
    public void Test()
    {
        QueueMessage("SUPEr cALIFrig \n SO ON AND SO FORTTHHHH");
    }
    public void QueueMessage(string message)
    {
        messageQueue.Add(message);
        if(!isDisplayingMessage)
        {
            isDisplayingMessage = true;
            StartDisplaying();
        }
    }

    private void StartDisplaying()
    {
        dialogueGroup.alpha = 1;
        currentCorout = StartCoroutine(Corout_DisplayMessage());
    }

    private void StopDisplaying()
    {
        dialogueGroup.alpha = 0;
        isDisplayingMessage = false;
    }

    private IEnumerator Corout_DisplayMessage()
    {
        string message = messageQueue[0];
        int length = message.Length;
        string current = "";
        float interval = 0.05f;
        for(int x = 0; x < length;x++)
        {
            current += message[x];
            text.text = current;
            yield return new WaitForSeconds(interval);
        }
        isWaitingForNext = true;
        skipText.alpha = 1;
    }

    private void MoveToNext()
    {
        isWaitingForNext = false;
        skipText.alpha = 0;
        messageQueue.RemoveAt(0);
        if(messageQueue.Count == 0)
        {
            StopDisplaying();
        }
    }

    private void FastForwardText()
    {
        isWaitingForNext = true;
        skipText.alpha = 1;
        StopCoroutine(currentCorout);
        text.text = messageQueue[0];
    }

    private void SkipKeyPressed()
    {
        if(isWaitingForNext)
        {
            MoveToNext();
        }
        else
        {
            FastForwardText();
        }
    }
}