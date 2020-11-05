using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewerController : MonoBehaviour
{
    public float speed = 5f;
    private GameObject selectedPart;
    public GameObject labelPrefab;
    private ButtonGenerator buttonGenerator;

    void Start()
    {
        buttonGenerator = FindObjectOfType<ButtonGenerator>();
        // Set up action
        buttonGenerator.ViewPart += ButtonGenerator_ViewPart;

        labelPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                // If it's not the ground and part not previously selected
                if (!hit.transform.gameObject.tag.Equals("Ground") && selectedPart != hit.transform.gameObject)
                {
                    selectedPart = hit.transform.gameObject;
                    StartCoroutine(moveCamera(selectedPart));
                }
                else
                {
                    selectedPart = null;
                    HideLabel();
                }
            }
        }

    }

    private void ButtonGenerator_ViewPart(GameObject obj)
    {
        selectedPart = obj;
        StartCoroutine(moveCamera(obj));
    }
    
    public IEnumerator moveCamera(GameObject part)
    {
        Transform sphereTransform = transform.parent.transform;

        // Create direction vector
        Vector3 v = sphereTransform.position - part.transform.position;
    
        // While the difference of the rotations is significant
        while (Mathf.Abs( Mathf.Abs(Quaternion.LookRotation(-v).y) - Mathf.Abs(sphereTransform.rotation.y) )>= 0.1f)
        {
            // calculate the Quaternion for the rotation
            // -v since the vector is backwards
            Quaternion rot = Quaternion.Slerp(sphereTransform.rotation, Quaternion.LookRotation(-v), speed * Time.deltaTime);

            //Apply the rotation 
            sphereTransform.rotation = rot;

            sphereTransform.eulerAngles = new Vector3(0, sphereTransform.eulerAngles.y, 0);
            
            // Mocked an Update since this method is used in 2 diferent places
            yield return new WaitForEndOfFrame();
        }

        // Now look
        //transform.LookAt(part.transform, Vector3.up);
        var rotation = Quaternion.LookRotation(part.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        DisplayLabel(selectedPart);
    }

    private void DisplayLabel(GameObject part)
    {
        string name = part.name;
        labelPrefab.transform.GetComponentInChildren<Text>().text = name;

        //Obtain the line renderer from the panel
        LineRenderer lr = labelPrefab.transform.GetComponent<LineRenderer>();

        lr.sortingOrder = 1;
        lr.material = new Material(Shader.Find("Unlit/Texture"));
        lr.material.color = Color.red;
        lr.startWidth = 3;
        lr.endWidth = 6;

        // From the panel to the part
        lr.SetPosition(0, labelPrefab.transform.position );
        lr.SetPosition(1, part.transform.position);
        //lr.SetPosition(1, Camera.main.transform.position);

        labelPrefab.SetActive(true);

    }

    private void HideLabel()
    {
        labelPrefab.SetActive(false);
    }
}
