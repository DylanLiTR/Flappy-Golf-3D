using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : Photon.MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] GameObject[] wingSkin;
	MeshRenderer mesh;
	GameObject wingParent;

    void Awake()
    {
		mesh = GetComponent<MeshRenderer>();
        wingParent = transform.Find("Wings").gameObject;
    }

    public void ChangeBall(string skin)
    {
        if (PhotonNetwork.inRoom) 
        {
            photonView.RPC("SetBall", PhotonTargets.AllBuffered, skin);
        }
        else
        {
            SetBall(skin);
        }
    }

    public void ChangeWings(int skinIndex)
    {
        if (PhotonNetwork.inRoom) 
        {
            photonView.RPC("SetWings", PhotonTargets.AllBuffered, skinIndex);
        }
        else
        {
            SetWings(skinIndex);
        }
    }

    [PunRPC]
	void SetBall(string skin)
	{
        if (skin == "Yellow")
        {
                mesh.material.color = Color.yellow;
        }
        else if (skin == "White")
        {
                mesh.material.color = Color.white;
        }
        else if (skin == "Orange")
        {
                mesh.material.color = new Color(1, 102f / 255f, 0);
        }
        else if (skin == "Red")
        {
                mesh.material.color = Color.red;
        }
        else if (skin == "Pink")
        {
                mesh.material.color = Color.magenta;
        }
        else if (skin == "Blue")
        {
                mesh.material.color = Color.blue;
        }
        else if (skin == "Black")
        {
                mesh.material.color = Color.black;
        }
	}

	[PunRPC]
	void SetWings(int skinIndex)
	{
        // Destroy existing wings
        foreach (Transform child in wingParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new wings
        GameObject wings = Instantiate(wingSkin[skinIndex]);
        wings.transform.parent = wingParent.transform;

        // Reset transform
        wings.transform.localScale = new Vector3(1f, 1f, 1f);
        wings.transform.localPosition = new Vector3(0f, 0f, 0f);
        wings.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        GetComponent<Movement>().flapAnim = wings.transform.GetChild(0).gameObject.GetComponent<Animator>();
	}
}
