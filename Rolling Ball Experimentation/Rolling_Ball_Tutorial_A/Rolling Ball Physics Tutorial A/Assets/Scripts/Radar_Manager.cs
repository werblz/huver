using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Radar_Manager : MonoBehaviour {

    [SerializeField]
    private GameObject nextIndicator = null;
    [SerializeField]
    private Color nextCurrentColor = Color.white;
    [SerializeField]
    private float nextIndicatorDefaultScale = 0.08f;
    private MeshRenderer nextIndicatorMesh = null;

    [SerializeField]
    private GameObject gasIndicator = null;
    [SerializeField]
    private Color gasCurrentColor = Color.white;
    [SerializeField]
    private float gasIndicatorDefaultScale = 0.05f;
    private MeshRenderer gasIndicatorMesh = null;

    [SerializeField]
    private GameObject homeIndicator = null;
    [SerializeField]
    private float homeIndicatorDefaultScale = 0.01f;
    private MeshRenderer homeIndicatorMesh = null;

    [SerializeField]
    private GameObject[] ledSegment = null;

    [SerializeField]
    private GameObject compassDirectionIndicator = null;

    [SerializeField]
    private SpriteRenderer checkEngineLight = null;

    [SerializeField]
    private Image gasGaugeImage = null;

    [SerializeField]
    private Image damageImage = null;

    
    [Header ( "Audio")]
    private AudioSource gaugeAudio = null;

    [SerializeField]
    private AudioClip clipFuelPing = null;

    [SerializeField]
    private AudioClip clipFuelPingCrossLine = null;

    [SerializeField]
    private AudioClip clipFuelYellow = null;

    [SerializeField]
    private AudioClip clipFuelOrange = null;

    [SerializeField]
    private AudioClip clipFuelRed = null;


    // The following are for the cracked radar. THIS SYSTEM IS ABOUT TO BE DEPRECATED!
    /*
    [SerializeField]
    private SpriteRenderer[] crackSpriteRenderer = null;
    private MaterialPropertyBlock mpb = null;
    */

    // IN FAVOR OF THIS ONE:
    [Tooltip("Sprite Renderer object that holds the crack sprite image")]
    [SerializeField]
    private SpriteRenderer[] crackSpriteRenderers = null;

    [Tooltip("Sprite images")]
    [SerializeField]
    private Sprite[] crackSprites = null;

    [SerializeField]
    private Taxi_Controller taxi = null;

    [SerializeField]
    private Game_Manager gm = null;

    [SerializeField]
    private TextMeshPro shiftText = null;

    [SerializeField]
    private TextMeshPro gasTankText = null;

    [SerializeField]
    private float gasFlashPercentage = .2f;

    [SerializeField]
    private float gasFlashSpeed = 0.5f;

    [Header("Upgrade Icons")]
    [SerializeField]
    private SpriteRenderer[] upgradeIcon = null;

    [Header("Shield Text")]
    [SerializeField]
    private TextMeshPro shieldText = null;



    private float angle = 0.0f;

    private Vector3 indRotation = new Vector3(0.0f, 0.0f, 0.0f);

    

    private Vector3 compassRotation = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 gasIndRotation = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 homeIndRotation = new Vector3(0.0f, 0.0f, 0.0f);

    private float taxiAngle = 0.0f;

    private int padNum = 0;

    private float damagePercentage = 0.0f;




    private Vector3 padLoc = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 taxiLoc = new Vector3(0.0f, 0.0f, 0.0f);

    private Taxi_Controller tc = null;

    private static bool[,] seg = new bool[,]
    {
        {true, true, true, true, true, true, false },
        {false, true, true, false, false, false, false },
        {true, true, false, true, true, false, true },
        {true, true, true, true, false, false, true },
        {false, true, true, false, false, true, true },
        {true, false, true, true, false, true, true },
        {true, false, true, true, true, true, true },
        {true, true, true, false, false, false, false },
        {true, true, true, true, true, true, true },
        {true, true, true, true, false, true, true },
        {true, true, true, true, true, true, false },
        {true, false, false, true, false, false, true }
    };

    private bool flashGasFlag = false;
    private float flashTime = 0.0f;
    private float alphaColor = 1.0f;
    private bool soundPingedAlreadyYellow = false;
    private bool soundPingedAlreadyOrange = false;
    private bool soundPingedAlreadyRed = false;

    // Color the indicators
    private Color nextColor = Color.white;
    private MaterialPropertyBlock nextMpb = null;
    private Color gasColor = Color.white;
    private MaterialPropertyBlock gasMpb = null;


    // THE PLAN
    //
    // On Update, the radar gets the taxi's current position and rotation.
    // Then it gets the position of the next pad
    // Using trigonometry, we get the angle the pad is at compared to the taxi, subtracting the taxi's rotation
    // The radius will not matter, since we are simply going to rotate the indicator's pad.


    // Use this for initialization
    void Start ()
    {
        // Now get the actual MeshRenderers for the compass indicators
        nextIndicatorMesh = nextIndicator.GetComponentInChildren<MeshRenderer>();
        gasIndicatorMesh = gasIndicator.GetComponentInChildren<MeshRenderer>();
        homeIndicatorMesh = homeIndicator.GetComponentInChildren<MeshRenderer>();
        Debug.Log("             NEXT MESH " + nextIndicatorMesh);
        Debug.Log("             GAS MESH " + gasIndicatorMesh);
        Debug.Log("             HOME MESH " + homeIndicatorMesh);

        // Start their scales off as the accepted defaults
        nextIndicatorMesh.transform.localScale = new Vector3(nextIndicatorDefaultScale, nextIndicatorDefaultScale, nextIndicatorDefaultScale);
        gasIndicatorMesh.transform.localScale = new Vector3(gasIndicatorDefaultScale, gasIndicatorDefaultScale, gasIndicatorDefaultScale);
        homeIndicatorMesh.transform.localScale = new Vector3(homeIndicatorDefaultScale, homeIndicatorDefaultScale, homeIndicatorDefaultScale);

        gaugeAudio = GetComponent<AudioSource>();
        //Debug.Log("<color=white>*************************** Array element 3, 6 = " + seg[3, 5].ToString());

        tc = (Taxi_Controller)taxi.GetComponent(typeof(Taxi_Controller));

        // Turn off all upgrade icons, just in case

        for (int i = 0; i < upgradeIcon.Length; i++)
        {
            upgradeIcon[i].enabled = false;
        }

        checkEngineLight.enabled = false;

        DisplayLED(gm.numPads);
        //Debug.Log("<color=blue>************************************ Number of pads to start = " + gm.numPads);

        //mpb = new MaterialPropertyBlock();
        //crackedRadarSprite.GetPropertyBlock(mpb);

        // Set color of indicators.
        // Next Indicator

    }

    // Update is called once per frame

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!! INVESTIGATE transform.LookAt! This apparently does it 
    void Update()
    {
        padNum = gm.nextPad;

        nextIndicator.SetActive(padNum < gm.numPads);

        shiftText.text = gm.shift.ToString();

        gasTankText.text = taxi.gas.ToString("#0");

        // Get the taxi angle. It is used for all angular indicators on radars
        taxiAngle = taxi.transform.localEulerAngles.y;

        // THIS ONE IS THE REAL DIGITAL READOUT
        DisplayLED(gm.numPads - padNum); // Better way to do it, with an array of bools

        // If level is over, do not try getting the next pad's location, since it will put the array out of bounds
        if (padNum < gm.numPads && taxi.hasNextIndicator)
        {
            // Find the location of the current pad
            padLoc = gm.pads[gm.nextPad].transform.position;

            // Find the taxi's location
            taxiLoc = taxi.transform.position;

            // Use trig to get the angle
            angle = Mathf.Atan2(padLoc.x - taxiLoc.x, padLoc.z - taxiLoc.z) * 180 / Mathf.PI;
            //taxiAngle = taxi.transform.localEulerAngles.y;
            indRotation = new Vector3(0.0f, angle + 180.0f - taxiAngle, 0.0f); // This is in case angle needs an offset.] DOES THIS DO ANYTHING?
            compassRotation = new Vector3(0.0f, 0.0f, taxiAngle);

            //!!!!!!!!!!!!!!!!!!!! - SO FAR THE INDICATOR DOES ROTATE, but not accurately. It's in the angle offset first And then has to add the taxi's rotation
            nextIndicator.transform.localRotation = Quaternion.Euler(indRotation);

            // Get distance between taxi and this pad. Luckily I just got the locations of the two above here
            // So it's a matter of math. With maxPadDistance being 1050, how do I get a value that means something useful between, say, 0 and 1?
            float padDistance = Vector3.Distance(padLoc, taxiLoc);
            // y=mx+B
            // where Y is the result
            // m is the slope
            // m = y2-y2/x2-x1 => .5-1/1050-0 => -.5/1050
            // B is 1, the y value at 0 on the graph
            // x is distance to pad
            // y = (-.5/1050) * distance + 1
            float padDistanceNormalized = (-.5f / gm.maxPadDistance) * padDistance + 1.0f;
            float padScaleMult = padDistanceNormalized * nextIndicatorDefaultScale;
            // Limit the scale from getting too tiny to see
            if (padScaleMult < nextIndicatorDefaultScale / 5.0f)
            {
                padScaleMult = nextIndicatorDefaultScale / 5.0f;
            }
            nextIndicatorMesh.transform.localScale = new Vector3(padScaleMult, padScaleMult, padScaleMult);

            // Now darken the color over distance too.
            if (nextIndicatorMesh)
            {
                nextMpb = new MaterialPropertyBlock();
                // Stupidly, since we're counting UP pads, but counting DOWN numbers, I have
                // to do a little math there to get the right index number
                Color newNextColor = nextCurrentColor * padDistanceNormalized;
                nextMpb.SetColor("_Color", newNextColor);
                nextIndicatorMesh.SetPropertyBlock(nextMpb);
            }

        }

        // Turn pad indicator on or off depending on if we have it
        nextIndicator.SetActive(taxi.hasNextIndicator);

        homeIndicator.SetActive(taxi.hasHomeIndicator);


        // Gas Station Indicator
        if (taxi.hasStationIndicator)
        {
            // Find the closest pad to the taxi
            Vector3 gasLoc = gm.stations[ClosestStation()].transform.position;
            

            // Find the taxi's location
            taxiLoc = taxi.transform.position;

            // Use trig to get the angle
            angle = Mathf.Atan2(gasLoc.x - taxiLoc.x, gasLoc.z - taxiLoc.z) * 180 / Mathf.PI;
            //taxiAngle = taxi.transform.localEulerAngles.y;
            gasIndRotation = new Vector3(0.0f, angle + 180.0f - taxiAngle, 0.0f); // This is in case angle needs an offset.] DOES THIS DO ANYTHING?
            //gasIndicatorRot = new Vector3(0.0f, 0.0f, taxiAngle);

            //!!!!!!!!!!!!!!!!!!!! - SO FAR THE INDICATOR DOES ROTATE, but not accurately. It's in the angle offset first And then has to add the taxi's rotation
            gasIndicator.transform.localRotation = Quaternion.Euler(gasIndRotation);

            // Get distance between taxi and this gas pad. Luckily I just got the locations of the two above here
            // So it's a matter of math. With maxPadDistance being 1050, how do I get a value that means something useful between, say, 0 and 1?
            float gasDistance = Vector3.Distance(gasLoc, taxiLoc);
            // y=mx+B
            // where Y is the result
            // m is the slope
            // m = y2-y2/x2-x1 => .5-1/1050-0 => -.5/1050
            // B is 1, the y value at 0 on the graph
            // x is distance to pad
            // y = (-.5/1050) * distance + 1
            float gasDistanceNormalized = (-1.8f / gm.maxPadDistance) * gasDistance + 1.0f;
            float gasScaleMult = gasDistanceNormalized * gasIndicatorDefaultScale;
            if (gasScaleMult < gasIndicatorDefaultScale / 4.0f)
            {
                gasScaleMult = gasIndicatorDefaultScale / 4.0f;
            }
            gasIndicatorMesh.transform.localScale = new Vector3(gasScaleMult, gasScaleMult, gasScaleMult);
            // Limit the scale from getting too tiny to see


            // Now darken the color over distance too.
            if (gasIndicatorMesh)
            {
                gasMpb = new MaterialPropertyBlock();
                // Stupidly, since we're counting UP pads, but counting DOWN numbers, I have
                // to do a little math there to get the right index number
                Color newGasColor = gasCurrentColor * gasDistanceNormalized;
                gasMpb.SetColor("_Color", newGasColor);
                gasIndicatorMesh.SetPropertyBlock(gasMpb);
            }

        }

        if (taxi.hasHomeIndicator)
        {
            // Find the home pad location
            Vector3 homeLoc = gm.homeBldg.transform.position;

            // Find the taxi's location
            taxiLoc = taxi.transform.position;

            // Use trig to get the angle
            angle = Mathf.Atan2(homeLoc.x - taxiLoc.x, homeLoc.z - taxiLoc.z) * 180 / Mathf.PI;
            //taxiAngle = taxi.transform.localEulerAngles.y;
            homeIndRotation = new Vector3(0.0f, angle + 180.0f - taxiAngle, 0.0f); // This is in case angle needs an offset.] DOES THIS DO ANYTHING?
                                                                                   //gasIndicatorRot = new Vector3(0.0f, 0.0f, taxiAngle);

            // Get distance between taxi and this pad. Luckily I just got the locations of the two above here
            // So it's a matter of math. With maxPadDistance being 1050, how do I get a value that means something useful between, say, 0 and 1?
            float homeDistance = Vector3.Distance(homeLoc, taxiLoc);
            // y=mx+B
            // where Y is the result
            // m is the slope
            // m = y2-y2/x2-x1 => .5-1/1050-0 => -.5/1050
            // B is 1, the y value at 0 on the graph
            // x is distance to pad
            // y = (-.5/1050) * distance + 1
            float homeDistanceNormalized = (-.5f / gm.maxPadDistance) * homeDistance + 1.0f;
            float homeScaleMult = homeDistanceNormalized * homeIndicatorDefaultScale;
            // Limit the scale from getting too tiny to see
            if (homeScaleMult < homeIndicatorDefaultScale / 5.0f)
            {
                homeScaleMult = homeIndicatorDefaultScale / 5.0f;
            }
            homeIndicatorMesh.transform.localScale = new Vector3(homeScaleMult, homeScaleMult, homeScaleMult);

            //!!!!!!!!!!!!!!!!!!!! - SO FAR THE INDICATOR DOES ROTATE, but not accurately. It's in the angle offset first And then has to add the taxi's rotation
            homeIndicator.transform.localRotation = Quaternion.Euler(homeIndRotation);

        }

        // Turn indicator on if we have it, off if not
        gasIndicator.SetActive(taxi.hasStationIndicator);

        // Compass overlay
        compassDirectionIndicator.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, taxiAngle);

        // Update the gas gauge
        float fillPercentage = Mathf.Abs(tc.gas / tc.maxGas);
        gasGaugeImage.fillAmount = fillPercentage * 0.75f;


        // This next section is for when the gas gauge crosses a color line.
        // Right now, it pings inside a range. That's bad, since a slow leak will ping for a long time. I want a true one-shot ping inside the range.
        // Which means setting up a bool that I set to true when it pings the first time, and then false again after it's outside the range.

        // If gas crosses the green line, make a ping noise - Magic numbers represent the graphic of the gas gauge where it changes color
        if ( fillPercentage >= .530 && fillPercentage <= .533)
        {
            if (!soundPingedAlreadyYellow)
            {
                gaugeAudio.PlayOneShot(clipFuelYellow, 1.0f); // clipFuelPingCrossLine differs by having some silence at the beginning, so PlayOneSHot repeating will play silence until it stop playing constantly
                soundPingedAlreadyYellow = true;
            }
            
        }
        else
        {
            soundPingedAlreadyYellow = false; // Reset the flag that triggers the in-range gas sound,.
        }

        

        if (fillPercentage >= .355 && fillPercentage <= .358)
        {
            if (!soundPingedAlreadyOrange)
            {
                gaugeAudio.PlayOneShot(clipFuelOrange, 1.0f); // clipFuelPingCrossLine differs by having some silence at the beginning, so PlayOneSHot repeating will play silence until it stop playing constantly
                soundPingedAlreadyOrange = true;
            }

        }
        else
        {
            soundPingedAlreadyOrange = false; // Reset the flag that triggers the in-range gas sound,.
        }

        if ( fillPercentage >= gasFlashPercentage && fillPercentage <= gasFlashPercentage + 0.003 )
        {
            if (!soundPingedAlreadyRed)
            {
                gaugeAudio.PlayOneShot(clipFuelRed, 1.0f); // clipFuelPingCrossLine differs by having some silence at the beginning, so PlayOneSHot repeating will play silence until it stop playing constantly
                soundPingedAlreadyRed = true;
            }

        }
        else
        {
            soundPingedAlreadyRed = false; // Reset the flag that triggers the in-range gas sound,.
        }

        // If gas gets into the red, flash
        if ( fillPercentage < gasFlashPercentage)
        {
            flashGasFlag = true;
        }
        else
        {
            flashGasFlag = false;
            gasGaugeImage.color = Color.white;
           
        }

        // Make the bing go faster the lower the gauge gets into the red.
        // Divide fillPercentage by gasFlashPercentage. That will be 1 when it starts, as the two equal
        // Then, though, I want it faster to start. So multiply it by .5. That's a half-second.
        // Then add an offset of .2, because I don't want it faster than that
        gasFlashSpeed = ( fillPercentage / gasFlashPercentage ) + 0.3f;

        FlashGas(gasFlashSpeed);




        // Display the appropriate upgrade icons
        IconsUp();


    }



    private void FlashGas (float speed)
    {
        



        if (flashGasFlag == false)
        {
            return;
        }


        flashTime += Time.deltaTime;

        if (flashTime > speed)
        {
            if (!taxi.isCrashing)
            {
                gaugeAudio.PlayOneShot(clipFuelPing, 0.7f);
            }

            alphaColor = 1.0f;
            flashTime = 0;
            // If the taxi is crashing, we don't want the gas gauge to continue alerting

            
        }
        else
        {
            alphaColor -= 0.03f;
        }

        gasGaugeImage.color = new Color(1.0f, 1.0f, 1.0f, alphaColor);

    }




    public void UpdateDamageRadar()
    {
        // I used to do this here, but now I need it to be a method so I can call it from the taxi on demand.
        // Because when I use a powerup that fixes damage, it does not update the radar. So Powerup Manager now must tell the taxi to call this
        // because the Powerup Manager is not going to have a reference to the radar manager. The taxi does
        // Update damage gauge
        damagePercentage = Mathf.Abs(tc.damage / tc.maxDamage);
        damageImage.fillAmount = damagePercentage * 0.165f;

        // Update the glass crack overlay
        // Get the damage, stepped by dividing by 5
        // 0 = no damage, 1-4 = increasing damage. 4 is max
        // So take damagePercentage, mult by 5. (0-1 becomes 0-4)
        // But this gauges to howver many sprites you put in the array. I now have 5 sprites.
        // 0 is no damage. 1,2,3,4 are incremental damage, then I want the final damage NOT 
        // to change sprites, but keep 4 up there. This way 4 is max damage, but you keep flying
        // for ONE MORE damage pip
        float damageStepped = (int)(damagePercentage * crackSprites.Length);
        if (damageStepped > crackSprites.Length)
        {
            damageStepped = crackSprites.Length;
        }
        //Debug.Log("<color=red>**********DAMAGE DAMAGE DAMAGE - " + damageStepped + "</color>");
        /*
        Debug.Log("<color=red>**********DAMAGE DAMAGE DAMAGE - " + damageStepped +
            " which uses SPRITE " + crackSprites[(int)damageStepped] + "</color>");
        */

        for (int i = 0; i < crackSpriteRenderers.Length; i++)
        {
            if (damageStepped > crackSpriteRenderers.Length)
            {
                damageStepped = crackSpriteRenderers.Length;
            }
            crackSpriteRenderers[i].sprite = crackSprites[(int)damageStepped];
            //Debug.Log("<color=blue> Sprite is " + crackSprites[(int)damageStepped].name + "</color>");
        }
    }



    int ClosestStation()
    {
        int closest = 0;

        float minDistance = 10000.0f;

        for (int i = 0; i < gm.numStations; i++)
        {
            // Get location of this station
            Vector3 sLoc = gm.stations[i].transform.position;
            Vector3 tLoc = taxi.transform.position;



            float distance = Vector3.Distance(sLoc, tLoc);
            

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = i;
            }
            
        }
        return closest;
    }

    // seg is a static array set up with the segments of the LED display set to on or off to correspond to the digits 0-9
    void DisplayLED(int number)
    {
        if (number <0 || number > 9)
        {
            number = 10;
        }
            
        for (int i = 0; i < 7; i++)
        {
            ledSegment[i].SetActive(seg[number, i]);
        }
        
    }


    void IconsUp()
    {
        // Change this code section to this form: upgradeIcon[0].enabled = gm.hasRadarPad.
        /*
        if (gm.hasRadarPad)
        {
            upgradeIcon[0].enabled = true;
        }
        if (gm.hasRadarStation)
        {
            upgradeIcon[1].enabled = true;
        }
        if (gm.hasStrafe)
        {
            upgradeIcon[2].enabled = true;
        }
        if (gm.hasTurbo)
        {
            upgradeIcon[3].enabled = true;
        }
        if (gm.hasTank)
        {
            upgradeIcon[4].enabled = true;
        }
        if (taxi.minCollisionThreshold > 10.0f)
        {
            upgradeIcon[5].enabled = true;
        }
        */

        upgradeIcon[0].enabled = gm.hasRadarPad;
        upgradeIcon[1].enabled = gm.hasRadarStation; ;
        upgradeIcon[2].enabled = gm.hasStrafe;
        upgradeIcon[3].enabled = gm.hasTurbo;
        upgradeIcon[4].enabled = gm.hasTank;
        upgradeIcon[5].enabled = taxi.shieldPercent > 1.0f;
        shieldText.enabled = taxi.shieldPercent > 1.0f; // Enable the text ONLY if shieldPercent > 1.0. Otherwise disble
        shieldText.text = "+" + (taxi.shieldPercent - 1.0f).ToString("P0");
        
        upgradeIcon[6].enabled = gm.hasHomePad;

        if (!gm.uiIsUp)
        {
            // hasControl? If not, CHECK ENGINE LIGHT
            checkEngineLight.enabled = !gm.hasControl;

        }

    }

}
