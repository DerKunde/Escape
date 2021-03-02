using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour
{
    /**
    * Script zur Steuerung des Zahlenschloss, hält die aktuellen Einstellungen fest und prüft,
    * ob die korrekte Kombination eingegeben wurde.
    * Falls ja, wird das Objekt geöffnet.
    */

    

    

    public GameObject player { get; }
    public GameObject rightDoorToOpen;
    public GameObject leftDoorToOpen;

    //result: aktuell angezeigte Zahl
    //correctCombination: Kombination zum Öffnen des Schlosses
    private int[] result, correctCombination;

    void Start()
    {
        // Anzeige in der Startposition
        result = new int[] { 1, 1, 1 };
        correctCombination = new int[] { 1, 6, 3 };

    }

    /**
     * bekommt das zuletzt gedrehte Rad und die neu eingestellte Zahl übergeben
     * speichert diese ab und vergleicht mit der korrekten Zahlenfolge.
     * Wird die korrekte Zahlenfolge eingestellt, wird die Tür geöffnet
     */
    public void CheckResults(string wheelName, int number)
    {

        switch (wheelName)
        {
            case "Wheel_1":
                result[0] = number;
                break;
            case "Wheel_2":
                result[1] = number;
                break;
            case "Wheel_3":
                result[2] = number;
                break;
        }

        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2])
        {
            Debug.Log("Opened!");
            SoundManager.PlayUnlockFrontdoorSound();
            rightDoorToOpen.GetComponent<openFrontdoor>().doorClosed = false;
            leftDoorToOpen.GetComponent<openFrontdoor>().doorClosed = false;
        }
    }


    public GameObject getPlayer() { return player; }

    public int GetNumber(string wheelName)
    {
        switch (wheelName) { 
            case "Wheel_1":
                return result[0];
            case "Wheel_2":
                 return result[1];
            case "Wheel_3":
                return result[2];
            default: return 0;
        }
    }

}
