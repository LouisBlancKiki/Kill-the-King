using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Elements;

public class GameManager : MonoBehaviour
{
    // MANAGERS
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]

    public AccueilManager AM;
    public InterfaceManager IM;
    public PlateauManager PM;
    public DeckManager DM;
    public MainManager MM;
    public TableauManager TM;
    public BoutonManager BM;

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
    public Sprite imageJoueur_P1;
    public string nomJoueur_P1;
    public GameObject boutonFinDeTour_P1;

    [Space(10)]

    // P2
    [Header("<b><color=#FFDC99>P2</color></b>")]
    public Color couleur_P2;
    public Sprite imageJoueur_P2;
    public string nomJoueur_P2;
    public GameObject boutonFinDeTour_P2;

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
        prochainJoueur = 2;
        //prochainJoueur = Random.Range(1, 3);
    }

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    private void MiseAJourJoueurActif()
    {
        IM.AfficherEnvironnementJoueur(joueurActif);
        Debug.Log($"<b><color=#000000>/// JoueurActif = { joueurActif } </color></b>");
    }

    private void DefinirIdentiteDesJoueurs()
    {
        // P1
        couleur_P1 = listeCouleurJoueur[AM.numCouleurSelectionee_P1 - 1];
        imageJoueur_P1 = AM.blason_Roi_P1.GetComponent<SpriteRenderer>().sprite;
        
        if (!string.IsNullOrWhiteSpace(AM.champNom_P1.text))
        {
            nomJoueur_P1 = AM.champNom_P1.text;
        }
        else
        {
            nomJoueur_P1 = "Joueur 1";
        }

        // P2
        couleur_P2 = listeCouleurJoueur[AM.numCouleurSelectionee_P2 - 1];
        imageJoueur_P2 = AM.blason_Roi_P2.GetComponent<SpriteRenderer>().sprite;

        if (!string.IsNullOrWhiteSpace(AM.champNom_P2.text))
        {
            nomJoueur_P2 = AM.champNom_P2.text;
        }
        else
        {
            nomJoueur_P2 = "Joueur 2";
        }
    }

    public void InitialiserNouvellePartie()
    {
        DefinirIdentiteDesJoueurs();
        DefinirAccesDesBoutonsFinDeTour();
        IM.Afficher_ProfilDesJoueurs();
        PM.InitialiserPlateauDeJeu();
        DM.InitialiserDeckDesJoueurs();


        AM.OuvertureDesVoletsAcceuil();
        Invoke("ChangementDeJoueur", 1.5f);
    }

    public void ChangementDeJoueur()
    {
        Debug.Log("<b><color=#000000>=== CHANGEMENT DE JOUEUR ===</color></b>");
        RedeinirLeJoueurActif();
        TM.AfficherTableau_DebutDeTour();
    }

    public void RedeinirLeJoueurActif()
    {
        if (prochainJoueur == 1)
        {
            joueurActif = Joueur.P1;
            prochainJoueur = 2;
        }

        else if (prochainJoueur == 2)
        {
            joueurActif = Joueur.P2;
            prochainJoueur = 1;
        }
    }

    public void ConfirmationJoueurPret()
    {
        TM.MasquerTableau_DebutDeTour();
        Invoke("DemarrerTourDuProchainJoueur", 0.2f);
    }

    public void DemarrerTourDuProchainJoueur()
    {
        IM.nbTourDeJeu++;

        IM.Afficher_nbTourDeJeu();
        DefinirAccesDesBoutonsFinDeTour();
        LancerDebutDeTour();
    }

    public void LancerDebutDeTour()
    {
        MM.piocheDeDebutDeTour();
    }
    public void DefinirAccesDesBoutonsFinDeTour()
    {
        if (joueurActif == Joueur.P1)
        {
            boutonFinDeTour_P1.GetComponent<BoxCollider2D>().enabled = true;
            boutonFinDeTour_P1.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Accessible;
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().AfficherNbDeSupportLumaction(GetScoreActif().nbActions);
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(true);

            boutonFinDeTour_P2.GetComponent<BoxCollider2D>().enabled = false;
            boutonFinDeTour_P2.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Inaccessible;
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(false);
        }
        else if (joueurActif == Joueur.P2)
        {
            boutonFinDeTour_P2.GetComponent<BoxCollider2D>().enabled = true;
            boutonFinDeTour_P2.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Accessible;
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().AfficherNbDeSupportLumaction(GetScoreActif().nbActions);
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(true);

            boutonFinDeTour_P1.GetComponent<BoxCollider2D>().enabled = false;
            boutonFinDeTour_P1.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Inaccessible;
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(false);
        }
        else if (joueurActif == Joueur.Game)
        {
            boutonFinDeTour_P1.GetComponent<BoxCollider2D>().enabled = false;
            boutonFinDeTour_P1.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Inaccessible;
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().MasquerTousLesSupportsLumaction();
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().MasquerToutesLesLumieresLumaction();
            //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(false);

            boutonFinDeTour_P2.GetComponent<BoxCollider2D>().enabled = false;
            boutonFinDeTour_P2.GetComponent<SpriteRenderer>().color = BM.couleur_Bout_Inaccessible;
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().MasquerTousLesSupportsLumaction();
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().MasquerToutesLesLumieresLumaction();
            //boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().PS_etincelleBouton.SetActive(false);
        }
    }

    public void DemanderLaFinDeTour()
    {
        AttendreFinDeMouvementEtFinaliserLeTour();
    }

    public void AttendreFinDeMouvementEtFinaliserLeTour()
    {
        StartCoroutine(AttendreFinDeMouvementEtFinaliserLeTour_Coroutine());
    }

    private IEnumerator AttendreFinDeMouvementEtFinaliserLeTour_Coroutine()
    {
        //yield return GD2P.StartCoroutine(GD2P.VerifieSiUneCarteBouge_Coroutine(GD2P.jeu_Main));
        yield return null;
        Invoke("FinaliserLeTour", 0.5f);
    }

    public void FinaliserLeTour()
    {
        //GD2P.estEnFinDeTour = true;
        StartCoroutine(FinaliserLeTour_Coroutine());
    }

    private IEnumerator FinaliserLeTour_Coroutine()
    {
        MM.DefausserMainDuJoueur();
        //PAM.FermerLePanneauAction();
        //PM.RendreToutesLesCases_NonSelectionables();

        //yield return StartCoroutine(GD2P.DefausserLaMainEnFinDeTour_Coroutine());

        //yield return StartCoroutine(GD2P.AttendreFinMouvementCartes_Coroutine());

        //boutonFinDeTour_P1.GetComponent<Bouton_FinDeTour>().AfficherNbDeSupportLumaction(0);
        // boutonFinDeTour_P2.GetComponent<Bouton_FinDeTour>().AfficherNbDeSupportLumaction(0);
        joueurActif = Joueur.Game;
        DefinirAccesDesBoutonsFinDeTour();
        yield return new WaitForSeconds(0.2f);

        ChangementDeJoueur();
    }
}
