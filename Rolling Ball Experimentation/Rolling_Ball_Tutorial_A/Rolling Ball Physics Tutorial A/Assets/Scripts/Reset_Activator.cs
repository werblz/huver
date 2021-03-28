using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Reset_Activator : MonoBehaviour {


    // Must reference Game_Manager in order to call a funciton on it to delete saved game file and restart
    [Tooltip("This script needs the Game Manager reference so it can call ResetGame on it.")]
    [SerializeField]
    private Game_Manager gm = null;

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
            gm.ResetGame();
        }

    }
}
