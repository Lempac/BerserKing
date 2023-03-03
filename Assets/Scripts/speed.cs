using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class speed : MonoBehaviour
{
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        transform.position = new Vector3(0,2);
        GameObject clonename = Instantiate(gameObject);
        clonename.SetActive(true);
        clonename.name = "speed";
    }

}
