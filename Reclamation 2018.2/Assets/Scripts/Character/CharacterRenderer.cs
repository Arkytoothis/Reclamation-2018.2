using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterRenderer : MonoBehaviour
{
    public Transform pivot;
    public Transform backMount;
    public Transform hipMount;
    public Transform headMount;
    public Transform leftHandMount;
    public Transform rightHandMount;
    public Transform leftShoulderMount;
    public Transform rightShoulderMount;
    public Transform leftBracerMount;
    public Transform rightBracerMount;

    public Mesh fatMesh;
    public Mesh normalMesh;
    public Mesh skinnyMesh;

    void Start()
    {
        //GetComponent<NavMeshAgent>().enabled = false;
    }
}