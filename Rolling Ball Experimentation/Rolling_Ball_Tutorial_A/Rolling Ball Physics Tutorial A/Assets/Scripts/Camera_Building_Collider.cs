using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Building_Collider : MonoBehaviour {


    private Building_Camera_Collision_Manager bmm = null;

    // GET READY TO DELETE THIS! 

        // It was a failed concept. I hoped the camera collider could manage the buildings better, but I had issues
        // so I switched it so buildings looked after collision with camera and did the right thing.
        // Still no proof I have that right yet. If I do, this script can go. For now it does nothing and is not referenced by any gameobject











    // As proof of concept, this works.
    // Right now, whenever the camera's collider sphere hits a building, it scales the building back to 1, 1, 1.
    // And when it leaves that building, it scales it back to its original scale.
    // Right now it flickers, because it is in a loop of scaling down, thus leaving the trigger, thus scaling it back, thus hitting it and scaling down, etc.
    // But this will soon be replaced by telling the building to use a different material
    // Which I think I can do thusly:
    // SerializeField SolidMat; TransMat;
    // Tell the renderer to swap the material to TransMat on trigger enter
    // Tell the renderer to swap the material to SolidMat on trigger exit

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Building")
        {
            Debug.LogError("<color=red> FUUUUCK!!! Name of the building I hit is " + other.gameObject.name + "</color>");
            // Get the Building_Material_Manager on this building. I hope this is right.
            bmm = other.gameObject.GetComponent<Building_Camera_Collision_Manager>();
            Debug.Log(" FOUND BMM = " + bmm.name);
            bmm.SetTransparent();

            Debug.Log("YIKES! YOU HIT A BUILDING BACK THERE!");

            // Test code: Make building tiny on collision, normalsize on not.
            // originalBuildingScale = other.gameObject.transform.localScale;
            // other.gameObject.transform.localScale = new Vector3 ( 1.0f, 1.0f, 1.0f);

            


        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Building")
        {

            bmm = other.gameObject.GetComponent<Building_Camera_Collision_Manager>();
            bmm.SetOpaque();


            Debug.Log("You left that building!");
            //other.gameObject.SetActive(true);
            //other.gameObject.transform.localScale = originalBuildingScale;
        }
        
    }






}
