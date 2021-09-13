using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDestroy : MonoBehaviour
{

    [SerializeField] private float dieRotationX = 60f;
    [SerializeField] private float dieSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
         Vector3 rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(dieRotationX, transform.rotation.y, transform.rotation.z)), Time.deltaTime * dieSpeed).eulerAngles;
         transform.rotation = Quaternion.Euler(rotation.x, transform.rotation.y, transform.rotation.z);
    }
}
