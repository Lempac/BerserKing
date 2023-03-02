using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speed : MonoBehaviour
{
    private void OnDestroy()
    {
        transform.position = new Vector3(0,2);
        GameObject clonename = Instantiate(gameObject);
        clonename.SetActive(true);
        clonename.name = "speed";
    }

}
