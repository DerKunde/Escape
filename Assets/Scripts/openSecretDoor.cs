using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controlls the secret door, counts all found objects, if all objects are found, door is opend
public class openSecretDoor : MonoBehaviour
{
    
    public int filledSlots { set; get; }
    private bool doorOpend;
    
    
    void Start()
    {
        filledSlots = 0;
        doorOpend = false;

    }

   
    private void MoveDoor()
    {
       //Move Shelve to new Position to open Secret Door
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        transform.position = newPos;
        doorOpend = true;
    }
    

    void Update()
    {
       
        if (filledSlots == 4)
        {
            if (!doorOpend)
            {
                //open Door
                MoveDoor();
                SoundManager.PlayOpenSecretDoorSound();
            }
            
            
        }
    }
}
