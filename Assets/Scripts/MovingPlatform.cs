using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] string nameObj;

   private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == nameObj)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == nameObj)
        {
            collision.gameObject.transform.SetParent(null);
        }
    } 
}
