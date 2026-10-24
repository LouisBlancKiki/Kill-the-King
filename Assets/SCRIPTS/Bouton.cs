using UnityEngine;
using UnityEngine.Events;

public class Bouton : MonoBehaviour
{

    [SerializeField] private UnityEvent on_ClicGauche;
    [SerializeField] private UnityEvent on_ClicDroit;
    Vector3 taille_boutonDefaut = new Vector3(1, 1, 1); 
    Vector3 taille_boutonPresse = new Vector3(0.95f, 0.95f, 0);

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("<b><color=#FF80FF>[ CLIC GAUCHE ] : " + gameObject.name + "</color></b>");
            
            transform.localScale = taille_boutonPresse;
        }
        if (Input.GetMouseButtonUp(0))
        {
            on_ClicGauche?.Invoke();
            transform.localScale = taille_boutonDefaut;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("<b><color=#FF80FF>[ CLIC DROIT ] : " + gameObject.name + "</color></b>");

            transform.localScale = taille_boutonPresse;
        }
        if (Input.GetMouseButtonUp(1))
        {
            on_ClicDroit?.Invoke();
            transform.localScale = taille_boutonDefaut;
        }
    }
    
}
