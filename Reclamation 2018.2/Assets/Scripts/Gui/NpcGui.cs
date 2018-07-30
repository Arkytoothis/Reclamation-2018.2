using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGui : MonoBehaviour
{
    [SerializeField] GameObject vitalBarPrefab;

    Camera cameraToLookAt;

	void Start ()
    {
        cameraToLookAt = Camera.main;
        Instantiate(vitalBarPrefab, transform.position, Quaternion.identity, transform);
        gameObject.layer = 5;
	}
	
	void LateUpdate ()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
	}
}
