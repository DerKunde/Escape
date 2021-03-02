using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class teleporting : MonoBehaviour
{
    //Script für die Teleportationspunkte
    //Ein Punkt kann erreicht werden, wenn er im Sichtfeld liegt.

    private Color inactive = new Color(0.39f, 0.39f, 0.39f, 0.36f);
    private Color gazedAt=new Color(0.02f,0.96f,0.91f,1f);
    private MeshRenderer myRenderer;
    private float myTimer = 0;
    private float waitingTime = 1;
    private GameObject player; //Verweis auf den Spieler

    private bool teleport = false;
    private bool isEmpty = true;

    private void Start()
    {
        //player = GameObject.FindWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material.color = inactive;
        isEmpty = (!(player.transform.position.x == transform.position.x) || !(player.transform.position.y == transform.position.y));
        
    }

    /**
     * 
     * für die Blicksteuerung 
     */
    public void gazeAt(bool gazing)
    {
            if (gazing)
            {
                teleport = true;
            }
            else
            {
                myTimer = 0;

                myRenderer.material.color = inactive;
                teleport = false;
            }

    }
    
 

    void Update()
    {

        isEmpty = (!(player.transform.position.x == transform.position.x) || !(player.transform.position.y == transform.position.y));
        if (teleport && isEmpty)
            {
                myTimer += Time.deltaTime;
                //linearer Farbwechsel
                myRenderer.material.color = Color.Lerp(gazedAt, inactive, (waitingTime - myTimer) / waitingTime);

                if (myTimer >= waitingTime)
                {
                    //Position an den Player übergeben
                    Vector3 pos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                    player.transform.position = pos;
         
                    isEmpty = false;
                    teleport = false;

                }
            }
        }
    
}
