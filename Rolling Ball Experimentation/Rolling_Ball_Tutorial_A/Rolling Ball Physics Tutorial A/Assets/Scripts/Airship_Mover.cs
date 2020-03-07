using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Airship_Mover : MonoBehaviour {


    // All this is handled by Game_Manager now.
    // This now JUST turns ship on and sets the animator playing at a random timeline point
    [SerializeField]
    private Vector3 airshipStartPosition = Vector3.zero;

    //[SerializeField]
    //private float cityRadius = 500.0f;

    [SerializeField]
    public GameObject rotator = null;

    //public float yOffset = 390.0f;

    //public float xOffset = 500.0f;

    [SerializeField]
    public float airshipHeight = 10.0f;

    [SerializeField]
    public Rigidbody airshipRB = null;

    [SerializeField]
    private Collider airshipCollider = null;

    public bool collided = false;

    [SerializeField]
    public Animator anim = null;

    
    private RaycastHit hit;

    // Use this for initialization...
    void Start () {


        //PlaceShip();
		
	}


    public void PlaceShip(Vector3 pos)
    {
        // Just offset in the X
        // No. Let the GameManager do this.
        //float xOffset = (Random.value * cityRadius * 2.0f) - cityRadius;
        // Not using yOffset ecuase GameManager wants to place it vertically, so it can
        // set each ship in its own vertical lane
        //transform.localPosition = new Vector3(xOffset, 0.0f, 0.0f);

        transform.position = pos;

        // Let's try this Raycast thing:
        Vector3 p1 = transform.position; // Position of the airship Mover
        float distanceToObstacle = 0.0f;
        string whatTagDidIHit = tag;
        float distanceToCast = 1000.0f;

        if (Physics.SphereCast(p1, airshipHeight / 2, transform.forward, out hit, distanceToCast ))
        {
            distanceToObstacle = hit.distance; // How far did it hit?
            Debug.Log("<color=blue>SHIP COLLIDED with "
                + hit.collider + " at " +
                + distanceToObstacle + "!</color>");
        }

        // For now, do not let the ships  rotate themselves
        //rotator.transform.eulerAngles = new Vector3(0.0f, Random.value * 360.0f, 0.0f);
        

        RestartAnimation();
    
    }

    // Tells the ship to restart. 
    // This is its own method becuase Airship_Event_Driver needs to reset the ship
    // when the animation gets to its end.
    public void RestartAnimation()
    {
        enabled = true;
        anim.Play("Airship_Move_Z", -1, Random.value);
        airshipCollider.enabled = false;

    }

}
