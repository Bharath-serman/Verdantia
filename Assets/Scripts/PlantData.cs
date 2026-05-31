using UnityEngine;

public class PlantData : MonoBehaviour
{
    //Define the properties of th eplant.

    [Header("Name of the plant")]
    public string PlantName;
    [TextArea(3, 5)]
    public string PlantDescription;
}
