using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class MainManager : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public GameManager GM;
    public DeckManager DM;

    [Header("<b><color=#FFDC99>ZONE MAIN</color></b>")]
    public Transform zone_Main;

    [Header("<b><color=#FFDC99>MAINS</color></b>")]
    public List<Carte> Main_P1 = new List<Carte>();
    public List<Carte> Main_P2 = new List<Carte>();

    [Header("<b><color=#FFDC99>TEMPS</color></b>")]
    public float tempsEntreChaquePioche;
    public float tempsEntreChaqueDefausse;
    public float tempsDeplacementDesCartes;

    [Header("<b><color=#FFDC99>PRESENTATION DES CARTES</color></b>")]
    public float espacementDesCartes;
    public float hauteurCourbeMain;
    public float angleMaxCourbe;


    // ============================================================
    // CREATION VISUELLE
    // ============================================================

    private void CreerVisuelDeLaCarte(Carte carte, Joueur joueurCible)
    {
        GameObject nouvelleCarte = Instantiate(carte.data.carteGameObject, zone_Main);

        carte.objetCarte = nouvelleCarte;

        Debug.Log($"<b><color=#80D8FF> -> Carte créée : {carte.data.nom} </color></b>");


        int sortingOrderCarte;

        if (joueurCible == Joueur.P1)
        {
            nouvelleCarte.transform.position = DM.zone_Deck_P1.position;
            sortingOrderCarte = Main_P1.Count * -10;
        }
        else
        {
            nouvelleCarte.transform.position = DM.zone_Deck_P2.position;
            sortingOrderCarte = Main_P2.Count * -10;
        }

        nouvelleCarte.GetComponent<SpriteRenderer>().sprite = carte.data.sprite_Carte;
        nouvelleCarte.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderCarte;

        nouvelleCarte.transform.localRotation = Quaternion.identity;
    }


    // ============================================================
    // DEBUT DE TOUR
    // ============================================================

    public void piocheDeDebutDeTour()
    {
        StartCoroutine(PiocherCartesDeDebutDeTour());
    }

    private IEnumerator PiocherCartesDeDebutDeTour()
    {
        int nombreDeCartes = GM.nbCartePiocheesAuDebutDuTour;

        for (int i = 0; i < nombreDeCartes; i++)
        {
            yield return StartCoroutine(PiocherCarte(GM.joueurActif));

            yield return new WaitForSeconds(tempsEntreChaquePioche);
        }
    }


    // ============================================================
    // PIOCHE D'UNE CARTE
    // ============================================================

    private IEnumerator PiocherCarte(Joueur joueurCible)
    {
        Carte cartePiochee;
        Vector3 positionDepart;

        if (joueurCible == Joueur.P1)
        {
            if (DM.deck_P1.Count == 0)
            {
                Debug.LogWarning("<b><color=#FFFF00>!!! Deck de P1 vide, recyclage de la défausse !</color></b>");

                yield return StartCoroutine(DM.RecyclageDefausse(joueurCible));

                if (DM.deck_P1.Count == 0)
                {
                    Debug.LogWarning("<b><color=#FFFF00>!!! Impossible de piocher : le deck et la défausse de P1 sont vides !</color></b>");
                    yield break;
                }
            }

            cartePiochee = DM.deck_P1[DM.deck_P1.Count - 1];
            DM.deck_P1.RemoveAt(DM.deck_P1.Count - 1);
            Main_P1.Add(cartePiochee);

            positionDepart = DM.zone_Deck_P1.position;
        }
        else
        {
            if (DM.deck_P2.Count == 0)
            {
                Debug.LogWarning("<b><color=#FFFF00>!!! Deck de P2 vide, recyclage de la défausse !</color></b>");

                yield return StartCoroutine(DM.RecyclageDefausse(joueurCible));

                if (DM.deck_P2.Count == 0)
                {
                    Debug.LogWarning("<b><color=#FFFF00>!!! Impossible de piocher : le deck et la défausse de P2 sont vides !</color></b>");
                    yield break;
                }
            }

            cartePiochee = DM.deck_P2[DM.deck_P2.Count - 1];
            DM.deck_P2.RemoveAt(DM.deck_P2.Count - 1);
            Main_P2.Add(cartePiochee);

            positionDepart = DM.zone_Deck_P2.position;
        }

        Debug.Log($"<b><color=#80D8FF> -> {joueurCible} pioche une carte ! </color></b>");

        SpriteRenderer spriteRenderer = cartePiochee.objetCarte.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cartePiochee.data.sprite_Carte;

        cartePiochee.objetCarte.transform.position = positionDepart;

        List<Carte> mainActive;

        if (joueurCible == Joueur.P1)
        {
            mainActive = Main_P1;
        }
        else
        {
            mainActive = Main_P2;
        }

        Dictionary<Carte, Carte_Presentation> dico_presentationFinale = CalculerPositionsDesCartes(mainActive);

        foreach (Carte carte in mainActive)
        {
            Vector3 positionActuelle = carte.objetCarte.transform.position;
            Carte_Presentation presentationFinale = dico_presentationFinale[carte];
            Vector3 positionFinale = presentationFinale.position;

            StartCoroutine(DeplacerCarte(carte, positionActuelle, positionFinale, presentationFinale.rotation));
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

            Vector3 positionMonde = zone_Main.TransformPoint(positionLocale);

            float angle = Mathf.Lerp(angleMaxCourbe, -angleMaxCourbe, progression);

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            Carte_Presentation presentation = new Carte_Presentation(positionMonde, rotation);

            dico_presentationsFinales.Add(carte, presentation);
        }

        return dico_presentationsFinales;
    }


    // ============================================================
    // DEPLACEMENT FLUIDE
    // ============================================================

    private IEnumerator DeplacerCarte(Carte carte, Vector3 positionDepart, Vector3 positionArrivee, Quaternion rotationArrivee)
    {
        float temps = 0f;

        Quaternion rotationDepart = carte.objetCarte.transform.rotation;

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carte.objetCarte.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carte.objetCarte.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        carte.objetCarte.transform.position = positionArrivee;
        carte.objetCarte.transform.rotation = rotationArrivee;
    }

    // ============================================================
    // DEFAUSSER MAIN DU JOUEUR
    // ============================================================

    public void DefausserMainDuJoueur()
    {
        StartCoroutine(DefausserMain(GM.joueurActif));
    }

    private IEnumerator DefausserMain(Joueur joueurCible)
    {
        List<Carte> mainActive;

        if (joueurCible == Joueur.P1)
        {
            mainActive = Main_P1;
        }
        else
        {
            mainActive = Main_P2;
        }

        while (mainActive.Count > 0)
        {
            Carte carte = mainActive[0];
            
            List<Carte> defausseActive;
            Transform zoneDefausse;

            if (joueurCible == Joueur.P1)
            {
                defausseActive = DM.defausse_P1;
                zoneDefausse = DM.zone_Defaussse_P1;
            }
            else
            {
                defausseActive = DM.defausse_P2;
                zoneDefausse = DM.zone_Defaussse_P2;
            }

            Vector3 positionDepart = carte.objetCarte.transform.position;

            carte.objetCarte.GetComponent<SpriteRenderer>().sortingOrder = defausseActive.Count * 10;
            defausseActive.Add(carte);
            mainActive.RemoveAt(0);

            yield return StartCoroutine(DeplacerCarteVersDefausse(carte, positionDepart, zoneDefausse.position));

            yield return new WaitForSeconds(tempsEntreChaqueDefausse);
        }
    }

    // ============================================================
    // DEPLACER CARTE VERS LA DEFAUSSE
    // ============================================================

    private IEnumerator DeplacerCarteVersDefausse(Carte carte, Vector3 positionDepart, Vector3 positionArrivee)
    {
        float temps = 0f;

        Quaternion rotationDepart = carte.objetCarte.transform.rotation;
        Quaternion rotationArrivee = Quaternion.identity;

        while (temps < tempsDeplacementDesCartes)
        {
            temps += Time.deltaTime;

            float progression = temps / tempsDeplacementDesCartes;

            carte.objetCarte.transform.position = Vector3.Lerp(positionDepart, positionArrivee, progression);

            carte.objetCarte.transform.rotation = Quaternion.Lerp(rotationDepart, rotationArrivee, progression);

            yield return null;
        }

        carte.objetCarte.transform.position = positionArrivee;
        carte.objetCarte.transform.rotation = rotationArrivee;
    }
}