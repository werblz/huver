using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Reset_Activator : MonoBehaviour {


    // Must reference Game_Manager in order to call a funciton on it to delete saved game file and restart
    [Tooltip("This script needs the Game Manager reference so it can call ResetGame on it.")]
    [SerializeField]
    private Game_Manager gm = null;

    [Tooltip("The Panel that will pop up after you reset.")]
    [SerializeField]
    private GameObject confirmPanel = null;

    [Tooltip("The Panel Parent so I can turn it off but not turn off this whole object, which needs to control reset")]
    [SerializeField]
    private GameObject activatorParent = null;

    [Tooltip("Taxi, because I need it to play sounds on it.")]
    [SerializeField]
    private Taxi_Controller taxi = null;

    private float aButtonForNewGame = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {

        aButtonForNewGame = Input.GetAxis("Fire1"); // The "A" Button

        if ( aButtonForNewGame > 0.10f)
        {
            activatorParent.SetActive(false);
            confirmPanel.SetActive(true);
            taxi.SoundUISelectionSuccess();
            gm.ResetGame();
        }

    }
}
