using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.gray;
    }
}
