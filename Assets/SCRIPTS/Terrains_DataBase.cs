using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase_Terrains", menuName = "Terrain/nouvelle DataBase de terrains")]
public class Terrains_DataBase : ScriptableObject
{
    public List<Terrain_Data> liste_Terrains;
}