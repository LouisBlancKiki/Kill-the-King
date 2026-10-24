using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;

public class Bouton_Action : MonoBehaviour
{
    [Header("<b><color=#FFDC99>MANAGERS</color></b>")]
    public PanneauActionManager PAM;

    [Space(10)]

    [Header("<b><color=#FFDC99>SPRITES</color></b>")]
    public Sprite sprite_Desselectionne;
    public Sprite sprite_Selectionne;
    [Space(10)]
    public Sprite sprite_Info_Action;
    [Space(10)]
    public SpriteRenderer sr_Image;
    [Space(10)]
    public Action action;

    [Space(10)]

    [Header("<b><color=#FFDC99>ETAT DE SELECTION</color></b>")]
    public bool estSelectionne;
   

    public Coroutine animationPop_Coroutine;

    private void Awake()
    {
        sr_Image = transform.Find("image").GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (PAM != null)
        {
            PAM.SelectionnerBoutonAction(this);
        }
    }

    private void OnDestroy()
    {
        if (animationPop_Coroutine != null)
        {
            StopCoroutine(animationPop_Coroutine);
            animationPop_Coroutine = null;
        }
    }

    public void Selectionner()
    {
        estSelectionne = true;
        sr_Image.sprite = sprite_Selectionne;
        transform.localScale = Vector3.one * PAM.taille_bouton_Action_Selectionne;

    }

    public void Deselectionner()
    {
        estSelectionne = false;
        sr_Image.sprite = sprite_Desselectionne;
        transform.localScale = Vector3.one;
    }

    public void DemarrerAnimationPop()
    {
        animationPop_Coroutine = StartCoroutine(AnimationPop_Coroutine());
    }

    public IEnumerator AnimationPop_Coroutine()
    {
        if (PAM == null) yield break;

        float temps = 0f;

        while (temps < PAM.duree_Pop_Bouton_Action)
        {
            temps += Time.deltaTime;
            float progression = Mathf.Clamp01(temps / PAM.duree_Pop_Bouton_Action);
            float taille = Mathf.Lerp(0f, PAM.taille_Max_Pop_Bouton_Action, progression);

            transform.localScale = new Vector3(taille, taille, 1f);

            yield return null;
        }

        temps = 0f;

        while (temps < PAM.duree_Pop_Bouton_Action * 0.9f)
        {
            temps += Time.deltaTime;
            float progression = Mathf.Clamp01(temps / (PAM.duree_Pop_Bouton_Action * 0.9f));
            float taille = Mathf.Lerp(PAM.taille_Max_Pop_Bouton_Action, 1f, progression);

            transform.localScale = new Vector3(taille, taille, 1f);

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
