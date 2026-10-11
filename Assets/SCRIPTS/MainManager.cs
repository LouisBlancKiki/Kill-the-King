using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class MainManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;
    public DeckManager DM;

    [Space(10)]

    [Header("<b><color=#FFDC99>TEMPS</color></b>")]
    public float tempsEntreChaquePioche;
    public float tempsEntreChaqueDefausse;
    public float tempsDeplacementDesCartes;

    [Space(10)]

    [Header("<b><color=#FFDC99>PRESENTATION DES CARTES</color></b>")]
    public float espacementDesCartes;
    public float hauteurCourbeMain;
    public float angleMaxCourbe;

    [Space(10)]

    [Header("<b><color=#FFDC99>SELECTION CARTE</color></b>")]
    public float hauteurSelection;
    public float facteurTailleSelection;
    public float tempsAnimationSelection;
    [Space(10)]
    private Carte carteSelectionnee;
    

    // ============================================================
    // DEBUT DE TOUR
    // ============================================================

    public void piocheDeDebutDeTour()
    {
        StartCoroutine(PiocherCartesDeDebutDeTour_Coroutine());
    }

    private IEnumerator PiocherCartesDeDebutDeTour_Coroutine()
    {
        int nombreDeCartes = GM.nbCartePiocheesAuDebutDuTour;

        for (int i = 0; i < nombreDeCartes; i++)
        {
            yield return StartCoroutine(PiocherCarte_Coroutine());

            yield return new WaitForSeconds(tempsEntreChaquePioche);
        }
    }

    // ============================================================
    // PIOCHE D'UNE CARTE
    // ============================================================

    private IEnumerator PiocherCarte_Coroutine()
    {
        Carte cartePiochee;
        Vector3 positionDepart;
        
        if (DM.liste_Deck_Actif.Count == 0)
        {
            Debug.Log($"<b><color=#80D8FF> -> Recyclage de la défausse de {GM.joueurActif} !</color></b>");

            yield return StartCoroutine(DM.DemnderRecyclageDefausse_Coroutine());

            if (DM.liste_Deck_Actif.Count == 0)
            {
                Debug.LogWarning($"<b><color=#FFFF00>!!! Impossible de piocher : le deck et la défausse de {GM.joueurActif} sont vides !</color></b>");
                yield break;
            }
        }

        cartePiochee = DM.liste_Deck_Actif[DM.liste_Deck_Actif.Count - 1];
        DM.liste_Deck_Actif.RemoveAt(DM.liste_Deck_Actif.Count - 1);
        DM.liste_Main_Actif.Add(cartePiochee);
        cartePiochee.gameObject.transform.SetParent(DM.main_Actif.transform);
        positionDepart = DM.zone_Deck_Actif.position;

        Debug.Log($"<b><color=#80D8FF> -> {GM.joueurActif} pioche une carte ! </color></b>");

        SpriteRenderer sr = cartePiochee.gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = cartePiochee.data.sprite_Carte;
        sr.sortingLayerName = "Main";
        sr.sortingOrder = (DM.liste_Main_Actif.Count - 1) * 10;
        cartePiochee.gameObject.transform.position = positionDepart;

        Dictionary<Carte, Carte_Presentation> dico_presentationFinale = CalculerPositionsDesCartes(DM.liste_Main_Actif);

        foreach (Carte carte in DM.liste_Main_Actif)
        {
            Vector3 positionActuelle = carte.gameObject.transform.position;
            Carte_Presentation presentationFinale = dico_presentationFinale[carte];
            Vector3 positionFinale = presentationFinale.position;

            StartCoroutine(DeplacerCarte_Coroutine(carte, positionActuelle, positionFinale, presentationFinale.rotation));
        }

        yield return new WaitForSeconds(tempsDeplacementDesCartes);
    }

    // ============================================================
    // CALCUL DES POSITIONS
    // ============================================================

    private Dictionary<Carte, Carte_Presentation> CalculerPositionsDesCartes(List<Carte> main)
    {
        Dictionary<Carte, Carte_Presentation> dico_presentationsFinales = new Dictionary<Carte, Carte_Presentation>();

        int nombreDeCartes = main.Count;

        for (int i = 0; i < nombreDeCartes; i++)
        {
            Carte carte = main[i];

            float positionX = (i - (nombreDeCartes - 1) / 2f) * espacementDesCartes; 
            
            float progression = 0.5f;

            if (nombreDeCartes > 1)
            {
                progression = (float)i / (nombreDeCartes - 1);
            }

            float hauteur = Mathf.Sin(progression * Mathf.PI) * hauteurCourbeMain;

            Vector3 positionLocale = new Vector3(positionX, hauteur, 0f);

            Vector3 positionMonde = DM.zone_Main_Actif.TransformPoint(positionLocale);

            float angle = Mathf.Lerp(angleMaxCourbe, -angleMaxCourbe, progression);

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            Carte_Presentation presentation = new Carte_Presentation(positionMonde, rotation);

            dico_presentationsFinales.Add(carte, presentation);
        }

        return dico_presentationsFinales;
    }

    // ============================================================
    // DEPLACEMENT CARTE
    // ============================================================

    private IEnumerator DeplacerCarte_Coroutine(Carte carte, Vector3 positionDepart, Vector3 positionArrivee, Quaternion rotationArrivee)
    {
        float temps = 0f;

        Quaternion rotationDepart = carte.gameObject.transform.rotation;

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carte.gameObject.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carte.gameObject.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        Vector3 positionFinale = positionArrivee;

        if (carte == carteSelectionnee)
        {
            positionFinale += Vector3.up * hauteurSelection;
        }

        carte.gameObject.transform.position = positionFinale;

        carte.gameObject.transform.rotation = rotationArrivee;
    }

    // ============================================================
    // DEFAUSSER MAIN DU JOUEUR
    // ============================================================



    public IEnumerator DefausserMainDuJoueur_Coroutine()
    {
        if (carteSelectionnee != null)
        {
            yield return StartCoroutine(DeselectionnerCarte_Coroutine(carteSelectionnee));
            carteSelectionnee = null;
        }

        yield return StartCoroutine(DefausserMain_Coroutine());
    }

    private IEnumerator DefausserMain_Coroutine()
    {
        while (DM.liste_Main_Actif.Count > 0)
        {
            Carte carte = DM.liste_Main_Actif[0];
            
            Vector3 positionDepart = carte.gameObject.transform.position;

            carte.gameObject.GetComponent<SpriteRenderer>().sortingOrder = DM.liste_Defausse_Actif.Count * 10;
            DM.liste_Main_Actif.RemoveAt(0);
            DM.liste_Defausse_Actif.Add(carte);
            carte.gameObject.transform.SetParent(DM.defausse_Actif.transform);

            SpriteRenderer sr = carte.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = "Defausse";
            sr.sortingOrder = (DM.liste_Defausse_Actif.Count - 1) * 10;

            yield return StartCoroutine(DeplacerCarteVersDefausse_Coroutine(carte, positionDepart, DM.zone_Defaussse_Actif.position));

            yield return new WaitForSeconds(tempsEntreChaqueDefausse);
        }
    }

    // ============================================================
    // DEPLACER CARTE VERS LA DEFAUSSE
    // ============================================================

    private IEnumerator DeplacerCarteVersDefausse_Coroutine(Carte carte, Vector3 positionDepart, Vector3 positionArrivee)
    {
        float temps = 0f;

        Quaternion rotationDepart = carte.gameObject.transform.rotation;
        Quaternion rotationArrivee = Quaternion.identity;

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carte.gameObject.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carte.gameObject.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        carte.gameObject.transform.position = positionArrivee;
        carte.gameObject.transform.rotation = rotationArrivee;
    }

    // ============================================================
    //SELECTION DES CARTE 
    // ============================================================

    public void SelectionnerCarte(Carte carte)
    {
        // Cliquer sur la carte déjà sélectionnée
        if (carteSelectionnee == carte)
        {
            StartCoroutine(DeselectionnerCarte_Coroutine(carte));
            carteSelectionnee = null;
            return;
        }

        // Désélectionner l'ancienne
        if (carteSelectionnee != null)
        {
            StartCoroutine(DeselectionnerCarte_Coroutine(carteSelectionnee));
        }

        // Sélectionner la nouvelle
        carteSelectionnee = carte;

        StartCoroutine(AnimerSelectionCarte_Coroutine(carte, true));

        Debug.Log($"<b><color=#FFFF00> -> Carte sélectionnée : {carte.data.nom}</color></b>");
    }

    private IEnumerator DeselectionnerCarte_Coroutine(Carte carte)
    {
        yield return StartCoroutine(AnimerSelectionCarte_Coroutine(carte, false));
    }

    private IEnumerator AnimerSelectionCarte_Coroutine(Carte carte, bool selection)
    {
        Transform transformCarte = carte.gameObject.transform;

        Vector3 positionDepart = transformCarte.position;
        Vector3 echelleDepart = transformCarte.localScale;

        Vector3 positionArrivee = positionDepart;
        Vector3 echelleArrivee = echelleDepart;

        if (selection)
        {
            transformCarte.GetComponent<SpriteRenderer>().sortingOrder = 1000;

            positionArrivee += Vector3.up * hauteurSelection;
            echelleArrivee *= facteurTailleSelection;
        }
        else
        {
            transformCarte.GetComponent<SpriteRenderer>().sortingOrder = DM.liste_Main_Actif.IndexOf(carte) * 10;
            positionArrivee -= Vector3.up * hauteurSelection;
            echelleArrivee /= facteurTailleSelection;
        }

        float temps = 0f;

        while (temps < tempsAnimationSelection)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsAnimationSelection;

            transformCarte.position = Vector3.Lerp(
                positionDepart,
                positionArrivee,
                progression
            );

            transformCarte.localScale = Vector3.Lerp(
                echelleDepart,
                echelleArrivee,
                progression
            );

            yield return null;
        }

        transformCarte.position = positionArrivee;
        transformCarte.localScale = echelleArrivee;

    }
}