using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollections : MonoBehaviour
{
    private int bananas = 0;
    [SerializeField] private Text pointsText;
    [SerializeField] private AudioSource itemCollectSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Banana")){
            itemCollectSound.Play();
            Destroy(collision.gameObject);
            bananas++;
            pointsText.text = "Points : " + bananas;
        }
    }
}
