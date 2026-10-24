using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static Elements;


public class PlateauManager : MonoBehaviour
{
    // MANAGERS
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;

    [Space(10)]

    // PARAMETRES DU PLATEAU
    [Header("<b><color=#FFDC99>PARAMETRES DU PLATEAU</color></b>")]
    public int nbColonnes;
    public int nbLignes;
    public float largeur_Case;
    public float hauteur_Case;
    public Vector3 plateauAncrage;
    public Case[,] grillePlateau; // tableau pour stocker les cases
    public GameObject casePlateau_Prefab;
    public Tuiles_DataBase tuilesDatabase;
    public Terrains_DataBase terrainsDatabase;

    [Space(10)]

    // PARAMETRES DE PLACEMENT DES GISEMENTS
    [Header("<b><color=#FFDC99>PARAMETRES DU PLATEAU</color></b>")]
    HashSet<Vector2Int> casesAvecGisement = new HashSet<Vector2Int>();


    // ============================================================
    // INITIALISATION DU PLATEAU DE JEU
    // ============================================================
    public void InitialiserPlateauDeJeu()
    {
        Debug.Log("<b><color=#ACFF80> -> Initialisation du plateau de jeu</color></b>");
        grillePlateau = CreerGrilleDeCase(nbColonnes, nbLignes);
        ConstruirePlateaudeDepart();
    }

    public Case[,] CreerGrilleDeCase(int colonnes, int lignes)
    {
        Case[,] nouvelleGrille = new Case[colonnes, lignes];

        for (int x = 0; x < colonnes; x++)
        {
            for (int y = 0; y < lignes; y++)
            {
                GameObject caseGO = caseGO = Instantiate(casePlateau_Prefab);
                Case caseScript = caseGO.GetComponent<Case>();

                caseScript.GM = GM;
                caseScript.PM = this;
                caseScript.PAM = GM.PAM;

                caseScript.coordX = x;
                caseScript.coordY = y;
                caseScript.posX = x * largeur_Case;
                caseScript.posY = y * hauteur_Case;

                Vector3 positionCase = new Vector3(caseScript.posX, -caseScript.posY, 0) + plateauAncrage;
                caseGO.transform.position = positionCase;

                caseScript.case_Cadre_Couleur_Joueur = caseGO.transform.Find("Cadre_Couleur_Joueur").gameObject;
                caseScript.case_Cadre_Couleur_Joueur.GetComponent<SpriteRenderer>().color = GM.couleur_Game;

                caseScript.case_LumSelect = caseGO.transform.Find("case_LumSelect").gameObject;
                caseScript.DefinirSelectionable(false);

                GM.RM.AjouterCase(Joueur.Game, caseScript);

                nouvelleGrille[x, y] = caseScript;
            }
        }

        return nouvelleGrille;
    }

    void ConstruirePlateaudeDepart()
    {
        PlacerTerrainPrairieDuDebut();
        
        PlacerTerrainGisementDuDebut();

        PlacerTuilesChateauDuDebut(Joueur.P1);
        PlacerTuilesChateauDuDebut(Joueur.P2);
    }

    void PlacerTerrainPrairieDuDebut()
    {
        for (int x = 0; x < grillePlateau.GetLength(0); x++)
        {
            for (int y = 0; y < grillePlateau.GetLength(1); y++)
            {
                Case caseCible = grillePlateau[x, y];

                if (caseCible.estOccupee)
                    continue;

                caseCible.InitialiserTerrain(TypeTerrain.Prairie);
            }
        }
    }

    void PlacerTuilesChateauDuDebut(Joueur proprietaire)
    {
        int colonne;

        // joueur 1 à gauche
        if (proprietaire == Joueur.P1)
        {
            colonne = 0;
        }
        // joueur 2 à droite
        else
        {
            colonne = nbColonnes - 1;
        }

        Case caseChateau_Haut = grillePlateau[colonne, 2];
        Case caseChateau_Centre = grillePlateau[colonne, 3];
        Case caseChateau_Bas = grillePlateau[colonne, 4];

        GM.RM.TransfererCase(caseChateau_Haut, proprietaire);
        GM.RM.TransfererCase(caseChateau_Centre, proprietaire);
        GM.RM.TransfererCase(caseChateau_Bas, proprietaire);

        caseChateau_Haut.InitialiserTuile(proprietaire, TypeTuile.AileDeChateau);
        caseChateau_Centre.InitialiserTuile(proprietaire, TypeTuile.Chateau);
        caseChateau_Bas.InitialiserTuile(proprietaire, TypeTuile.AileDeChateau);

    }

