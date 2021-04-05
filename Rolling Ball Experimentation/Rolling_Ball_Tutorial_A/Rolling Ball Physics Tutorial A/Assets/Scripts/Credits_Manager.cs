using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_Manager : MonoBehaviour {

    [Tooltip("Need the taxi so we can determine whether or not it isAtPad.")]
    [SerializeField]
    private Taxi_Controller taxi = null;

    [Tooltip("UI to bring up")]
    [SerializeField]
    private GameObject creditsPanel = null;

    [Tooltip("Which string for the button?")]
    [SerializeField]
    private string button = "Fire1";

	
	// Update is called once per frame
	void Update () {
	
        if ( taxi.isAtBuilding )
        {
            float creditButton = Input.GetAxis(button);
            if (creditButton > 0.01)
            {
                creditsPanel.SetActive(true);
            }
            else
            {
                creditsPanel.SetActive(false);
            }
                
        }
        else
        {
            creditsPanel.SetActive(false);
        }
        


	}
}
