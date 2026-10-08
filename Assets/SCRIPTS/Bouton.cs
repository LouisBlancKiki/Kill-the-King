using UnityEngine;
using UnityEngine.Events;

public class Bouton : MonoBehaviour
{

    [SerializeField] private UnityEvent on_Clic;
    Vector3 taille_boutonDefaut = new Vector3(1, 1, 1); 
    Vector3 taille_boutonPresse = new Vector3(0.95f, 0.95f, 0);

    private void OnMouseDown()
    {
        Debug.Log("<b><color=#FF80FF>[ CLIC ] : " + gameObject.name + "</color></b>");
       
        transform.localScale = taille_boutonPresse;
    }
    private void OnMouseUp()
    {
        on_Clic?.Invoke();
        transform.localScale = taille_boutonDefaut;
    }
}
