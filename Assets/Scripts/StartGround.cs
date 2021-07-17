using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartGround : MonoBehaviour
{
    [SerializeField] private GameObject startGroundObject;
    public Vector3 StartGroundObjScale
    {
        get { return startGroundObject.transform.localScale; }
    }
}
