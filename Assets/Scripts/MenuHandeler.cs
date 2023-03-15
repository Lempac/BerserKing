using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandeler : MonoBehaviour
{
    public static MenuHandeler Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Menu Handeler!");
        else Instance = this;
    }
}