    void PlacerTerrainGisementDuDebut()
    {
        HashSet<Vector2Int> casesUtilisees = new HashSet<Vector2Int>();

        // ZONE 1 : ligne 0 et 1, colonnes 0 à 6
        PlacerGisementsDansLesZone(0, 1, 0, 6, -1, false, casesUtilisees);

        // ZONE 2 : ligne 0 et 1, colonnes 9 à 13
        PlacerGisementsDansLesZone(0, 1, 9, 13, 1, false, casesUtilisees);

        // ZONE 3 : lignes 2 à 4, colonnes 1 à 6
        PlacerGisementsDansLesZone(2, 4, 1, 6, -1, true, casesUtilisees);

        // ZONE 4 : lignes 2 à 4, colonnes 9 à 12
        PlacerGisementsDansLesZone(2, 4, 9, 12, 1, true, casesUtilisees);

        // ZONE 5 : ligne 5 et 6, colonnes 0 à 6
        PlacerGisementsDansLesZone(5, 6, 0, 6, -1, false, casesUtilisees);

        // ZONE 6 : ligne 5 et 6, colonnes 9 à 13
        PlacerGisementsDansLesZone(5, 6, 9, 13, 1, false, casesUtilisees);
    }

    void PlacerGisementsDansLesZone(int ligneMin, int ligneMax, int colonneMin, int colonneMax, int pierreSens, bool uneCaseParLigne, HashSet<Vector2Int> casesUtilisees)
    {
        List<Vector2Int> casesDisponibles = new List<Vector2Int>();

        // Récupérer les cases libres de la zone
        for (int y = ligneMin; y <= ligneMax; y++)
        {
            for (int x = colonneMin; x <= colonneMax; x++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (!grillePlateau[x, y].estOccupee && !casesUtilisees.Contains(position))
                    casesDisponibles.Add(position);
            }
        }

        // Mélanger les cases disponibles
        for (int i = casesDisponibles.Count - 1; i > 0; i--)
        {
            int index = Random.Range(0, i + 1);
            Vector2Int temporaire = casesDisponibles[i];
            casesDisponibles[i] = casesDisponibles[index];
            casesDisponibles[index] = temporaire;
        }

        // Sélectionner 3 cases différentes
        List<Vector2Int> casesChoisies = new List<Vector2Int>();

        foreach (Vector2Int position in casesDisponibles)
        {
            if (uneCaseParLigne && casesChoisies.Exists(p => p.y == position.y))
                continue;

            casesChoisies.Add(position);

            if (casesChoisies.Count == 3)
                break;
        }

        List<TypeTerrain> gisements = new List<TypeTerrain>
    {
        TypeTerrain.GisementDePierre,
        TypeTerrain.GisementDeCharbon,
        TypeTerrain.GisementDeMetal
    };

        // Placer la pierre sur la colonne extrême
        int indexPierre = 0;

        for (int i = 1; i < casesChoisies.Count; i++)
        {
            if ((pierreSens == -1 && casesChoisies[i].x < casesChoisies[indexPierre].x) ||
                (pierreSens == 1 && casesChoisies[i].x > casesChoisies[indexPierre].x))
            {
                indexPierre = i;
            }
        }

        Vector2Int positionPierre = casesChoisies[indexPierre];

        grillePlateau[positionPierre.x, positionPierre.y].InitialiserTerrain(TypeTerrain.GisementDePierre);
        casesUtilisees.Add(positionPierre);
        casesChoisies.RemoveAt(indexPierre);
        gisements.Remove(TypeTerrain.GisementDePierre);

        // Placer le charbon et le métal aléatoirement
        foreach (Vector2Int position in casesChoisies)
        {
            int index = Random.Range(0, gisements.Count);
            TypeTerrain type = gisements[index];

            grillePlateau[position.x, position.y].InitialiserTerrain(type);
            casesUtilisees.Add(position);
            gisements.RemoveAt(index);
        }
    }

    // ============================================================
    // ACCES AU DONNEES DES TUILES
    // ============================================================
    public Terrain_Data GetTerrainData(TypeTerrain type)
    {
        Terrain_Data data = terrainsDatabase.liste_Terrains.Find(t => t.type == type);

        if (data == null)
            Debug.LogError("Aucune TerrainData pour : " + type);

        return data;
    }

    public Tuile_Data GetTuileData(TypeTuile type)
    {
        Tuile_Data data = tuilesDatabase.liste_Tuiles.Find(t => t.type == type);

        if (data == null)
            Debug.LogError("Aucune TuileData pour : " + type);

        return data;
    }

