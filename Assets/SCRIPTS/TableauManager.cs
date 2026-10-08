using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Elements;
using static UnityEngine.Rendering.DebugUI;

public class TableauManager : MonoBehaviour
{
    public GameManager GM;
    public GameObject tableau_DebutDeTour;
    TMP_Text txt_Nom_tableau_DebutDeTour;
    Vector3  pos_Tableau_DebutDeTour;

    void Start()
    {
        tableau_DebutDeTour.SetActive(false);
        txt_Nom_tableau_DebutDeTour = tableau_DebutDeTour.transform.Find("txt_NomDuJoueur").GetComponent<TMP_Text>();
        pos_Tableau_DebutDeTour = tableau_DebutDeTour.transform.position;
    }

    public void AfficherTableau_DebutDeTour()
    {
        if (GM.joueurActif == Joueur.P1)
        {
            txt_Nom_tableau_DebutDeTour.text = GM.nomJoueur_P1;
            //cadre_AQuiLeTour.GetComponent<CadreAQuiLeTour>().couleurJoueur_Cadre_AQuiLeTour.GetComponent<SpriteRenderer>().color = couleur_P1;
        }
        else if (GM.joueurActif == Joueur.P2)
        {
            txt_Nom_tableau_DebutDeTour.text = GM.nomJoueur_P2;
            //cadre_AQuiLeTour.GetComponent<CadreAQuiLeTour>().couleurJoueur_Cadre_AQuiLeTour.GetComponent<SpriteRenderer>().color = couleur_P2;
        }

        StartCoroutine(AfficherTableau_DebutDeTour_Coroutine(tableau_DebutDeTour));
    }

    public IEnumerator AfficherTableau_DebutDeTour_Coroutine(GameObject tableau)
    {
        Vector3 positionFinale = pos_Tableau_DebutDeTour;
        Vector3 positionDepart = positionFinale + Vector3.up * 5f;
        Vector3 positionDepassee = positionFinale - Vector3.up * 0.25f;

        SpriteRenderer[] sprites =
            tableau.GetComponentsInChildren<SpriteRenderer>(true);

        tableau.SetActive(true);

        foreach (SpriteRenderer sprite in sprites)
        {
            Color c = sprite.color;
            c.a = 1f;
            sprite.color = c;
        }

        // Position de départ : AU-DESSUS
        tableau.transform.position = positionDepart;

        // ==========================================
        // ARRIVÉE DEPUIS LE HAUT
        // ==========================================

        float temps = 0f;
        float duree = 0.25f;

        while (temps < duree)
        {
            temps += Time.deltaTime;

            float t = temps / duree;
            float mouvement = 1f - Mathf.Pow(1f - t, 3f);

            tableau.transform.position = Vector3.Lerp(
                positionDepart,
                positionDepassee,
                mouvement
            );

            yield return null;
        }

        // ==========================================
        // INERTIE : REMONTE LÉGÈREMENT
        // ==========================================

        temps = 0f;
        duree = 0.18f;

        while (temps < duree)
        {
            temps += Time.deltaTime;

            float t = temps / duree;
            float mouvement = 1f - Mathf.Pow(1f - t, 2f);

            tableau.transform.position = Vector3.Lerp(
                positionDepassee,
                positionFinale,
                mouvement
            );

            yield return null;
        }

        // On s'assure qu'il est exactement à sa position finale
        tableau.transform.position = positionFinale;
    }

    public void MasquerTableau_DebutDeTour()
    {
        StartCoroutine(MasquerTableau_DebutDeTour_Coroutine(tableau_DebutDeTour));
    }

    public IEnumerator MasquerTableau_DebutDeTour_Coroutine(GameObject tableau)
    {
        Vector3 positionDepart = tableau.transform.position;
        Vector3 positionFinale = pos_Tableau_DebutDeTour + Vector3.up * 5f;

        SpriteRenderer[] sprites =
            tableau.GetComponentsInChildren<SpriteRenderer>(true);

        float temps = 0f;
        float duree = 0.25f;

        while (temps < duree)
        {
            temps += Time.deltaTime;

            float t = temps / duree;

            float mouvement = t * t * t;

            tableau.transform.position = Vector3.Lerp(
                positionDepart,
                positionFinale,
                mouvement
            );

            foreach (SpriteRenderer sprite in sprites)
            {
                Color c = sprite.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                sprite.color = c;
            }

            yield return null;
        }

        tableau.transform.position = positionFinale;

        tableau.SetActive(false);

        foreach (SpriteRenderer sprite in sprites)
        {
            Color c = sprite.color;
            c.a = 1f;
            sprite.color = c;
        }
    }
}
