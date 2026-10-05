using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;


public class PlateauManager : MonoBehaviour
{
    // MANAGERS
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;
    public InterfaceManager IM;
    
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
    public Tuiles_DataBase tuileDatabase;


    public Tuile_Data GetTuileData(TypeTuile type)
    {
        Tuile_Data data = tuileDatabase.tuiles_Liste.Find(t => t.type == type);

        if (data == null)
            Debug.LogError("Aucune TuileData pour : " + type);

        return data;
    }

    public void InitialiserPlateauDeJeu()
    {
        grillePlateau = CreerGrilleDeCase(nbColonnes, nbLignes);
        PlacerTuileDepart();
    }
    
    public Case[,] CreerGrilleDeCase(int colonnes, int lignes)
    {
        Case[,] nouvelleGrille = new Case[colonnes, lignes];

        for (int x = 0; x < colonnes; x++)
        {
            for (int y = 0; y < lignes; y++)
            {
                float posX = x * largeur_Case;
                float posY = y * hauteur_Case;
                Vector3 positionCase = new Vector3(posX, -posY, 0) + plateauAncrage;

                GameObject caseGO = Instantiate(casePlateau_Prefab, positionCase, Quaternion.identity);

                Case caseScript = caseGO.GetComponent<Case>();
                caseScript.coordX = x;
                caseScript.coordY = y;

                caseScript.GM = GM;
                caseScript.PM = this;

                nouvelleGrille[x, y] = caseScript;
            }
        }

        return nouvelleGrille;
    }
    
    void PlacerTuileDepart()
    {
        PlacerTuilesChateauDuDebut(Joueur.P1);
        PlacerTuilesChateauDuDebut(Joueur.P2);
        PlacerTuilesGisementDuDebut();
        PlacerTuilesPrairieDuDebut();
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

        int ligne = nbLignes / 2;

        int x1 = ligne - 1;
        int x2 = ligne;
        int x3 = ligne + 1;

        Case caseChateau_gauche = grillePlateau[colonne, x1];
        Case caseChateau_centre = grillePlateau[colonne, x2];
        Case caseChateau_droite = grillePlateau[colonne, x3];

        caseChateau_gauche.InitialiserTuile(proprietaire,TypeTuile.AileDeChateau);
        caseChateau_centre.InitialiserTuile(proprietaire, TypeTuile.Chateau);
        caseChateau_droite.InitialiserTuile(proprietaire, TypeTuile.AileDeChateau);
    }

    void PlacerTuilesGisementDuDebut()
    {
        Case caseCible = null;
        List<int> c;
        int c1;
        int c2;
        int c3;

        // Instalation des gisements P1
        c = new List<int> { 0, 1, 2, 3, 4 };

        c1 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c1);

        c2 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c2);

        c3 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c3);
               
        
        caseCible = grillePlateau[1, c1];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDePierre);

        caseCible = grillePlateau[2, c2];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDeCharbon);

        caseCible = grillePlateau[3, c3];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDeMetal);

        // Instalation des gisements P2
        c = new List<int> { 0, 1, 2, 3, 4 };

        c1 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c1);

        c2 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c2);

        c3 = c[UnityEngine.Random.Range(0, c.Count)];
        c.Remove(c3);


        caseCible = grillePlateau[8, c1];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDePierre);

        caseCible = grillePlateau[7, c2];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDeCharbon);

        caseCible = grillePlateau[6, c3];
        caseCible.InitialiserTuile(Joueur.Game, TypeTuile.GisementDeMetal);
    }

    void PlacerTuilesPrairieDuDebut()
    {
        for (int x = 0; x < grillePlateau.GetLength(0); x++)
        {
            for (int y = 0; y < grillePlateau.GetLength(1); y++)
            {
                Case caseCible = grillePlateau[x, y];

                if (caseCible.estOccupee)
                    continue;

                caseCible.InitialiserTuile(Joueur.Game, TypeTuile.Prairie);

            }
        }
    }
    
}
