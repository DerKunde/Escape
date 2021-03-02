using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{
    public bool canDestroyCam;

    private float myTimer = 0;
    public float waitingTime = 2;
    private GameObject hands;
    private bool moveOb = false;
    private GameObject player;

    private Rigidbody objectRb;

    private bool isLerping;
    private float startTime;
    private float lerpTime;

    private Vector3 startPos;
    private Vector3 endPos;

    private laserPointer laserPointer;
    public float offset = 0.5f;
    public float size = 1;

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * überprüft, ob die Hand leer ist und kein Objekt gerade aufgehoben wird
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
    public void gazeAt(bool gazing)
    {
        
        if (gazing && objectRb.IsSleeping())
        {
            laserPointer.setLaserLength(transform.position, true);
            laserPointer.setLaserVisible(GetComponent<MeshRenderer>(), transform.position, size, Color.blue, offset);
            laserPointer.setLaserColor(Color.blue);
            if (hands.transform.childCount == 0 && !isLerping)
            {
                moveOb = true;
            }
        }
        else
        {
            myTimer = 0;
            moveOb = false;
            if (!isLerping)
            {
                laserPointer.setLaserInvisible();
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
    }

    // Update is called once per frame
    /**
     * dieses Objekt wird aufgehoben
     */
    void Update()
    {

        if (moveOb)
        {
            myTimer += Time.deltaTime;
            if (myTimer >= waitingTime)
            {   
                StartLerping();
                moveOb = false;
            }
        }
        
    }

    /**
     * setzt Daten zum Bewegen des Objekts fest
     */
    void StartLerping()
    {
        isLerping = true;
        startTime = Time.time;

        startPos = transform.position;
        endPos = hands.transform.position;

        objectRb.useGravity = false;
        objectRb.detectCollisions = false;

        float dist = Vector3.Distance(startPos, endPos);

        lerpTime = dist * 0.3f;
    }

    /**
     * bewegt Objekt in die Hand des Players
     */
    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / lerpTime;

            transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

            laserPointer.setLaserLength(transform.position, true);

            if (percentageComplete >= 1.0f)
            {
                isLerping = false;
                transform.SetParent(hands.transform);
                transform.localPosition = new Vector3(0, 0, 0);
                laserPointer.setLaserInvisible();
            }
        }
    }
}
