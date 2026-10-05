using UnityEngine;
using static Elements;

[CreateAssetMenu(fileName = "carte_Data_", menuName = "Cartes/nouvelle Carte")]
public class Carte_Data : ScriptableObject
{
    [Header("<b><color=#FFDC99>DEFINITION</color></b>")]
    public TypeCarte type;
    public NomCarte nom;
    
    [Space(10)]

    [Header("<b><color=#FFDC99>SPRITE</color></b>")]
    public Sprite sprite_Carte;
    public Sprite sprite_EntetePanneauAction;
    public Sprite sprite_BoutonRecrute;

    [Space(10)]

    [Header("<b><color=#FFDC99>COUT</color></b>")]
    public int prixEnPiece;
    [Space(10)]
    public int coutEnNourriture;
    public int coutEnBois;
    public int coutEnPierre;
    public int coutEnCharbon;
    public int coutEnMetal;

    [Space(10)]
    public GameObject carteGameObject;

}