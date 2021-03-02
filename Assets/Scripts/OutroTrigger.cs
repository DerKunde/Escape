using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroTrigger : MonoBehaviour
{

    public string sceneToLoad;

    public LevelChanger lvlChanger;
    public GameObject Taxi;
    public GameObject inventory;
    public GameObject hands;
    private float pos = 0.3f;

    /**
     * Überprüft ob der Spieler sich in den Auslöser bewegt hat.
     * Ruft den LevelChanger der Scene auf und läd das eingestellte Level.
     */
    public void OnTriggerEnter(Collider other)
    {
            Taxi.gameObject.GetComponent<Animator>().enabled = true;
            Taxi.gameObject.GetComponent<Animator>().speed = 0.2f;
            SoundManager.PlayCarStartSound();
            MoveObjToScene();

            lvlChanger.FadeToLevel(sceneToLoad);

            StartSceneManager.endScene = true;
    }

    /**
     * überprüft, ob sich noch Objekte im Inventar oder in der Hand befinden
     * wenn ja, werden sie mit in die nächste Scene geladen
     * 
     */
    private void MoveObjToScene()
    {
        foreach (Transform child in inventory.transform)
        {
            if (child.transform.childCount == 3)
            {
                GameObject go = child.transform.GetChild(2).gameObject;
                go.transform.SetParent(null);
                go.transform.position = new Vector3(pos, 0.6f, 1.6f);
                go.transform.localScale = go.transform.localScale * 2;
                DontDestroyOnLoad(go);
                pos -= 0.2f;
            }
        }
        if(hands.transform.childCount != 0)
        {
            GameObject go = hands.transform.GetChild(0).gameObject;
            go.transform.SetParent(null);
            go.transform.position = new Vector3(pos, 0.6f, 1.6f);
            go.transform.localScale = go.transform.localScale * 0.5f;
            DontDestroyOnLoad(go);
        }
    }
}
