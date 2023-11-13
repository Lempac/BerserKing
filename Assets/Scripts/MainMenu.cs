using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public static bool DoDamageNumbers;
    [SerializeField] private AudioMixer main;
    [SerializeField] private GameObject Title, Version;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OnVolumeChange(float val)
    {
        main.SetFloat("MainVolume", val);
    }
    public void OnDamageNumbers(bool val)
    {
        DoDamageNumbers = val;
    }
    public void OnEntityLimiChange(string val)
    {
        if(Convert.ToInt32(val) > 0)
        EntityManager.EntityLimit = Convert.ToInt32(val);
    }
    private void Start()
    {
        Title.GetComponent<TMP_Text>().text = Application.productName;
        Version.GetComponent<TMP_Text>().text = Application.version;
    }
}
