using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing_Pad_Sound_Pitch : MonoBehaviour {

    
    private Pad_Manager pm = null;

    private AudioSource padAudio = null;
    

	// Use this for initialization
	void Start () {

        pm = GetComponentInParent<Pad_Manager>();
        Debug.LogWarning("<color=white>************************************** PAD NUMBER IS " + pm.padNumber + "</color>");
        padAudio = GetComponent<AudioSource>();
        Debug.LogWarning("<color=white>************************************** AUDIO SOURCE " + padAudio.name + "</color>");

        padAudio.pitch = 1.0f + (pm.padNumber * .05f);
    }
	

}
