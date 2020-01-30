using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {
    public struct MessageItem
    {
        public string Message;
        public float Time;

        public MessageItem(string message, float time)
        {
            Message = message;
            Time = time;
        }
    }

    public enum State
    {
        NotRunning,
        Started,
        Finished
    }

    [SerializeField]
    Text Message;

    float Timer;
    int QueueIndex;
    MessageItem[] MessageQueue;

    State MyState;

	// Use this for initialization
	void Start () {
        Message.gameObject.SetActive(false);
        ResetQueue();
	}

    void ResetQueue()
    {
        this.QueueIndex = 0;
        MyState = State.NotRunning;
    }

    public bool IsState(State queryState)
    {
        return MyState == queryState;
    }
	
    public void SetQueueAndStart(MessageItem[] newQueue)
    {
        MessageQueue = newQueue;
        MyState = State.Started;
        Timer = 0;
        QueueIndex = 0;
        Message.gameObject.SetActive(true);
        SetTextToQueueItem(MessageQueue[QueueIndex]);
    }

    void SetTextToQueueItem(MessageItem queueItem)
    {
        Message.text = queueItem.Message;
    }

	// Update is called once per frame
	void Update () {
		if (MyState == State.Started)
        {
            // Check to see if the counter is done yet
            Timer = Timer + Time.deltaTime;
            if (Timer > MessageQueue[QueueIndex].Time)
            {
                // Increase the queue
                if (++QueueIndex > MessageQueue.Length - 1)
                {
                    OnFinishedQueue();                    
                } else
                {
                    SetTextToQueueItem(MessageQueue[QueueIndex]);
                    Timer = 0;
                }
            }
        }
	}

    void OnFinishedQueue()
    {
        Message.gameObject.SetActive(false);
        MyState = State.Finished;
    }
}
