using UnityEngine;
using static Elements;

public class GameManager : MonoBehaviour
{
    // MANAGERS
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]

    public AccueilManager AM;
    public InterfaceManager IM;
    public PlateauManager PM;


    [Space(10)]

    // COULEURS JOUEURS
    [Header("<b><color=#FFDC99>COULEURS JOUEURS</color></b>")]

    public Color[] listeCouleurJoueur;


    [Space(10)]

    // GAME
    [Header("<b><color=#FFDC99>GAME</color></b>")]
    public Color couleur_Game;
    private Joueur _joueurActif;
    public Joueur joueurActif
    {
        get => _joueurActif;
        set
        {
            _joueurActif = value;
            MiseAJourJoueurActif();
        }
    }
    public int prochainJoueur;


    [Space(10)]

    // P1
    [Header("<b><color=#FFDC99>P1</color></b>")]
    public Color couleur_P1;

    [Space(10)]


    // P2
    [Header("<b><color=#FFDC99>P2</color></b>")]
    public Color couleur_P2;

    [Space(10)]

    // COMPO DU DECK AU DEPART
    [Header("<b><color=#FFDC99>COMPO DU DECK AU DEPART</color></b>")]
    public int nbCarte_AuDepart_Paysan;
    public int nbCarte_AuDepart_Bucheron;
    public int nbCarte_AuDepart_Mineur;
    public int nbCarte_AuDepart_Marchand;
    public int nbCarte_AuDepart_Tavernier;
    public int nbCarte_AuDepart_Charpentier;
    public int nbCarte_AuDepart_Batisseur;
    public int nbCarte_AuDepart_Architecte;
    public int nbCarte_AuDepart_Forgeron;
    public int nbCarte_AuDepart_Aumonier;
    public int nbCarte_AuDepart_Philosophe;
    public int nbCarte_AuDepart_Mendiant;
    public int nbCarte_AuDepart_Artilleur;
    public int nbCarte_AuDepart_Bourreau;

    [Space(10)]

    // RESSOURCES AU DEPART
    [Header("<b><color=#FFDC99>RESSOURCES AU DEPART</color></b>")]
    public int nbPieces_AuDepart;
    public int nbNourritures_AuDepart;
    public int nbBois_AuDepart;
    public int nbPierres_AuDepart;
    public int nbCharbons_AuDepart;
    public int nbMetals_AuDepart;


    public int pointDeVieChateauMax;

    [Space(10)]

    // DONNEES DE DEBUT DE TOUR
    [Header("<b><color=#FFDC99>DONNEES DE DEBUT DE TOUR</color></b>")]
    public int nbCartePiocheesAuDebutDuTour;
    public int nbActionAuDebutDuTour;

    [Space(10)]

    // PLAFOND RESSOURCES
    [Header("<b><color=#FFDC99>PLAFOND RESSOURCES</color></b>")]
    public int limiteDeTirage;
    public int limiteDeAction;
    public int limiteDePiece;
    public int limiteDeRessource;

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    void Start()
    {
        AM.pageAcceuil.SetActive(true);
        joueurActif = Joueur.Game;
        prochainJoueur = 1;
        //prochainJoueur = Random.Range(1, 3);
    }

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    private void MiseAJourJoueurActif()
    {
        IM.AfficherEnvironnementJoueur(joueurActif);
        Debug.Log("<b><color=#FFCE80>-> JoueurActif = " + joueurActif + "</color></b>");
    }

    private void DefinirCouleurDesJoueurs()
    {
        couleur_P1 = listeCouleurJoueur[AM.numCouleurSelectionee_P1 - 1];
        couleur_P2 = listeCouleurJoueur[AM.numCouleurSelectionee_P2 - 1];
    }

    public void InitialiserNouvellePartie()
    {
        DefinirCouleurDesJoueurs();
        PM.InitialiserPlateauDeJeu();


        AM.OuvertureDesVoletsAcceuil();
        Invoke("ChangementDeJoueur", 1.5f);
    }

    public void ChangementDeJoueur()
    {
        Debug.Log("<b><color=#FFA500>=== CHANGEMENT DE JOUEUR ===</color></b>");
        Invoke("StartNextPlayer", 0.2f);
    }

    public void StartNextPlayer()
    {
        if (prochainJoueur == 1)
        {
            joueurActif = Joueur.P1;
            //GD2P.premièrePioche_P1 = true;
            //GD2P.MettreAJourDecksActifs();
            //GD2P.MettreAJourDonneeDeDebutDeTour();
            prochainJoueur = 2;
        }

        else if (prochainJoueur == 2)
        {
            joueurActif = Joueur.P2;
            //GD2P.premièrePioche_P2 = true;
            //GD2P.MettreAJourDecksActifs();
            //GD2P.MettreAJourDonneeDeDebutDeTour();
            prochainJoueur = 1;
        }

        IM.nbTourDeJeu++;

        if (IM.nbTourDeJeu == 1)
        {
            //InitialiserDonnéeDesJoueursAuPremierTours();
        }

        IM.Afficher_nbTourDeJeu();
        //ActiverLeBoutonFinDeTour();
        //AfficherCadreAQuiLeTour();
        //Event_TourSuivant?.Invoke();
        //GD2P.LancerDebutDeTour();
    }
}
