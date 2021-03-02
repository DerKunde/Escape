using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class InteractiveButton : MonoBehaviour
{
    public Canvas canvas;
    public Canvas videoCanvas;
    private VideoPlayer videoPlayer;
    private bool useButton = false;
    private float myTimer = 0;
    public float waitingTime = 1;
    public GameObject playButton;
    public bool play { get; set; }

    // Start is called before the first frame update
    /**
     * init needed components
     */
    void Start()
    {
        videoCanvas.enabled = false;
        videoPlayer = videoCanvas.GetComponentInChildren<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonFunction();
    }

    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            useButton = true;
            
        }
        else
        {
            useButton = false;
        }

    }


    /**
     * Video startet
     * Button wird inaktiv gesetzt
     */
    private void ButtonFunction()
    {
        if (useButton)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                playButton.SetActive(false);
                videoCanvas.enabled = true;
                videoPlayer.Play();
                useButton = false;
                play = true;
            }
        }
    }
}
