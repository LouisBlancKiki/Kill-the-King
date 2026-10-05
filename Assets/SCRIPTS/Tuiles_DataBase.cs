using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tuiles_DataBase", menuName = "Tuiles/nouvelle DataBase de tuiles")]
public class Tuiles_DataBase : ScriptableObject
{
    public List<Tuile_Data> tuiles_Liste;
}