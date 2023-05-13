using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomizationMenu : MonoBehaviour
{
    public Customization customizationScript;

    [SerializeField] TMP_Dropdown ballDropdown;
    [SerializeField] TMP_Dropdown wingsDropdown;
    [SerializeField] TMP_Dropdown trailDropdown;

    public void ChangeBall(int skinIndex)
    {
        // Change skin colour based on dropdown option selected
        string skin = ballDropdown.options[ballDropdown.value].text;
        customizationScript.ChangeBall(skin);
        PlayerPrefs.SetInt("ball", skinIndex);
    }

    public void ChangeWings(int skinIndex)
    {
        customizationScript.ChangeWings(skinIndex);
        PlayerPrefs.SetInt("wings", skinIndex);
    }

    public void ChangeTrail(int skinIndex)
    {
        PlayerPrefs.SetInt("trail", skinIndex);
    }

    public void Reset()
    {
        ChangeBall(0);
        ChangeWings(0);

        ballDropdown.value = 0;
        wingsDropdown.value = 0;
        trailDropdown.value = 0;

        PlayerPrefs.SetInt("ball", 0);
        PlayerPrefs.SetInt("wings", 0);
        PlayerPrefs.SetInt("trail", 0);
    }
}
