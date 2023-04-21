using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerForPath : MonoBehaviour
{
    public Transform playerTransform; 
    
    void Update()
    {
        
        if (playerTransform != null)
        {
            
            transform.position = playerTransform.position;
        }
    }
}