using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] MeshRenderer ball;
    [SerializeField] Movement moveScript;
    [SerializeField] GameObject wingParent;

    [SerializeField] TMP_Dropdown ballDropdown;
    [SerializeField] TMP_Dropdown wingsDropdown;
    [SerializeField] TMP_Dropdown trailDropdown;

    [SerializeField] GameObject[] wingSkin;

    public void ChangeBall(int skinIndex)
    {
        // Change skin colour based on dropdown option selected
        string skin = ballDropdown.options[ballDropdown.value].text;
        if (skin == "Yellow")
        {
            ball.material.color = Color.yellow;
        }
        else if (skin == "White")
        {
            ball.material.color = Color.white;
        }
        else if (skin == "Orange")
        {
            ball.material.color = new Color(1, 102f / 255f, 0);
        }
        else if (skin == "Red")
        {
            ball.material.color = Color.red;
        }
        else if (skin == "Pink")
        {
            ball.material.color = Color.magenta;
        }
        else if (skin == "Blue")
        {
            ball.material.color = Color.blue;
        }
        else if (skin == "Black")
        {
            ball.material.color = Color.black;
        }
        PlayerPrefs.SetInt("ball", skinIndex);
    }

    public void ChangeWings(int skinIndex)
    {
        // Destroy existing wings
        Destroy(wingParent.transform.GetChild(0).gameObject);

        // Create new wings
        GameObject wings = Instantiate(wingSkin[skinIndex]);
        wings.transform.parent = wingParent.transform;

        // Reset transform
        wings.transform.localScale = new Vector3(1f, 1f, 1f);
        wings.transform.localPosition = new Vector3(0f, 0f, 0f);
        wings.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        if (moveScript != null)
        {
            moveScript.flapAnim = wings.transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
        PlayerPrefs.SetInt("wings", skinIndex);
    }

    public void ChangeTrail(int skinIndex)
    {
        PlayerPrefs.SetInt("trail", skinIndex);
    }

    public void Reset()
    {
        ball.material.color = Color.yellow;
        ballDropdown.value = 0;
        PlayerPrefs.SetInt("ball", 0);

        ChangeWings(0);
        wingsDropdown.value = 0;
        PlayerPrefs.SetInt("wings", 0);
        PlayerPrefs.SetInt("trail", 0);
    }
}
