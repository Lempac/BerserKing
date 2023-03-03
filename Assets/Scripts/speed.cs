using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class speed : MonoBehaviour
{
    /*private void OnDisable()
    {
        if (!gameObject.activeSelf)
        {
            transform.position = new Vector3(0, 2);
            gameObject.SetActive(true);
        }

    }*/
    
    public void respawn(GameObject player)
    {

        transform.position = new Vector3(Random.Range(1, 6), Random.Range(1, 6));

    }
}
