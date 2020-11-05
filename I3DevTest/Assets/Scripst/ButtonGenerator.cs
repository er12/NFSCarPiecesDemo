using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using System;

public class ButtonGenerator : MonoBehaviour
{
    public GameObject car;
    public GameObject buttonPrefab;
    private Camera currentCam;
    public event Action<GameObject> ViewPart;

    // Start is called before the first frame update
    void Start()
    {
        currentCam = Camera.main;
        GetParts();
    }


    private void GetParts()
    {
        // Auto generate the list of parts of the car
        foreach(Transform part in car.transform)
        {
            part.gameObject.AddComponent<MeshCollider>();
            part.gameObject.AddComponent<Highlight>();

            GameObject button = Instantiate(buttonPrefab);

            //Add the button to the Grid
            button.transform.SetParent(transform.Find("Grid").transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);


            // Separate string by uppercase to generate name
            string[] splitName = Regex.Split(part.name, @"(?<!^)(?=[A-Z])");

            string name = string.Join(" ", splitName);


            button.GetComponentInChildren<TextMeshProUGUI>().SetText(name);
            
            Button tempButton = button.GetComponent<Button>();

            // Make 'click on button' have an System.Action to do the same thing as ViewPArt
            tempButton.onClick.AddListener(() => ViewPart?.Invoke(part.gameObject));
        }
    }

}
