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

    [SerializeField]
    private Renderer timerGaugeRend = null;

    [HideInInspector]
    public int buildingOwner = 0;

    private AudioClip clipDestruct = null;

    private Renderer rend = null;

    private MaterialPropertyBlock mpb = null;

    private MaterialPropertyBlock gaugeMpb = null;

    private MaterialPropertyBlock[] spriteBlock = null;

    // An array of sprites to represent the various powerups, from 0 to max, and we must be consistent with ordering, because 
    // a later case statement will not only populate the visuals, but perform the powerup
    [SerializeField]
    private Sprite[] powerupSprite = null;

    [SerializeField]
    private Color[] powerupColor = null;

    [SerializeField]
    private float[] powerupAnimSpeed = null;

    private int powerupNumber = 0;

    private SpriteRenderer[] spriteRend = null;

    [SerializeField]
    private float[] powerupCash = null;
    [SerializeField]
    private float[] powerupGas = null;
    [SerializeField]
    private float[] powerupRepair = null;
    private int powerupStrength = 0; // Index for the array of powerup strentghs

    private Animator anim = null;

    private bool triggered = false;

    private Color updatedMeshColor = Color.white;
    private Color gaugeMeshColor = Color.white;

    // Use this for initialization
    private void Start () {

        // For now, get a random sprite representation based on the ones I specify in Inspector
        powerupNumber = (int)(UnityEngine.Random.value * powerupSprite.Length);
        powerupStrength = (int)(UnityEngine.Random.value * powerupCash.Length);

        rend = GetComponentInChildren<Renderer>();
        
        if (rend)
        {
            mpb = new MaterialPropertyBlock();
            // Stupidly, since we're counting UP pads, but counting DOWN numbers, I have
            // to do a little math there to get the right index number
            mpb.SetColor("_Color", new Color(1.0f, 0.0f, 1.0f, 1.0f));
            rend.SetPropertyBlock(mpb);
        }


        gaugeMeshColor = new Color(
            powerupColor[powerupNumber].r,
            powerupColor[powerupNumber].g,
            powerupColor[powerupNumber].b,
            powerupColor[powerupNumber].a
            );

        if (timerGaugeRend)
        {
            gaugeMpb = new MaterialPropertyBlock();
            // Stupidly, since we're counting UP pads, but counting DOWN numbers, I have
            // to do a little math there to get the right index number
            gaugeMpb.SetColor("_Color", gaugeMeshColor);
            timerGaugeRend.SetPropertyBlock(gaugeMpb);
        }

        anim = GetComponentInChildren<Animator>();



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

        // Adjust anim speed based on the strength of the powerup. Low strength = slow
        if (anim && powerupAnimSpeed.Length == powerupCash.Length)
        {
            anim.speed = powerupAnimSpeed[powerupStrength];
        }

        counter = countDownTime;
	}

    // Update is called once per frame
    private void FixedUpdate() {

        counter--;

        // Color the powerup so as it darkens, you know it soon goes away. In this case as a test we simply darken it down to 0,0,0,1
        float newColor = (float)counter / countDownTime;

        updatedMeshColor = new Color(
            powerupColor[powerupNumber].r * newColor,
            powerupColor[powerupNumber].g * newColor,
            powerupColor[powerupNumber].b * newColor,
            powerupColor[powerupNumber].a * newColor
            );

        if (rend)
        {
            mpb.SetColor("_Color", updatedMeshColor); // As test, use Cyan. 0, 1, 1. Alpha stays 1.
            rend.SetPropertyBlock(mpb);
        }

        /* Actually, I don't want to darken the color of the gauge. Just scale it
        if (timerGaugeRend)
        {
            gaugeMpb.SetColor("_Color", updatedMeshColor); // As test, use Cyan. 0, 1, 1. Alpha stays 1.
            timerGaugeRend.SetPropertyBlock(gaugeMpb);
        }
        */


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

        Vector3 timerScale = new Vector3(timerGaugeRend.transform.localScale.x, (float)counter / (float)countDownTime, timerGaugeRend.transform.localScale.x);
        timerGaugeRend.transform.localScale = timerScale;
		
	}

    private void DestroyPowerup()
    {
        Destroy(gameObject);
        gm.buildingHasPowerup[buildingOwner] = false; // Tell the Game Manager that the building no longer has a powerup
    }

    public void DoPowerup()
    {
        // Since the trigger code is in the taxi, it seems to be calling the DoPowerUp multiple times. Perhaps because each collider on taxi
        // is triggering? Not sure. But we need a way to stop it. So:
        if (triggered)
        {
            return;
        }
        triggered = true;

        switch (powerupNumber)
        {
            case 0:
                // If I didn't fill out the array with exactly 4 strengths, always be weakest
                if ( powerupCash.Length != 4 )
                {
                    powerupStrength = 0;
                }

                gm.cash += powerupCash[powerupStrength];
                Debug.LogError("<color=purple>#######################</color> STRENGTH: " + powerupStrength
                    + "; CASH: " + powerupCash[powerupStrength]
                    + "; SPEED: " + anim.speed
                    );

                break;
            case 1:
                // If I didn't fill out the array with exactly 4 strengths, always be weakest
                if (powerupGas.Length != 4)
                {
                    powerupStrength = 0;
                }

                taxi.gas += powerupGas[powerupStrength];
                if (taxi.gas > taxi.maxGas)
                {
                    taxi.gas = taxi.maxGas;
                }
                Debug.LogError("<color=purple>#######################</color> STRENGTH: " + powerupStrength
                    + "; GAS: " + powerupCash[powerupStrength]
                    + "; SPEED: " + anim.speed
                    );

                break;
            case 2:
                // If I didn't fill out the array with exactly 4 strengths, always be weakest
                if (powerupRepair.Length != 4)
                {
                    powerupStrength = 0;
                }

                taxi.damage -= powerupRepair[powerupStrength];
                if (taxi.damage < 0.0f)
                {
                    taxi.damage = 0.0f;
                }
                taxi.ForceDamageGaugeUpdate();
                Debug.LogError("<color=purple>#######################</color> STRENGTH: " + powerupStrength
                    + "; REPAIR: " + powerupCash[powerupStrength]
                    + "; SPEED: " + anim.speed
                    );

                break;
        }

        DestroyPowerup();
    }


}
