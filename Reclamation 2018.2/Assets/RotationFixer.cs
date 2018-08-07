using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFixer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = new Quaternion(-90, 0, 0, 0);
	}
}
