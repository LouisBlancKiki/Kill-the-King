using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cartes_DataBase", menuName = "Cartes/nouvelle DataBase de cartes")]
public class Cartes_DataBase : ScriptableObject
{
    public List<Carte_Data> cartes_Liste;
}