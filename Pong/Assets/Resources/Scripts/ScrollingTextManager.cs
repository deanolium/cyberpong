using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingTextManager : MonoBehaviour {
    public TextMeshProUGUI TMPScrollingText;
    public Transform ScrollingTextContainer;
    public float ScrollSpeed = 1000;
    public bool IsDone { get; private set; }
    public bool IsRunning = false;

    private float textWidth;
    private float scaling;
    private Vector3 startPosition;
    private float scrollPosition;

    private Vector3 curPosition;
    private float comparison;

	// Use this for initialization
	void Start () {
        scaling = FindObjectOfType<Canvas>().scaleFactor;

        startPosition = new Vector3(ScrollingTextContainer.position.x, ScrollingTextContainer.position.y, ScrollingTextContainer.position.z);
        textWidth = TMPScrollingText.preferredWidth;
        Reset();
	}

    private void Reset()
    {
        IsDone = false;
        scrollPosition = 0;
    }

    public void StartScroll()
    {
        Reset();
        IsRunning = true;
        TMPScrollingText.gameObject.SetActive(true);
    }

    void OnFinishScroll()
    {
        IsDone = true;
        IsRunning = false;
        ScrollingTextContainer.position = startPosition;
        TMPScrollingText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (!IsRunning) return;

        curPosition = new Vector3(startPosition.x - scrollPosition, startPosition.y, startPosition.z);
        ScrollingTextContainer.position = curPosition;

        scrollPosition += ScrollSpeed * Time.deltaTime;

        if (startPosition.x - scrollPosition < -textWidth * scaling)
        {
            OnFinishScroll();
        }
	}
}
