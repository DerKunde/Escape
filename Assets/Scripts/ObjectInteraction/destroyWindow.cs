using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyWindow : MonoBehaviour
{
    public GameObject fractured;
    bool destroy;
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

    /**
     * gazeAt wird aufgerufen, wenn man dieses Objekt (this.gameObject) anguckt
     * zeigt Laserstrahl
     * überprüft, ob in der Hand ein Objekt ist
     * <param name="gazing">ob der Spieler auf dieses Objekt guckt</param>
     */
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
                destroy = true;
            }

        }
        else
        {
            myTimer = 0;
            laserPointer.setLaserInvisible();
        }

    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        hands = player.transform.Find("Main Camera/Hands").gameObject;
        laserPointer = player.GetComponent<laserPointer>();
    }

    // Update is called once per frame
    /**
     * Objekt aus der Hand wird gegen das Fenster geworfen
     */
    void Update()
    {
        if (destroy)
        {
            myTimer += Time.deltaTime;

            if (myTimer >= waitingTime)
            {
                throwObject.transform.SetParent(null);
                throwObjectRb.detectCollisions = true;
                StartLerping();

                Vector3 oldPos = transform.position;
                Instantiate(fractured, oldPos, fractured.transform.rotation);
                destroy = false;
            }
        }
    }

    /**
     * wenn das geworfene Objekt mit der Kamera kollidiert,
     * wird ein Geräusch angespielt
     * <param name="collision">collision</param>
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (throwObjectRb != null)
        {
            SoundManager.PlayDestroyWindowSound();
            throwObjectRb.useGravity = true;
        }
    }

    /**
     * setzt Daten zum Bewegen des Objekts fest
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

    /**
     * Objekt wird gegen das Fenster geworfen
     */
    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / lerpTime;

            throwObject.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);
            if (percentageComplete >= 1.0f)
            {
                Destroy(gameObject);
                throwObjectRb.useGravity = true;
                isLerping = false;
                laserPointer.setLaserInvisible();
            }
        }
    }
}
