using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private int levelToLoadIdx;

    private string levelToLoadName;

    private bool fadeout;
    private bool fadein;
    private float myTimer = 0;
    private float waitingTime = 3;
    public Color black = new Color(0f, 0f, 0f, 1f);
    public Color transparent = new Color(0f, 0f, 0f, 0f);

    /**
     * Aktiviert die Animation für den Übergang
     * levelName: Name der zu ladenen Scene.
     */ 
    public void FadeToLevel(string levelName)
    {
        levelToLoadName = levelName;
        fadeout = true;
        
    }

    private void Update()
    {
        if (fadeout)
        {
            myTimer += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(black, transparent, (waitingTime - myTimer) / waitingTime);

            if (myTimer >= waitingTime)
            {
                SceneManager.LoadScene(levelToLoadName);
                fadeout = false;
                myTimer = 0;
            }
        }
        if (fadein)
        {
            myTimer += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(transparent, black, (waitingTime - myTimer) / waitingTime);

            if (myTimer >= waitingTime)
            {
                fadein = false;
                myTimer = 0;
            }
        }
    }

    private void Start()
    {
        fadein = true;
    }
}
