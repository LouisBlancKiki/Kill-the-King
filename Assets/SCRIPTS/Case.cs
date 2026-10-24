using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static Elements;

public class Case : MonoBehaviour
{
    public GameManager GM;
    public PlateauManager PM;
    public PanneauActionManager PAM;

    public Joueur proprietaire_Case = Joueur.Game;

    public int coordX;
    public int coordY;

    public float posX;
    public float posY;

    public TypeTerrain case_TypeTerrain;
    public TypeTuile case_TypeTuile;
    public int pointDeVieMax;
    public bool caseEvolutive;

    public float niveau;
    public TMP_Text txtNiveau;

    public int vie;
    public TMP_Text txtVie;

    public bool estUnGisement = false;
    public bool estOccupee = false;
    public bool estSelectionable = false;
    public bool estSelectionee = false;

    //public GameObject tuile;
    public GameObject case_Cadre_Couleur_Joueur;
    public GameObject case_LumSelect;

    
    private void Start()
    {
        GM.Event_TourSuivant += FaireEvoluerCase;
    }

    private void OnDestroy()
    {
        GM.Event_TourSuivant -= FaireEvoluerCase;
    }

    public void InitialiserTerrain(TypeTerrain typeTerrainCible)
    {
        Terrain_Data terrain_Data = PM.GetTerrainData(typeTerrainCible);
        transform.Find("terrain").GetComponent<SpriteRenderer>().sprite = terrain_Data.sprite[0];
        case_TypeTerrain = typeTerrainCible;
        estUnGisement = terrain_Data.gisement;
        
    }

    public void InitialiserTuile(Joueur joueurCible, TypeTuile typeTuileCible)
    {
        case_TypeTuile = typeTuileCible;
        proprietaire_Case = joueurCible;
        estOccupee = true;

        Tuile_Data tuiledata = PM.GetTuileData(case_TypeTuile);

        
        pointDeVieMax = tuiledata.pointDeVieMax;
        vie = pointDeVieMax;

        niveau = 0;

        mettreAJourCase();
    }

    private void OnMouseDown()
    {
        PAM.DemanderAction(this);
    }

    public void DefinirTypeTuile(TypeTuile tuileCible)
    {
        case_TypeTuile = tuileCible;
        mettreAJourCase();
    }

    public void mettreAJourCase()
    {
        MiseAJourVisuelCase();
        DefinirProprietaire(proprietaire_Case);
        DefinirSelectionable(false);
    }

    void DefinirProprietaire(Joueur joueurCible)
    {
        if (joueurCible == Joueur.P1)
        {
            estOccupee = true;
            case_Cadre_Couleur_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_P1;
        }
        else if (joueurCible == Joueur.P2)
        {
            estOccupee = true;
            case_Cadre_Couleur_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_P2;
        }
        else if (joueurCible == Joueur.Game)
        {
            estOccupee = false;
            case_Cadre_Couleur_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_Game;
        }
    }

    public void MiseAJourVisuelCase()
    {
        //VERIFICATION
        Tuile_Data tuile_Data = PM.GetTuileData(case_TypeTuile);

        if (tuile_Data == null)
        {
            Debug.Log($"<b><color=#ACFF80> !!! Tuile_Data {case_TypeTuile} est NULL </color></b>");
            return;
        }

        if (tuile_Data.liste_SpriteSelonNiveau.Count == 0)
        {
            Debug.Log($"<b><color=#ACFF80> !!! La liste_SpriteSelonNiveau de { tuile_Data.name} est vide</color></b>");
            return;
        }

        if (case_TypeTuile == TypeTuile.Vide)
        {
            proprietaire_Case = Joueur.Game;
        }

        int index = Mathf.Clamp((int)niveau, 0, tuile_Data.liste_SpriteSelonNiveau.Count - 1);
        Sprite sprite = tuile_Data.liste_SpriteSelonNiveau[index];

        caseEvolutive = PM.GetTuileData(case_TypeTuile).estEvolutif;
        transform.Find("tuile").GetComponent<SpriteRenderer>().sprite = sprite;
        txtVie.text = vie.ToString();
        txtNiveau.text = niveau.ToString();

        /*
         Debug.Log("<b><color=#ACFF80> /// Mise à jour du visuel de la tuile ("
            + coordX + "/" + coordY + ") :"
            + PM.GetTuileData(case_TypeTuile).type + " ( " + proprietaire
            + " ) - ( NIV: " + niveau
            + " ) - ( PV: " + vie
            + " )</color></b>");
        */
    }

    public void DefinirSelectionable(bool selectionable)
    {
        case_LumSelect.SetActive(selectionable);
        GetComponent<BoxCollider2D>().enabled = selectionable;
    }

    public void FaireEvoluerCase()
    {
        if (case_TypeTuile != TypeTuile.Vide)
        {
            int niv_Max = PM.GetTuileData(case_TypeTuile).tuileNiveauMaximum;

            if (niveau < niv_Max && caseEvolutive)
            {
                niveau += 0.5f;

                MiseAJourVisuelCase();

                Debug.Log("<b><color=#ACFF80> /// Evolution de la case ("
                    + coordX + "/" + coordY + ") :"
                    + PM.GetTuileData(case_TypeTuile).type + " ( " + proprietaire_Case
                    + " ) - ( NIV: " + niveau
                    + " ) - ( PV: " + vie
                    + " )</color></b>");
            }

            
        }
        
    }
}
