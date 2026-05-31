using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class ARPlantManager : MonoBehaviour
{
    #region Inputs
    [Header("Inputs")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private List<GameObject> PlantPrefabs;
    bool ActiveStatus = false;
    float DestroyDuration = 10f;

    [Header("UI Elements")]
    [Tooltip("The Text UI elements like name and description of the plant.")]
    [SerializeField] private TMP_Text PlantName;
    [SerializeField] private TMP_Text PlantDescription;
    [SerializeField] private Image PlantImage;

    [Header("Panel UI")]
    [Tooltip("The Panel that stores the info about the plant")]
    [SerializeField] private GameObject DictionaryPanel;

    [Header("AR Raycast Hit")]
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    #endregion

    void Start()
    {
        //Hide the dictionary panel in the start.
        if (DictionaryPanel == null) return;
        else
            DictionaryPanel.SetActive(ActiveStatus);  //False.
    }

    void Update()
    {
        //Get the first touch input.
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        //Check the phase of the touch.
        if (touch.phase == TouchPhase.Began)
        {
            //Clicking on the existing plant
            if (SelectPlant(touch.position)) return;

            //Spanwing a new one on the plane.
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitpose = hits[0].pose;
                //Spawn a new plant.
                SpawnPlant(hitpose.position, hitpose.rotation);
            }
        }
    }
    #region PlantSpawnLogic
     void SpawnPlant(Vector3 position, Quaternion rotation)
    {
        if (PlantPrefabs == null || PlantPrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, PlantPrefabs.Count);
        GameObject SelectedPrefab = PlantPrefabs[randomIndex];

        //Instatiate the Random Plant onto the plane
        GameObject SpawnedPlant =  Instantiate(SelectedPrefab, position, rotation);

        //Destroy after some seconds.
        Destroy(SpawnedPlant, DestroyDuration);
    }

    #endregion

    #region Selecting Random Plant Logic.
    private bool SelectPlant(Vector2 touchposition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchposition);
        RaycastHit hitObject;

        if (Physics.Raycast(ray, out hitObject))
        {
            PlantData data = hitObject.transform.GetComponent<PlantData>();
            if (data != null)
            {
                DisplayUI(data);
                return true;
            }
        }
        return false;
    } 
    #endregion

    #region Display Dictionary Logic

    private void DisplayUI(PlantData data)
    {
        if(DictionaryPanel == null || PlantName == null || PlantDescription == null
            || PlantImage == null) return;

        PlantName.text = data.PlantName;
        PlantDescription.text = data.PlantDescription;
        PlantImage.sprite = data.PlantImage;

        //Enable the dictionary panel.
        DictionaryPanel.SetActive(!ActiveStatus);  //True.
    }
    #endregion

    #region Closing Dictionary Logic
    public void CloseUI()
    {
        if (DictionaryPanel == null) return;

        DictionaryPanel.SetActive(ActiveStatus);  //False.
    } 
    #endregion
}
