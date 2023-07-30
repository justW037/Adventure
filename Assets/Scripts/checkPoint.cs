using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkPoints : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private string nameScene;
    [SerializeField] private AudioSource checkpointSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkpointSound.Play();
            ModeSelect();
        }
    }
    public void ModeSelect()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(nameScene);
    }
}
