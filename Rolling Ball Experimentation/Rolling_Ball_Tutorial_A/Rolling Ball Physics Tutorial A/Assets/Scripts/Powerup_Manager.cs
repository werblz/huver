using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private Renderer rend = null;

    private MaterialPropertyBlock mpb = null;

    private MaterialPropertyBlock[] spriteBlock = null;

    // An array of sprites to represent the various powerups, from 0 to max, and we must be consistent with ordering, because 
    // a later case statement will not only populate the visuals, but perform the powerup
    [SerializeField]
    private Sprite[] powerupSprite = null;

    [SerializeField]
    private Color[] powerupColor = null;

    private int powerupNumber = 0;

    private SpriteRenderer[] spriteRend = null;



    // Use this for initialization
    private void Start () {

        rend = GetComponentInChildren<Renderer>();
        
        
        if (rend)
        {
            mpb = new MaterialPropertyBlock();
            // Stupidly, since we're counting UP pads, but counting DOWN numbers, I have
            // to do a little math there to get the right index number
            mpb.SetColor("_Color", new Color(1.0f, 0.0f, 1.0f, 1.0f));
            rend.SetPropertyBlock(mpb);

        }

        // For now, get a random sprite representation based on the ones I specify in Inspector
        powerupNumber = (int)(UnityEngine.Random.value * powerupSprite.Length);

        // Get sprites attached. This takes the array by name (no index) and fills it out with the number of sprites it finds
        spriteRend = GetComponentsInChildren<SpriteRenderer>();

        // Now populate them with a random sprite image, and color them
        for (int i = 0; i < spriteRend.Length; i++)
        {
            spriteRend[i].sprite = powerupSprite[powerupNumber];
            spriteRend[i].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        }

        // Get their MaterialPropertyBlocks
        // First, resize the array of MaterialPropertyBlocks to the size of the number of sprites we found
        /* NOT NECESSARY! Since a SpriteRenderer already has a color property, I should not need MPB
        Array.Resize(ref spriteBlock, spriteRend.Length);
        for (int i = 0; i < spriteRend.Length; i++ )
        {
            spriteBlock[i] = new MaterialPropertyBlock();
            spriteBlock[i].SetColor("_Color", new Color(1.0f, 0.0f, 0.0f, 1.0f));
            spriteRend[i].SetPropertyBlock(spriteBlock[i]);
        }
        */


        counter = countDownTime;
	}

    // Update is called once per frame
    private void FixedUpdate() {

        counter--;

        // Color the powerup so as it darkens, you know it soon goes away. In this case as a test we simply darken it down to 0,0,0,1
        float newColor = (float)counter / countDownTime;

        if (rend)
        {
            Color updatedMeshColor = new Color(
                powerupColor[powerupNumber].r * newColor,
                powerupColor[powerupNumber].g * newColor,
                powerupColor[powerupNumber].b * newColor,
                powerupColor[powerupNumber].a * newColor
                );
            mpb.SetColor("_Color", updatedMeshColor); // As test, use Cyan. 0, 1, 1. Alpha stays 1.
            rend.SetPropertyBlock(mpb);

        }

        for (int i = 0; i < spriteRend.Length; i++)
        {
            // This color differs from the Mesh color because it stays the same color, never darkening, just alpha fades
            Color updatedSpriteColor = new Color(
                powerupColor[powerupNumber].r,
                powerupColor[powerupNumber].g,
                powerupColor[powerupNumber].b,
                powerupColor[powerupNumber].a * newColor
                );
                
            spriteRend[i].color = updatedSpriteColor; // Make the sprite turquoise, but fade over time. But leave color alone. Different effect
        }

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

        switch (powerupNumber)
        {
            case 0:
                gm.cash += 50.0f;
                break;
            case 1:
                taxi.gas += 50.0f;
                if (taxi.gas > taxi.maxGas)
                {
                    taxi.gas = taxi.maxGas;
                }
                break;
            case 2:
                taxi.damage -= 10.0f;
                if (taxi.damage < 0.0f)
                {
                    taxi.damage = 0.0f;
                }
                taxi.ForceDamageGaugeUpdate();
                break;
        }

        DestroyPowerup();
    }


}
