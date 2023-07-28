using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Found;
    public TMP_Text Name;
    public TMP_Text Description;
    public Button Take;

    public GameObject MakeMenu(Item item)
    {
        Found.text = $"Found {item.GetType()}!";
        Name.text = item.Name;
        Description.text = item.Description;
        Icon.sprite = item.Sprite;
        return gameObject;
    }
}