using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPointer : MonoBehaviour
{
    public GameObject laser;
    public ParticleSystem ps;
    LineRenderer laserRenderer;
    public GameObject reticlePointer;
    public GameObject hands;
    private Camera mainCam;

    /**
     * laserrenderer wird festgelegt
     * spielt Hintergrundmusik ab
     */
    public void Awake()
    {
        laserRenderer = laser.GetComponent<LineRenderer>();
        laserRenderer.enabled = false;
        SoundManager.LoadAudioClips();
        SoundManager.PlayBackgroundSound();
    }

    /**
     * setzt Farbe des Laserstrahls
     * <param name="color">Farbe, die gesetzt werden soll</param>
     */
    public void setLaserColor(Color color)
    {
        if (laserRenderer != null)
        {
            laserRenderer.startColor = color;
            laserRenderer.endColor = color;
        }
    }

    /**
     * berechnet die Länge des Laserstrahls
     * <param name="pos">Position des Objektes, das angeguckt wird</param>
     * <param name="toHands">ob das Objekt aufgehoben wird</param>
     */
    public void setLaserLength(Vector3 pos, bool toHands)
    {
        float dist;
        if (toHands)
        {
            dist = Vector3.Distance(pos, hands.transform.position);
        }
        else
        {
            dist = Vector3.Distance(pos, laser.transform.position);
        }
        laserRenderer.SetPosition(1, new Vector3(0,0,dist));
        float laserStart = 0.4f;
        float angle;
        angle = Mathf.Atan(laserStart / dist);
        angle = angle * Mathf.Rad2Deg;
        laser.transform.localRotation = Quaternion.Euler(-angle, 0, 0);
    }

    /**
     * setzt Laserstrahl sichtbar
     * setzt Reticle Pointer unsichtbar
     * startet particle system
     * <param name="meshRenderer">Meshrenderer vom Objekt, das angeguckt wird</param>
     * <param name="pos">Position vom Objekt, das angeguckt wird</param>
     * <param name="size">Größe der particles</param>
     * <param name="color">Farbe der particles</param>
     * <param name="offset">Offset der particles</param>
     */
    public void setLaserVisible(MeshRenderer meshRenderer, Vector3 pos, float size, Color color, float offset)
    {
        var shape = ps.shape;
        var main = ps.main;
        laserRenderer.enabled = true;
        reticlePointer.GetComponent<MeshRenderer>().enabled = false;
        shape.normalOffset = offset;
        main.startSize = size;
        main.startColor = color;
        ps.Play();
        shape.meshRenderer = meshRenderer;
    }

    /**
     * setzt Laserstrahl unsichtbar
     * setzt Reticle Pointer sichtbar
     * stoppt particle system
     */
    public void setLaserInvisible()
    {
        reticlePointer.GetComponent<MeshRenderer>().enabled = true;
        laserRenderer.enabled = false;
        ps.Stop();
    }
}
