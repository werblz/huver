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


        if ( gm.isNewGame )
        {
            Debug.Log("<color=yellow>****************</color><color=blue>***************</color> SHIFT = " + gm.shift);
            howToResetPopup.SetActive(false);
            hasResetPopup.SetActive(true);
        }
        


    }
	

}
