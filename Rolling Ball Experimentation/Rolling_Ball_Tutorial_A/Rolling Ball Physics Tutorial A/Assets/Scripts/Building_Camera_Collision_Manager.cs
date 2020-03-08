using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Camera_Collision_Manager : MonoBehaviour {

    // New plan. Each building will have another mesh with the proper materials already on it.

    [SerializeField]
    private MeshRenderer opaqueBuilding = null;

    [SerializeField]
    private MeshRenderer transBuilding = null;

    


   

    // Use this for initialization
    void Start ()
    {
        SetOpaque();
	}

    public void SetTransparent()
    {
        transBuilding.enabled = true;
        opaqueBuilding.enabled = false;
    }

    public void SetOpaque()
    {
        transBuilding.enabled = false;
        opaqueBuilding.enabled = true;
    }


    /* 
     * 
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CameraCollider")
        {
            SetTransparent();
            Debug.Log("YIKES! YOU HIT A BUILDING BACK THERE!");

            // Test code: Make building tiny on collision, normalsize on not.
            // originalBuildingScale = other.gameObject.transform.localScale;
            // other.gameObject.transform.localScale = new Vector3 ( 1.0f, 1.0f, 1.0f);
            
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "CameraCollider")
        {

            SetOpaque();

            Debug.Log("You left that building!");
            //other.gameObject.SetActive(true);
            //other.gameObject.transform.localScale = originalBuildingScale;
        }

    }

    */
}
