using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData : MonoBehaviour {


    // These are on the Game Manager object
    //public int numPads;
    public float cash;
    public float standardFare; // Could be upgradable
    public float fareDrain; // Could be upgradabe
    public float tipDrain; // Could be upgradable
    public float gasCost; // Could be upgradable
    public float homePadGasCost; // Could change with gameplay
    public float gasFillRate; // Could change with gameplay
    public float homePadGasFillRate; // Could be upgradable
    public float damageRepairCost; // Could be upgradable
    public float homePadDamageRepairCost; // Could be upgradable
    public float DamageRepairRate; // Could be upgradable
    public float homePadDamageRepairRate; // Could be upgradable
    public float crashDeductible; // Could be upgradable
    //public int numPadsLandedOn;
    public bool hasHomePad;
    public bool hasRadarPad;
    public bool hasRadarStation;
    public bool hasStrafe;
    public bool hasTurbo;
    public bool hasTank;
    public bool hasControl;
    public int shift;
    public float faresThisShift;
    public float tipsThisShift;
    public float gasCostThisShift;
    public float repairsCostThisShift;
    public float gasCostHome;
    public float repairsCostHome;
    public bool isNewGame;
    //public int numPadsThisShift;
    //public int nextPad;


    // ONES I FORGOT
    // Add the number of pads this shift, so if you quit at pad 3, next time, you will still be at pad 3
    // Add the shift, so if you end at shift 11, you start again at shift 11


    // These are on the Taxi object
    public float upThrustMult;
    public float downThrustMult;
    public float forwardThrustMult;
    public float sideThrustMult;
    public float thrustMultiplier;
    public float turboMultiplier;
    public float turboAmount;
    public float maxGas;
    public float gas;
    public float gasUseRateUpThrust;
    public float gasUseRateDownThrust;
    public float gasUseRateForwardThrust;
    public float gasUseRateRotateThrust;
    public float gasUseRateSideThrust;
    public float defaultAngularDrag;
    public float minCollisionThreshold;
    public float maxDamage;
    public float damage;
    public bool hasHomeIndicator;
    public bool hasNextIndicator;
    public bool hasStationIndicator;
    public bool hasStrafeUpgrade;
    public bool hasTurboUpgrade;
    public float shieldPercent;

}
