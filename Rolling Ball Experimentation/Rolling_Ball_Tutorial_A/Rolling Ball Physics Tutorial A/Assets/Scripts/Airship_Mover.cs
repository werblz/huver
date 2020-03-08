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

    // Pass in the position to try placing the ship, and its rotation.
    // We need to know the rotation so the sphere cast can work in the right direction.
    // But it may be hard to determine that without knowing i, the index of the ship array
    // so we can rotate it here
    public void PlaceShip(Vector3 pos, int i)
    {
        // Distance to cast the sphere cast. LONG
        float distanceToCast = 10000.0f;

        // Rotate each ship 90 degrees more than previous in the passing array
        float rot = i * 90.0f;

        Vector3 castRotation = Vector3.forward; // Set up our cast rotation to forward
        // So we can, at every 90 degree turn, change it to left, then back to forward.
        // Because I'm convinced the spher cast is going down the same axis regardless of the ship rotation

        // Just a stat to see how often we have to move the ship before it clears the buildings
        int tries = 0;

        // Just offset in the X
        // No. Let the GameManager do this.
        //float xOffset = (Random.value * cityRadius * 2.0f) - cityRadius;
        // Not using yOffset ecuase GameManager wants to place it vertically, so it can
        // set each ship in its own vertical lane
        //transform.localPosition = new Vector3(xOffset, 0.0f, 0.0f);

        bool clearedBuildings = false;




        // This code does two things:
        // It fixes castRotation so the SphereCast goes outward in the right direction
        // And it puts every odd ship one airshipHeight higher, so the cross grid can never collide.
        if (i % 2 == 1) // Check which ship in the array, mod it by 2 to see if odd or even,
                        // so we can set the cast rotation 
        {
            castRotation = Vector3.right;

        }
        else
        {
            castRotation = Vector3.forward;
            pos.y = pos.y + airshipHeight;
        }



        // Place ship at first position. This will update in the while loop if it collides.
        transform.position = pos;

        // Rotate ship based on this particular ship in the array's Y rotation, which was passed
        // in to this method. The Game_Manager rotates each ship 90 degrees from the last, so
        // we always get the next ship rotated
        rotator.transform.eulerAngles = new Vector3(0.0f, rot, 0.0f);

        // Let's try this Raycast thing:

        


        // Let's do this in baby steps.
        // Put in a while loop that checks flag: clearedBuildings which starts out false
        // Next, place the ship regardless. Just debugspew the collision.
        //   And set clearedBuildings to true so the loop will stop.
        //   This should result in exactly the same behavior as before. Ships too low
        // Then, After the Debug, actually raise the ship up. The while loop should continue to run
        // until clearedBuildings is set to true, which we will ONLY do if no collision

        // True funny story. Since my SpherCast was referring to p1 for the position,
        // and I was setting that above here, but never updating it, I did indeed get into
        // an infinite loop. Can't call that unexpected...
        // So I changed p1 to pos which DOES update. I expect no more infinite loops

        while (clearedBuildings == false)
        {




            // Try placing the ship. We do it above, but that is just redundant.
            // We place it at pos, and we rotate it so each subsequent one is 90 degrees more
            // for a nice bidirectional grid
            transform.position = pos;
            

            Vector3 shipRotation = new Vector3(0.0f, rot, 0.0f);
            rotator.transform.eulerAngles = shipRotation;




            // WHAT IS LEFT TO DO?????
            // I STILL HAVE TO MAKE SURE NO SHIP PATHS CAN COLLIDE. 
            // I can do that by raising the ship up 1 height at the very beginning if even,
            // but not odd. That way I can be sure every cross ship is one height above others. And not on the other grid
            




            // Might I need to change transform.forward here to castRotation? Maybe.
            // Because I supsect transform.forward was not taking into account the parent's rotation
            // But is always pointing down the world's blue axis. This will flip it for each ship rotation
            if (Physics.SphereCast(pos, airshipHeight, castRotation, out hit, distanceToCast))
            {
                Debug.Log("<color=blue>SHIP " +
                    i + " is trying Y = " +
                    transform.position.y + 
                    " and is ROTATED " +
                    rot + " degrees, and COLLIDED with "
                    + hit.collider +
                    "!</color>");

                // airshipHeight not 1.0f
                pos = new Vector3(transform.position.x, transform.position.y + (airshipHeight * 2.0f), transform.position.z);
                tries++;
            }
            else
            {
                // Else means it did NOT collide. Which means we're in the clear.
                // We can stop raising the ship, and set the clearedBuilding flag to true
                // which ends the while loop
                clearedBuildings = true;
            }
            

            

            // The while loop has ended, because there was no collision at a new height,
            // so we set the flag to true.
            

        }
        Debug.Log("<color=red>Setting clearedBuildings to true after " + tries + " tries.</color>");



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
