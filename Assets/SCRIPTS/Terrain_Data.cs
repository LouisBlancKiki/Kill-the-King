using UnityEngine;
using static Elements;

[CreateAssetMenu(fileName = "Terrain_Data_", menuName = "Terrain/nouveau terrain")]
public class Terrain_Data : ScriptableObject
{

    public TypeTerrain type;
    public bool gisement;
    
    [Header("<b><color=#FFDC99>VISUEL</color></b>")]
    public Sprite[] sprite;

    [Space(10)]
    public GameObject terrainGameObject;
}