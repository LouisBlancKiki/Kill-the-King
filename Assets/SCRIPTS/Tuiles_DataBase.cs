using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase_Tuiles", menuName = "Tuiles/nouvelle DataBase de tuiles")]
public class Tuiles_DataBase : ScriptableObject
{
    public List<Tuile_Data> liste_Tuiles;
}