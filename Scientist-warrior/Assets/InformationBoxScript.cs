using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationBoxScript : MonoBehaviour
{
    [SerializeField] private string message;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
//            DialogCanvas.Instance.PrintMessage(message,null,PlayerVisualScript.Instance.GetItemVisual("Refrence"));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogCanvas.Instance.ResetDialogPanel();
            Destroy(gameObject);
        }
    }
}
