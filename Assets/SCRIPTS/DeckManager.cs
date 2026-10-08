using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class DeckManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;

    [Space(10)]

    [Header("<b><color=#FFDC99>CARTES</color></b>")]
    public Cartes_DataBase cartes_DataBase;
    public Sprite carte_Verso;
    public Vector3 decalage_EpaisseurDeck;

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
    public List<Carte> deck_P1 = new List<Carte>();
    public Transform zone_Defaussse_P1;
    public List<Carte> defausse_P1 = new List<Carte>();

    [Space(10)]

    // P2
    [Header("<b><color=#FFDC99>P2</color></b>")]
    public Transform zone_Deck_P2;
    public List<Carte> deck_P2 = new List<Carte>();
    public Transform zone_Defaussse_P2;
    public List<Carte> defausse_P2 = new List<Carte>();


    void Start()
    {
        
    }

    public void InitialiserDeckDesJoueurs()
    {
        CreerDeckDeDepart(Joueur.P1,deck_P1);
        CreerDeckDeDepart(Joueur.P2, deck_P2);
    }

    public void CreerDeckDeDepart( Joueur joueurCible, List<Carte> deckCible)
    {
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

        MelangerDeck(joueurCible);

        Debug.Log($"<b><color=#80D8FF> -> Création et mélange du deck de départ de {joueurCible} :  {deckCible.Count} cartes au total</color></b>");
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
            CreerVisuelDeLaCarte(carte, joueurCible);
        }
    }

    private void CreerVisuelDeLaCarte(Carte carte, Joueur joueurCible)
    {
        GameObject nouvelleCarte = Instantiate(carte.data.carteGameObject);

        carte.objetCarte = nouvelleCarte;

        Transform zoneDeck;

        if (joueurCible == Joueur.P1)
        {
            zoneDeck = zone_Deck_P1;
        }
        else
        {
            zoneDeck = zone_Deck_P2;
        }

        nouvelleCarte.transform.position = zoneDeck.position;
        nouvelleCarte.transform.rotation = Quaternion.identity;

        SpriteRenderer sr = nouvelleCarte.GetComponent<SpriteRenderer>();
        sr.sprite = carte_Verso;
        

        Debug.Log($"<b><color=#80D8FF> -> Visuel créé : {carte.data.nom} </color></b>");
    }

    public void MelangerDeck(Joueur joueurCible)
    {
        Transform zoneDeck;
        List<Carte> deck;

        if (joueurCible == Joueur.P1)
        {
            zoneDeck = zone_Deck_P1;
            deck = deck_P1;
        }
        else
        {
            zoneDeck = zone_Deck_P2;
            deck = deck_P2;
        }

        for (int i = 0; i < deck.Count; i++)
        {
            Carte temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;

            deck[i].objetCarte.transform.position = zoneDeck.position + (decalage_EpaisseurDeck * i);
            deck[i].objetCarte.GetComponent<SpriteRenderer>().sortingOrder = i * 10;
        }
    }

    public IEnumerator RecyclageDefausse(Joueur joueurCible)
    {
        yield return StartCoroutine(RecyclageDefausseCoroutine(joueurCible));
    }

    private IEnumerator RecyclageDefausseCoroutine(Joueur joueurCible)
    {
        List<Carte> defausseActive;
        List<Carte> deckActif;
        //Transform zoneDefausse;

        if (joueurCible == Joueur.P1)
        {
            defausseActive = defausse_P1;
            deckActif = deck_P1;
            //zoneDefausse = zone_Defaussse_P1;
        }
        else
        {
            defausseActive = defausse_P2;
            deckActif = deck_P2;
            //zoneDefausse = zone_Defaussse_P2;
        }

        int nombreCartes = defausseActive.Count;
        nombreCartesTerminees = 0;

        for (int i = 0; i < nombreCartes; i++)
        {
            Carte carte = defausseActive[0];

            defausseActive.RemoveAt(0);
            deckActif.Add(carte);

            StartCoroutine(EnvoyerCarteVersCercleMelange(carte, joueurCible));

            yield return new WaitForSeconds(tempsEntreEjectionCartes);
        }

        while (nombreCartesTerminees < nombreCartes)
        {
            yield return null;
        }

        Debug.Log("<b><color=#FF0000>/// Toutes les cartes sont retournées dans le deck !</color></b>");

        MelangerDeck(joueurCible);
    }

    private IEnumerator EnvoyerCarteVersCercleMelange(Carte carte, Joueur joueurCible)
    {
        if (joueurCible == Joueur.P1)
        {
            centre_Tourbillon.position = zone_Deck_P1.position;
        }
        if (joueurCible == Joueur.P2)
        {
            centre_Tourbillon.position = zone_Deck_P2.position;
        }

        Transform objet = carte.objetCarte.transform;

        Vector3 centre = centre_Tourbillon.position;

        Vector3 positionDepart = objet.position;

        Vector3 positionArrivee = centre + Vector3.up * rayonCercleMelange;

        Vector3 directionCourbe = Vector3.left;

        if (joueurCible == Joueur.P2)
        {
            directionCourbe = Vector3.right;
        }

        Vector3 positionControle = centre + directionCourbe * rayonCercleMelange * 1f;

        float temps = 0f;

        Vector3 scaleOriginal = objet.localScale;

        float angleDepart = objet.eulerAngles.z;
        float angleFinal;

        if (joueurCible == Joueur.P1)
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

        yield return StartCoroutine(FaireTournerCarte(carte, joueurCible, scaleOriginal));
    }

    private IEnumerator FaireTournerCarte(Carte carte, Joueur joueurCible, Vector3 scaleOriginal)
    {
        Transform objet = carte.objetCarte.transform;

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

            if (joueurCible == Joueur.P1)
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
