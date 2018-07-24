using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PortraitMount : MonoBehaviour
{
    public Transform pivot;
    public Light spotLight;
    public Light pointLightLeft;
    public Light pointLightRight;

    public Camera rtCamera;
    public RawImage rawImage;
}