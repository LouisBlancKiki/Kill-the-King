using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class DeckManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;
    public MainManager MM;

    [Space(10)]

    [Header("<b><color=#FFDC99>CARTES</color></b>")]
    public Cartes_DataBase cartes_DataBase;
    public Sprite carte_Verso;
    

    [Space(10)]

    [Header("<b><color=#FFDC99>TOURBILLON</color></b>")]
    public Transform centre_Tourbillon;
    public float tempsEntreEjectionCartes;
    public float tempsDeplacementVersCercle;
    [Range(0f, 10f)]
    public int rayonCercleMelange;
    
    
    [Range(0f,360f)]
    public float vitesseTourbillon_Min;
    [Range(0f, 3600f)]
    public int vitesseTourbillon_Max;
    [Range(0f, 1000f)]
    public float accelerationTourbillon;
    [Range(0f,10f)]
    public int vitesseReductionRayon;
    [Range(0f, 1f)]
    public float tempsRetournementCarte;

    private int nombreCartesTerminees;


    // P1
    [Header("<b><color=#FFDC99>P1</color></b>")]
    public Transform zone_Deck_P1;
    public GameObject deck_P1;
    public List<Carte> liste_Deck_P1 = new List<Carte>();
    [Space(10)]
    public Transform zone_Defaussse_P1;
    public GameObject defausse_P1;
    public List<Carte> liste_Defausse_P1 = new List<Carte>();
    [Space(10)]
    public Transform zone_Main_P1;
    public GameObject main_P1;
    public List<Carte> liste_Main_P1 = new List<Carte>();
    [Space(10)]
    public Vector3 decalage_EpaisseurDeck_P1;

    [Space(10)]

    // P2
    [Header("<b><color=#FFDC99>P2</color></b>")]
    public Transform zone_Deck_P2;
    public GameObject deck_P2;
    public List<Carte> liste_Deck_P2 = new List<Carte>();
    [Space(10)]
    public Transform zone_Defaussse_P2;
    public GameObject defausse_P2;
    public List<Carte> liste_Defausse_P2 = new List<Carte>();
    [Space(10)]
    public Transform zone_Main_P2;
    public GameObject main_P2;
    public List<Carte> liste_Main_P2 = new List<Carte>();
    [Space(10)]
    public Vector3 decalage_EpaisseurDeck_P2;

    [Space(10)]

    // NEUTRE
    [Header("<b><color=#FFDC99>ACTIF</color></b>")]
    public Transform zone_Deck_Actif;
    public GameObject deck_Actif;
    public List<Carte> liste_Deck_Actif;
    [Space(10)]
    public Transform zone_Defaussse_Actif;
    public GameObject defausse_Actif;
    public List<Carte> liste_Defausse_Actif;
    [Space(10)]
    public Transform zone_Main_Actif;
    public GameObject main_Actif;
    public List<Carte> liste_Main_Actif;
    [Space(10)]
    public Vector3 decalage_EpaisseurDeck_Actif;

    void Start()
    {
        
    }
    // ============================================================
    // INITIALISATION
    // ============================================================
    public void InitialiserDeckDesJoueurs()
    {
        CreerDeckDeDepart(Joueur.P1,liste_Deck_P1);
        CreerDeckDeDepart(Joueur.P2, liste_Deck_P2);
        MelangerDeck(Joueur.P1);
        MelangerDeck(Joueur.P2);
    }

    // ============================================================
    // CREATION DU DECK
    // ============================================================

    public void CreerDeckDeDepart( Joueur joueurCible, List<Carte> deckCible)
    {
        Debug.Log($"<b><color=#80D8FF> -> Création  du deck de départ de {joueurCible} :  {deckCible.Count} cartes au total</color></b>");

        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Paysan, GM.nbCarte_AuDepart_Paysan);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Bucheron, GM.nbCarte_AuDepart_Bucheron);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Mineur, GM.nbCarte_AuDepart_Mineur);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Marchand, GM.nbCarte_AuDepart_Marchand);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Tavernier, GM.nbCarte_AuDepart_Tavernier);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Charpentier, GM.nbCarte_AuDepart_Charpentier);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Batisseur, GM.nbCarte_AuDepart_Batisseur);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Architecte, GM.nbCarte_AuDepart_Architecte);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Forgeron, GM.nbCarte_AuDepart_Forgeron);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Aumonier, GM.nbCarte_AuDepart_Aumonier);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Philosophe, GM.nbCarte_AuDepart_Philosophe);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Mendiant, GM.nbCarte_AuDepart_Mendiant);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Artilleur, GM.nbCarte_AuDepart_Artilleur);
        AjouterCartesAuDeck(joueurCible, deckCible, NomCarte.Bourreau, GM.nbCarte_AuDepart_Bourreau);
    }

    public void AjouterCartesAuDeck(Joueur joueurCible, List<Carte> deckCible, NomCarte nomCarte, int quantite)
    {
        Carte_Data carteData = cartes_DataBase.TrouverCarte(nomCarte);

        if (carteData == null)
        {
            Debug.LogError($"<b><color=#FF0000>!!! Impossible d'ajouter les cartes : {nomCarte} introuvable dans la Database !</color></b>");
            return; 
        }

        for (int i = 0; i < quantite; i++)
        {
            Carte carte = new Carte(carteData);
            deckCible.Add(carte);
            CreerVisuelDeLaCarte(carte);
        }
    }

    private void CreerVisuelDeLaCarte(Carte carte)
    {
        Debug.Log($"<b><color=#80D8FF> -> Visuel créé : {carte.data.nom} </color></b>");

        GameObject nouvelleCarte = Instantiate(carte.data.carteGameObject, zone_Deck_Actif.position, Quaternion.identity, deck_Actif.transform);

        carte.gameObject = nouvelleCarte;

        CarteInteraction carteInteraction = nouvelleCarte.AddComponent<CarteInteraction>();
        carteInteraction.carte = carte;
        carteInteraction.mainManager = MM;

        SpriteRenderer sr = nouvelleCarte.GetComponent<SpriteRenderer>();
        sr.sprite = carte_Verso;
    }

    // ============================================================
    // MELANGE DU DECK
    // ============================================================

    public void MelangerDeck(Joueur joueurCible)
    {
        Debug.Log($"<b><color=#80D8FF> -> Mélange du deck de {joueurCible} </color></b>");

        List<Carte> liste_Deck_Cible;
        Vector3 decalage_EpaisseurDeck_Cible;
        Transform zone_Deck_Cible;

        if (joueurCible == Joueur.P1)
        {
            liste_Deck_Cible = liste_Deck_P1;
            decalage_EpaisseurDeck_Cible = decalage_EpaisseurDeck_P1;
            zone_Deck_Cible = zone_Deck_P1;
        }
        else
        {
            liste_Deck_Cible = liste_Deck_P2;
            decalage_EpaisseurDeck_Cible = decalage_EpaisseurDeck_P2;
            zone_Deck_Cible = zone_Deck_P2;
        }

        for (int i = 0; i < liste_Deck_Cible.Count; i++)
        {
            Carte temp = liste_Deck_Cible[i];
            int randomIndex = UnityEngine.Random.Range(i, liste_Deck_Cible.Count);
            liste_Deck_Cible[i] = liste_Deck_Cible[randomIndex];
            liste_Deck_Cible[randomIndex] = temp;

            liste_Deck_Cible[i].gameObject.transform.position = zone_Deck_Cible.position + (decalage_EpaisseurDeck_Actif * i);
            SpriteRenderer sr = liste_Deck_Cible[i].gameObject.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Deck";
            sr.sortingOrder = i * 10;
        }
    }

    // ============================================================
    // RECYCLAGE DE DEFAUSSE
    // ============================================================

    public IEnumerator DemnderRecyclageDefausse_Coroutine()
    {
        yield return StartCoroutine(RecyclageDefausse_Coroutine());
    }

    private IEnumerator RecyclageDefausse_Coroutine()
    {
        int nombreCartes = liste_Defausse_Actif.Count;
        nombreCartesTerminees = 0;

        for (int i = 0; i < nombreCartes; i++)
        {
            Carte carte = liste_Defausse_Actif[0];

            liste_Defausse_Actif.RemoveAt(0);
            liste_Deck_Actif.Add(carte);

            StartCoroutine(EnvoyerCarteVersCercleMelange_Coroutine(carte));

            yield return new WaitForSeconds(tempsEntreEjectionCartes);
        }

        while (nombreCartesTerminees < nombreCartes)
        {
            yield return null;
        }

        Debug.Log($"<b><color=#80D8FF>/// Toutes les cartes de la defausse de {GM.joueurActif} ont été renvoyées vers le deck !</color></b>");

        MelangerDeck(GM.joueurActif);
    }

    // ============================================================
    // ANIMATION RECYCLAGE
    // ============================================================

    private IEnumerator EnvoyerCarteVersCercleMelange_Coroutine(Carte carte)
    {
        Transform objet = carte.gameObject.transform;

        centre_Tourbillon.position = zone_Deck_Actif.position;

        Vector3 centre = centre_Tourbillon.position;

        Vector3 positionDepart = objet.position;

        Vector3 positionArrivee = centre + Vector3.up * rayonCercleMelange;

        Vector3 directionCourbe = Vector3.left;

        if (GM.joueurActif == Joueur.P2)
        {
            directionCourbe = Vector3.right;
        }

        Vector3 positionControle = centre + directionCourbe * rayonCercleMelange * 1f;

        float temps = 0f;

        Vector3 scaleOriginal = objet.localScale;

        float angleDepart = objet.eulerAngles.z;
        float angleFinal;

        if (GM.joueurActif == Joueur.P1)
        {
            angleFinal = angleDepart - 90f;
        }
        else
        {
            angleFinal = angleDepart + 90f;
        }

        while (temps < tempsDeplacementVersCercle)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementVersCercle;

            Vector3 point1 = Vector3.Lerp(positionDepart, positionControle, progression);
            Vector3 point2 = Vector3.Lerp(positionControle, positionArrivee, progression);

            objet.position = Vector3.Lerp(point1, point2, progression);

            objet.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(angleDepart, angleFinal, progression));

            objet.localScale = new Vector3(Mathf.Lerp(scaleOriginal.x, 0f, progression), scaleOriginal.y, scaleOriginal.z);

            yield return null;
        }

        objet.position = positionArrivee;
        objet.rotation = Quaternion.Euler(0f, 0f, angleFinal);
        objet.localScale = new Vector3(0f, scaleOriginal.y, scaleOriginal.z);

        SpriteRenderer spriteRenderer = objet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = carte_Verso;

        yield return StartCoroutine(FaireTournerCarte_Coroutine(carte, scaleOriginal));
    }

    private IEnumerator FaireTournerCarte_Coroutine(Carte carte, Vector3 scaleOriginal)
    {
        Transform objet = carte.gameObject.transform;

        Vector3 centre = centre_Tourbillon.position;

        float angle = 90f;
        float vitesseActuelle = vitesseTourbillon_Min;
        float rayonActuel = rayonCercleMelange;

        float tempsRetour = 0f;

        while (rayonActuel > 0f)
        {
            tempsRetour += Time.deltaTime;

            float progressionRetour = Mathf.Clamp01(tempsRetour / tempsRetournementCarte);

            float scaleX = Mathf.Lerp(0f, scaleOriginal.x, progressionRetour);

            objet.localScale = new Vector3(scaleX, scaleOriginal.y, scaleOriginal.z);

            vitesseActuelle = Mathf.MoveTowards(vitesseActuelle, vitesseTourbillon_Max, accelerationTourbillon * Time.deltaTime);

            rayonActuel = Mathf.MoveTowards(rayonActuel, 0f, vitesseReductionRayon * Time.deltaTime);

            if (GM.joueurActif == Joueur.P1)
            {
                angle -= vitesseActuelle * Time.deltaTime;
            }
            else
            {
                angle += vitesseActuelle * Time.deltaTime;
            }

            float angleRadians = angle * Mathf.Deg2Rad;

            float x = centre.x + Mathf.Cos(angleRadians) * rayonActuel;
            float y = centre.y + Mathf.Sin(angleRadians) * rayonActuel;

            objet.position = new Vector3(x, y, objet.position.z);

            objet.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        objet.position = centre;

        objet.rotation = Quaternion.Euler(0f, 0f, 0f);

        objet.localScale = scaleOriginal;

        nombreCartesTerminees++;
    }
}
