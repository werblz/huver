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
    AudioClip[] HeyTaxi = null;

    [SerializeField]
    AudioClip[] Pad1 = null;

    [SerializeField]
    AudioClip[] Pad2 = null;

    [SerializeField]
    AudioClip[] Pad3 = null;

    [SerializeField]
    AudioClip[] Pad4 = null;

    [SerializeField]
    AudioClip[] Pad5 = null;

    [SerializeField]
    AudioClip[] Pad6 = null;

    [SerializeField]
    AudioClip[] Pad7 = null;

    [SerializeField]
    AudioClip[] Pad8 = null;

    [SerializeField]
    AudioClip[] Pad9 = null;

    private AudioSource taxiAudio = null;


    private void Start()
    {
        taxiAudio = taxi.GetComponent<AudioSource>();
        Debug.Log("<color=white>$%%$%$%$%$%$%$ </color> Audio Source = " + taxiAudio.name);
    }

    // Update is called once per frame
    public void TriggerAudio (int whichSound, int whichPerson ) {

        // This case statement should be replaced. Instead of a case and a bunch of arrays, make single ScriptableObjects for each person, and then use the
        // array of pad numbers. But for now, for testing, this is fine.


        switch (whichSound)
        {
            case 0:
                taxiAudio.PlayOneShot(HeyTaxi[whichPerson], .5f);
                break;
            case 1:
                taxiAudio.PlayOneShot(Pad1[whichPerson], .5f);
                break;
            case 2:
                taxiAudio.PlayOneShot(Pad2[whichPerson], .5f);
                break;
            case 3:
                taxiAudio.PlayOneShot(Pad3[whichPerson], .5f);
                break;
            case 4:
                taxiAudio.PlayOneShot(Pad4[whichPerson], .5f);
                break;
            case 5:
                taxiAudio.PlayOneShot(Pad5[whichPerson], .5f);
                break;
            case 6:
                taxiAudio.PlayOneShot(Pad6[whichPerson], .5f);
                break;
            case 7:
                taxiAudio.PlayOneShot(Pad7[whichPerson], .5f);
                break;
            case 8:
                taxiAudio.PlayOneShot(Pad8[whichPerson], .5f);
                break;
            case 9:
                taxiAudio.PlayOneShot(Pad9[whichPerson], .5f);
                break;
            default:
                break;
        }


                //taxiAudio.PlayOneShot(HeyTaxi[whichPerson], .5f); // TEST. Later, the HeyTaxi will be in a case statement, and the array number will be the person

	}
}
