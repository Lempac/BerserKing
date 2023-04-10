using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.UI;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
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
        
    }
    private void Start()
    {
        Title.GetComponent<TMP_Text>().text = Application.productName;
        Version.GetComponent<TMP_Text>().text = Application.version;
    }
}
