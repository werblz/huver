using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPopup : MonoBehaviour {


    [SerializeField]
    private Game_Manager gm = null;

    [SerializeField]
    private GameObject howToResetPopup = null;

    [SerializeField]
    private GameObject hasResetPopup = null;

    // Use this for initialization
    void OnEnable() {

        // By default, a player is coming back to a previous game, so make sure they know how to reset if they want.
        // Show the How To Reset dialog
        howToResetPopup.SetActive(true);
        hasResetPopup.SetActive(false);

        // However, if there IS no save game file (fresh game) OR you have reset the game on startup, show the Welcome dialog
        if ( gm.hasResetGame )
        {
            howToResetPopup.SetActive(false);
            hasResetPopup.SetActive(true);
        }

        if ( gm.hasNoSaveFile )
        {
            howToResetPopup.SetActive(false);
            hasResetPopup.SetActive(true);
        }
        


    }
	

}
