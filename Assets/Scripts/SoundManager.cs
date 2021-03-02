using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    static AudioSource audioSource;
    static GameObject soundGameObject;
    static AudioClip openDoorSound;
    static AudioClip backgroundSound;
    static AudioClip heartmonitorSound;
    static AudioClip destroyCamSound;
    static AudioClip laserSound;
    static AudioClip doorLockedSound;
    static AudioClip destroyWindowSound;
    static AudioClip startBackgroundSound;
    static AudioClip birdSound;
    static AudioClip warpVirusSound;
    static AudioClip rotateCogwheelSound;
    static AudioClip openSecretDoorSound;
    static AudioClip unlockFrontdoorSound;
    static AudioClip carStartSound;

    //lädt alle Audioclips
    public static void LoadAudioClips()
    {
        openDoorSound = Resources.Load<AudioClip>("Audio/openDoorAudio");
        backgroundSound = Resources.Load<AudioClip>("Audio/backgroundSound");
        heartmonitorSound = Resources.Load<AudioClip>("Audio/heartmonitorSound");
        destroyCamSound = Resources.Load<AudioClip>("Audio/destroyCamSound");
        laserSound = Resources.Load<AudioClip>("Audio/laserSound");
        doorLockedSound = Resources.Load<AudioClip>("Audio/doorLockedSound");
        destroyWindowSound = Resources.Load<AudioClip>("Audio/destroyWindowSound");
        startBackgroundSound = Resources.Load<AudioClip>("Audio/startBackgroundSound");
        birdSound = Resources.Load<AudioClip>("Audio/birdSound");
        warpVirusSound = Resources.Load<AudioClip>("Audio/warpVirusSound");
        rotateCogwheelSound = Resources.Load<AudioClip>("Audio/rotateCogwheelSound");
        openSecretDoorSound = Resources.Load<AudioClip>("Audio/openSecretDoorSound");
        unlockFrontdoorSound = Resources.Load<AudioClip>("Audio/unlockFrontdoorSound");
        carStartSound = Resources.Load<AudioClip>("Audio/carStartVR");
    }



    public static void PlayCarStartSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(carStartSound);
    }

    //sound for opening door
    public static void PlayOpenDoorSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(openDoorSound);
    }

    //background sound
    public static void PlayBackgroundSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    //heartmonitor sound
    public static void PlayHeartmonitorSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = heartmonitorSound;
        audioSource.loop = true;
        audioSource.volume = 1f;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 3;
        audioSource.Play();
    }

    //sound for destroying the camera
    public static void PlayDestroyCamSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(destroyCamSound);
    }
    //sound for changing virus
    public static void PlayWarpVirusSound()
       {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(warpVirusSound);
    }
    //sound for combination lock
    public static void PlayRotateCogwheelSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(rotateCogwheelSound);
    }
    // sound for unlocking frontdoor
    public static void PlayUnlockFrontdoorSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(unlockFrontdoorSound);
    }
    // sound for opening secret door
    public static void PlayOpenSecretDoorSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(openSecretDoorSound);
    }


    //sound for locked door
    public static void PlayDoorLockedSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(doorLockedSound);
    }

    //sound for destroying the window
    public static void PlayDestroyWindowSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(destroyWindowSound);
    }

    //background sound for start scene
    public static void PlayStartBackgroundSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = startBackgroundSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    //bird sound
    public static void PlayBirdSound()
    {
        soundGameObject = new GameObject("Sound");
        audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = birdSound;
        audioSource.loop = true;
        audioSource.volume = 1f;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 3;
        audioSource.Play();
    }

}
