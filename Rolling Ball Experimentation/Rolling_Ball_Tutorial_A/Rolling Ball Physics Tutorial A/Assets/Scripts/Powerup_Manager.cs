using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Manager : MonoBehaviour {

    [SerializeField]
    private Game_Manager gm = null;

    [SerializeField]
    private Taxi_Controller taxi = null;

    [Tooltip("Time in FixedUpdates.")]
    [SerializeField]
    private int countDownTime = 6000; // Fixed Updates until this object removes itself

    private int counter = 0;

    //[HideInInspector]
    public int buildingOwner = 0;

    private AudioClip clipDestruct = null;

	// Use this for initialization
	private void Start () {
        counter = countDownTime;
	}
	
	// Update is called once per frame
	private void FixedUpdate () {

        counter--;
        
        if ( counter <= 0 )
        {
            DestroyPowerup(); // This will be called when the timer goes off, but also when it's hit, and does something.
        }
        
		
	}

    private void DestroyPowerup()
    {
        Destroy(gameObject);
        gm.buildingHasPowerup[buildingOwner] = false; // Tell the Game Manager that the building no longer has a powerup
    }

    public void DoPowerup()
    {
        taxi.gas += 100.0f;
        DestroyPowerup();
    }


}