    // ============================================================
    // SELECTION DE CASE
    // ============================================================

    public bool EstInoccupeeEtAdjacenteAUneCaseOccupee(Case[,] grille, int x, int y, Joueur joueur)
    {
        int colonnes = grille.GetLength(0);
        int lignes = grille.GetLength(1);

        Case caseCible = grille[x, y];

        if (caseCible.estOccupee)
            return false;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int voisinX = x + dx;
                int voisinY = y + dy;

                if (voisinX < 0 || voisinX >= colonnes || voisinY < 0 || voisinY >= lignes)
                    continue;

                Case caseVoisine = grille[voisinX, voisinY];

                if (caseVoisine.estOccupee && caseVoisine.proprietaire_Case == joueur)
                    return true;
            }
        }

        return false;
    }

    public void DefinirCaseSelectionables()
    {
        int colonnes = grillePlateau.GetLength(0);
        int lignes = grillePlateau.GetLength(1);

        Debug.Log($"<b><color=#ACFF80> -> Eclairage des cases selectionables pour l'action : {GM.PAM.actionSelectionee} </color></b>");
        

        //RendreToutesLesCases_NonSelectionables();
        switch (GM.PAM.actionSelectionee)
        {
            case Action.Semer :
            case Action.Planter:
            case Action.Construire_Canon:
            case Action.Construire_Chapelle:
            case Action.Construire_Entrepot:
            case Action.Construire_Forge:
            case Action.Construire_Maison:
            case Action.Construire_Marche:
            case Action.Construire_Monument:
            case Action.Construire_Moulin:
            case Action.Construire_Rempart:
            case Action.Construire_Taverne:
            case Action.Construire_Tribunal:

                for (int x = 0; x < colonnes; x++)
                {
                    for (int y = 0; y < lignes; y++)
                    {
                        Case caseCible = grillePlateau[x, y];

                        if (EstInoccupeeEtAdjacenteAUneCaseOccupee(grillePlateau, x, y, GM.joueurActif))
                        {
                            caseCible.GetComponent<Case>().DefinirSelectionable(true);
                        }
                    }
                }
                break;

            case Action.Creuser:
                
                for (int x = 0; x < colonnes; x++)
                {
                    for (int y = 0; y < lignes; y++)
                    {
                        Case caseCible = grillePlateau[x, y];

                        if (EstInoccupeeEtAdjacenteAUneCaseOccupee(grillePlateau, x, y, GM.joueurActif) && caseCible.estUnGisement)
                        {
                            caseCible.GetComponent<Case>().DefinirSelectionable(true);
                        }
                    }
                }
                break;

            case Action.Recolter:

                foreach (Case caseCible in GM.RM.ObtenirRegistreCases(GM.joueurActif))
                {
                    if (caseCible.case_TypeTuile == TypeTuile.Champ && caseCible.niveau >= GetTuileData(caseCible.case_TypeTuile).tuileNiveauMinimumRecoltable)
                    {
                        caseCible.DefinirSelectionable(true);
                    }
                }
                break;

            case Action.Couper:

                foreach (Case caseCible in GM.RM.ObtenirRegistreCases(GM.joueurActif))
                {
                    if (caseCible.case_TypeTuile == TypeTuile.Foret && caseCible.niveau >= GetTuileData(caseCible.case_TypeTuile).tuileNiveauMinimumRecoltable)
                    {
                        caseCible.DefinirSelectionable(true);
                    }
                }
                break;

            case Action.Extraire:
                // A DEFINIR
                break;
        }
    }

    public void RendreToutesLesCases_NonSelectionables()
    {
        int colonnes = grillePlateau.GetLength(0);
        int lignes = grillePlateau.GetLength(1);
        for (int x = 0; x < colonnes; x++)
        {
            for (int y = 0; y < lignes; y++)
            {
                grillePlateau[x, y].GetComponent<Case>().DefinirSelectionable(false);
            }
        }
    }

    // ============================================================
    // ACTIONS
    // ============================================================

    public void PlacerTuile(Case caseCible, TypeTuile tuileCible)
    {
        caseCible.proprietaire_Case = GM.joueurActif;
        GM.RM.TransfererCase(caseCible, GM.joueurActif);
        caseCible.DefinirTypeTuile(tuileCible);
    }

    public void SuprimerTuile(Case caseCible)
    {
        caseCible.proprietaire_Case = Joueur.Game;
        GM.RM.TransfererCase(caseCible, Joueur.Game);
        caseCible.DefinirTypeTuile(TypeTuile.Vide);
        
    }
}
