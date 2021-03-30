using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Manager : MonoBehaviour
{

    // This script does ONE thing. It is active when UI is UP only, so I should be able to rely on it doing nothing unless the UI panel is up.
    // IF the UI panel is up, this script will be active, and on LateUpate, will check for left joystick left, and if it detects it, will activate the activator.
    // That's it. It does nothing else.
    // But then when the activator is active, IT will have a script on it that chesk for the X button, and resets game if it finds it.

    // moveSideways is the left joystick left

    // The reset dialog activator. 
    [Tooltip("This script needs Reset_Activator object, so it can activate it when Left Joystick is left")]
    [SerializeField]
    private GameObject resetActivator = null;

    [Tooltip("Need the Taxi as reference so I can call a sound method that exists in the Taxi_Controller")]
    [SerializeField]
    private Taxi_Controller taxi = null;

    // The string to control the Left Joystick Left/Right
    private string sideJoy = "Horizontal";
    private float joyToleranceSideMax = 0.6f;
    private float joyToleranceSideMin = -0.6f;
    private bool pressedOnce = false;

    // Use this for initialization
    void Start () {
        pressedOnce = false;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float moveSideways = Input.GetAxis(sideJoy);

        if ( moveSideways < joyToleranceSideMin && ( resetActivator.activeSelf == false) )
        {
            resetActivator.SetActive(true);
            pressedOnce = true;
        }

        if ( moveSideways > joyToleranceSideMin )
        {
            resetActivator.SetActive(false);
        }

        if ( pressedOnce == true )
        {
            taxi.SoundUISelectionChange();
            pressedOnce = false;
        }
        
    }
}
