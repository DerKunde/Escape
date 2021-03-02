using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool saveObj;
    private float myTimer = 0;
    public float waitingTime = 2;
    private GameObject obInHand;
    private GameObject hands;
    private GameObject slot;
    private bool addOb = false;
    private bool takeOb = false;
    private bool moveOb = false;
    private bool taking = false;

    private Vector3 ogSize;
    private GameObject player;

    private laserPointer laserPointer;

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * überprüft, ob der Inventarplatz frei ist und in der Hand ein Objekt ist
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, false);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.grey, 0.1f);
            laserPointer.setLaserColor(Color.grey);
            if (slot.transform.childCount == 2 && hands.transform.childCount == 1)
            {
                obInHand = hands.transform.GetChild(0).gameObject;
                ogSize = obInHand.transform.localScale;
                moveOb = true;
                addOb = true;
            }
            else if(slot.transform.childCount == 3 && hands.transform.childCount == 0 && !taking)
            {
                taking = true;
                moveOb = true;
                takeOb = true;
            }
        }
        else
        {
            myTimer = 0;
            addOb = false;
            laserPointer.setLaserInvisible();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slot = transform.parent.gameObject;
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
    }

    // Update is called once per frame
    /**
     * Objekt aud der Hand wird ins Inventar gelegt
     */
    void Update()
    {
        if (moveOb)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {
                if (addOb)
                {
                    obInHand.transform.localScale = ogSize/4;
                    obInHand.transform.SetParent(slot.transform);
                    obInHand.transform.localPosition = new Vector3(0, 0, 0);
                    addOb = false;
                }
                else if (takeOb)
                {
                    obInHand.transform.localScale = ogSize;
                    obInHand.transform.SetParent(hands.transform);
                    obInHand.transform.localPosition = new Vector3(0, 0, 0);
                    takeOb = false;
                    taking = false;
                }

                moveOb = false;
            }

        }
    }
}
