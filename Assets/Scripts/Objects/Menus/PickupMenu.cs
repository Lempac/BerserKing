using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickupMenu : MonoBehaviour, IMenu
{
    public TMP_Text Title;
    public Image Image;
    public Button Take;

    public GameObject MakeMenu(Item item)
    {
        //Title.text = item.name;
        return null;
    }
}
