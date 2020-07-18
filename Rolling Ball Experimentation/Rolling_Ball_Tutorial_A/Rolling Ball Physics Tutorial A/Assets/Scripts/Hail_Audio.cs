using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hail_Audio : MonoBehaviour {

    [SerializeField]
    Game_Manager gm = null;

    [SerializeField]
    Taxi_Controller taxi = null;

    // I could simply randomize all of this, but I want to make sure all of the arrays are in order of person.
    // The reason is I want the pad to time down a bit, and if it is taking too long for the taxi to arrive, I want it to yell Hey Taxi in the current
    // person's voice.

    // Later replace this with Scriptable Objects?

    [SerializeField]
    public AudioClip[] HeyTaxi = null;

    [SerializeField]
    private AudioClip[] Pad1 = null;

    [SerializeField]
    private AudioClip[] Pad2 = null;

    [SerializeField]
    private AudioClip[] Pad3 = null;

    [SerializeField]
    private AudioClip[] Pad4 = null;

    [SerializeField]
    private AudioClip[] Pad5 = null;

    [SerializeField]
    private AudioClip[] Pad6 = null;

    [SerializeField]
    private AudioClip[] Pad7 = null;

    [SerializeField]
    private AudioClip[] Pad8 = null;

    [SerializeField]
    private AudioClip[] Pad9 = null;

    private AudioSource taxiAudio = null;


    private void Start()
    {
        //taxiAudio = taxi.GetComponent<AudioSource>();
        taxiAudio = taxi.voiceAudio;
        Debug.Log("<color=white>$%%$%$%$%$%$%$ </color> Audio Source = " + taxiAudio.name);
    }
    

    // Update is called once per frame
    public void TriggerAudio (int whichSound, int whichPerson, float volume, float pitch, float delay ) {

        // If Game Manager passes a number outisde the array, just return. This assumes I keep all arrays the same size. I should
        // Also put checks below when I try to actually play it.
        if (whichPerson > HeyTaxi.Length)
        {
            return;
        }

        // This case statement should be replaced. Instead of a case and a bunch of arrays, make single ScriptableObjects for each person, and then use the
        // array of pad numbers. But for now, for testing, this is fine.

        StartCoroutine(ExecuteAfterTime(whichSound, whichPerson, volume, pitch, delay));

        
    }

    void PlaySound(AudioClip sound, float volume)
    {
        taxiAudio.PlayOneShot(sound, volume);
    }

    IEnumerator ExecuteAfterTime(int whichSound, int whichPerson, float volume, float voicePitch, float time)
    {
        

        yield return new WaitForSeconds(time);

        // Code to execute after the delay

        taxiAudio.pitch = voicePitch;

        Debug.Log("<color=orange>*******</color><color=cyan>******</color> Pitch = " + voicePitch);

        switch (whichSound)
        {
            case 0:
                PlaySound(HeyTaxi[whichPerson], volume);
                break;
            case 1:
                PlaySound(Pad1[whichPerson], volume);
                break;
            case 2:
                PlaySound(Pad2[whichPerson], volume);
                break;
            case 3:
                PlaySound(Pad3[whichPerson], volume);
                break;
            case 4:
                PlaySound(Pad4[whichPerson], volume);
                break;
            case 5:
                PlaySound(Pad5[whichPerson], volume);
                break;
            case 6:
                PlaySound(Pad6[whichPerson], volume);
                break;
            case 7:
                PlaySound(Pad7[whichPerson], volume);
                break;
            case 8:
                PlaySound(Pad8[whichPerson], volume);
                break;
            case 9:
                PlaySound(Pad9[whichPerson], volume);
                break;
            default:
                PlaySound(HeyTaxi[whichPerson], volume);
                break;
        }

    }

}
