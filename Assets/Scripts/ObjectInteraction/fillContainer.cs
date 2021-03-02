using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * FillContainer funktioniert wie "destroyWindow".
 * Wird ein passender Gegenstand auf einen leeren Container geworfen, wird dieser durch einen gefüllten ausgetauscht.
 * Hat die Referenzen auf Container und das zu werfende Objekt, sowie die "SecretDoor", die am Ende geöffnet werden soll.
 */
public class fillContainer : MonoBehaviour
{   
    public GameObject secretDoor;
    public GameObject filledConainer;
    private GameObject acceptedObject1, acceptedObject2, acceptedObject3, acceptedObject4;
    private bool fill;
    private float myTimer = 0;
    private float waitingTime = 1;

    private GameObject player;
    private GameObject hands;
    private laserPointer laserPointer;
    private GameObject throwObject;
    private Rigidbody throwObjectRb;

    private bool isLerping;
    private float startTime;
    private float lerpTime;
    private Vector3 startPos;
    private Vector3 endPos;



    public void gazeAt(bool gazing)
    {
        if (gazing)
        {
            laserPointer.setLaserLength(transform.position, true);
            laserPointer.setLaserVisible(transform.GetChild(1).GetComponent<MeshRenderer>(), transform.position, 0.5f, Color.red, 0.1f);
            laserPointer.setLaserColor(Color.red);
            if (hands.transform.childCount == 1)
            {

                throwObject = hands.transform.GetChild(0).gameObject;
                throwObjectRb = throwObject.GetComponent<Rigidbody>();
                //Nur wenn ein Virus geworfen wird
                if (throwObject.Equals(acceptedObject1) || throwObject.Equals(acceptedObject2) || throwObject.Equals(acceptedObject3) || throwObject.Equals(acceptedObject4))
                { fill = true; }
            }
        }
        else
        {
            myTimer = 0;
            laserPointer.setLaserInvisible();
        }

    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
        acceptedObject1 = GameObject.Find("Virus1");
        acceptedObject2 = GameObject.Find("Virus2");
        acceptedObject3 = GameObject.Find("Virus3");
        acceptedObject4 = GameObject.Find("Virus4");

    }

    
    void Update()
    {
        if (fill)
        {
            myTimer += Time.deltaTime;

            if (myTimer >= waitingTime)
            {
                throwObject.transform.SetParent(null);
                SoundManager.PlayWarpVirusSound();
                StartLerping();

                Vector3 oldPos = transform.position;
                Instantiate(filledConainer, oldPos, Quaternion.identity);
                //Erhöhe den Zähler von "SecretDoor" um eins
                secretDoor.GetComponent<openSecretDoor>().filledSlots += 1;
                fill = false;
            }
        }
    }

 
    /**
     * Lerping(lineare Interpolation) für die "Flugkurve" des Gegenstands, der geworfen wird
     */
    void StartLerping()
    {
        isLerping = true;
        startTime = Time.time;

        startPos = throwObject.transform.position;
        endPos = transform.position;

        throwObjectRb.useGravity = false;

        float dist = Vector3.Distance(startPos, endPos);

        lerpTime = dist * 0.05f;
    }
    
    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / lerpTime;

            throwObject.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            if (percentageComplete >= 1.0f) //Lerping vollständig
            {
                Destroy(gameObject);
                Destroy(throwObject);
                laserPointer.setLaserInvisible();
                isLerping = false;
            }
        }
    }
}
