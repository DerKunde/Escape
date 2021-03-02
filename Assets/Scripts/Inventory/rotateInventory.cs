using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateInventory : MonoBehaviour
{
    float camYRot;
    float camXRot;

    // Update is called once per frame
    /**
     * wenn der Spieler sich dreht und nicht ins Inventar guckt, 
     * dreht sich das Inventar ab einem bestimmten Grad mit
     */
    void Update()
    {
        camYRot = Camera.main.transform.eulerAngles.y;
        camXRot = Camera.main.transform.eulerAngles.x;
        if ((camYRot < 45 || camYRot > 315) && camXRot < 40)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (camYRot > 45 && camYRot < 135 && camXRot < 40)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if(camYRot > 135 && camYRot < 225 && camXRot < 40)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if(camYRot > 225 && camYRot < 315 && camXRot < 40)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

}
