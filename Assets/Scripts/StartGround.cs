using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartGround : MonoBehaviour
{
    // To get StartGround's localscale
    [SerializeField] private GameObject startGroundObject;
    public Vector3 StartGroundObjScale
    {
        get { return startGroundObject.transform.localScale; }
    }
}
