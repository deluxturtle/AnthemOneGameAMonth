using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Author: Andrew Seba
/// Description: controlls options menu.
/// </summary>
public class Options : MonoBehaviour {

    public GameObject optionPanel;
    public Slider musicSlider;

    private bool option;
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc == null)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();

        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (option)
            {
                optionPanel.SetActive(false);
                option = !option;
            }
            else
            {
                optionPanel.SetActive(true);
                option = !option;
            }
        }

        if (option)
        {
            audioSrc.volume = musicSlider.value;
        }
    }
}
