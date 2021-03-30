using src.buildings.controllers;
using UnityEngine;
using Environment = src.util.Environment;

public class BeaconController : BuildingController
{
    GameObject cameraRig;
    GameObject explosive;

    /*===============================
    *  IDestroyable
    ==============================*/
    public override float ExtraOffset() =>  Environment.BEACON_OFFSET;
}
