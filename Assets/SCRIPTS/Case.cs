using TMPro;
using UnityEngine;
using static Elements;

public class Case : MonoBehaviour
{
    public GameManager GM;
    public PlateauManager PM;

    public Joueur proprietaire = Joueur.Game;

    public int coordX;
    public int coordY;

    public TypeTuile type;
    public int pointDeVieMax;
    public bool caseEvolutive;

    public float niveau;
    public TMP_Text txtNiveau;
    
    public int vie;
    public TMP_Text txtVie;

    public bool estOccupee = false;
    public bool estSelectionable = false;

    public GameObject image;
    public GameObject cadre_couleurJoueur_Case;
    
    private void Awake()
    {
        
    }

    public void InitialiserTuile(Joueur joueurCible,TypeTuile nouveauTypeTuile)
    {
        type = nouveauTypeTuile;
        proprietaire = joueurCible;

        Tuile_Data tuiledata = PM.GetTuileData(type);
        
        pointDeVieMax = tuiledata.pointDeVieMax;
        vie = pointDeVieMax;
        
        niveau = 0;
        
        if (type == TypeTuile.Prairie)
        {
            estOccupee = false;
        }
        else
        {
            estOccupee = true;
        }

        mettreAJourVisuelCase();
    }

    public void mettreAJourVisuelCase()
    {
        MiseAJourVisuelProprietaire(proprietaire);
        MiseAJourVisuelTuile();
    }

    void MiseAJourVisuelProprietaire(Joueur joueurCible)
    {
        if (joueurCible == Joueur.P1)
        {
            cadre_couleurJoueur_Case.GetComponent<SpriteRenderer>().color = GM.couleur_P1;
        }
        else if (joueurCible == Joueur.P2)
        {
            cadre_couleurJoueur_Case.GetComponent<SpriteRenderer>().color = GM.couleur_P2;
        }
        else if (joueurCible == Joueur.Game)
        {
            cadre_couleurJoueur_Case.GetComponent<SpriteRenderer>().color = GM.couleur_Game;
        }
    }
    
    public void MiseAJourVisuelTuile()
    {
        //VERIFICATION
        Tuile_Data data = PM.GetTuileData(type);

        if (data == null)
        {
            Debug.LogError("DATA NULL pour : " + type);
            return;
        }

        if (data.prefabsSelonNiveau == null || data.prefabsSelonNiveau.Length == 0)
        {
            Debug.LogError("Aucun prefab dans : " + data.name);
            return;
        }

        int index = Mathf.Clamp((int)niveau, 0, data.prefabsSelonNiveau.Length - 1);

        GameObject prefab = data.prefabsSelonNiveau[index];

        if (prefab == null)
        {
            Debug.LogError("Prefab NULL index " + index + " dans " + data.name);
            return;
        }

        //MISE A JOUR DU VISUEL
        Instantiate(prefab, image.transform.position, Quaternion.identity);
        txtVie.text = vie.ToString();
        txtNiveau.text = niveau.ToString();

        Debug.Log("<b><color=#666666> /// Mise à jour du visuel de la tuile ("
            + coordX + "/" + coordY + ") :"
            + PM.GetTuileData(type).type + " ( " + proprietaire
            + " ) - ( NIV: " + niveau
            + " ) - ( PV: " + vie
            + " )</color></b>");
    }
}
