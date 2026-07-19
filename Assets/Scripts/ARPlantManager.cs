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
    string Treeprefabname = "Tree_";

    [Header("UI Elements")]
    [Tooltip("The Text UI elements like name and description of the plant.")]
    [SerializeField] private TMP_Text PlantName;
    [SerializeField] private TMP_Text PlantDescription;
    [SerializeField] private Image PlantImage;
    [SerializeField] private GameObject GardenButton;
    [SerializeField] private GameObject OceanButton;  //In the Ocean Level.

    public GameObject OceanInventoryPanel;

    //For Additional Props.
    [SerializeField] private GameObject SelectedProp;

    [Header("Panel UI")]
    [Tooltip("The Panel that stores the info about the plant")]
    [SerializeField] private GameObject DictionaryPanel;
    [Tooltip("The Inventory UI panel that stores the props prefabs")]
    [SerializeField] private GameObject InventoryPanel;

    [Header("AR Raycast Hit")]
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    #endregion


    //Selected Prefab Button Logic
    public void SelectedPrefab(GameObject prefab)
    {
        SelectedProp = prefab;
    }


    //Garden Button Logic
    public void GardenButtonLogic()
    {
        InventoryPanel.SetActive(!ActiveStatus);  //True.
    }

    public void OceanButtonLogic()
    {
        OceanInventoryPanel.SetActive(!ActiveStatus);  //True.
    }

    //Back Button Logic
    public void BackLogic()
    {
        //Disable the Inventory UI.
        InventoryPanel.SetActive(ActiveStatus);  //False.
    }

    public void OceanPanelBackLogic()
    {
        //Disable the Inventory UI.
        OceanInventoryPanel.SetActive(ActiveStatus);  //False.
    }


    void Start()
    {
        //Hide the dictionary panel in the start.
        if (DictionaryPanel == null) return;
        else
            DictionaryPanel.SetActive(ActiveStatus);  //False.

        InventoryPanel.SetActive(ActiveStatus);  //False.
        OceanInventoryPanel.SetActive(ActiveStatus);  //False.
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        // Mouse Input (Editor/PC)
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }

#else

    // Touch Input (Mobile)
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            HandleInput(touch.position);
        }
    }

#endif
    }

    private void HandleInput(Vector2 screenPosition)
{
    // Check if an existing plant was clicked
    if (SelectPlant(screenPosition))
        return;

    // Otherwise try placing a new plant
    if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
    {
        Pose hitPose = hits[0].pose;
        SpawnPlant(hitPose.position, hitPose.rotation);
    }
}


    #region PlantSpawnLogic
    void SpawnPlant(Vector3 position, Quaternion rotation)
    {
        if (PlantPrefabs == null || PlantPrefabs.Count == 0) return;

        Quaternion spawnrotation = rotation;

        int randomIndex = Random.Range(0, PlantPrefabs.Count);
        GameObject SelectedPrefab = PlantPrefabs[randomIndex];

        //Check if the selected prop was tree.
        if (SelectedPrefab.name == Treeprefabname)
            spawnrotation *= Quaternion.Euler(-90f, 0f, 0f);

        //Instatiate the Random Plant onto the plane
        GameObject SpawnedPlant = Instantiate(SelectedProp, position, spawnrotation);

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
        if (DictionaryPanel == null || PlantName == null || PlantDescription == null
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




