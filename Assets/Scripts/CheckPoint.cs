
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool isChecked = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isChecked)
        {
            PlayerLife player = collision.GetComponent<PlayerLife>();
            if (player != null)
            {
                player.UpdateCheckpoint(transform.position);
            }
            AudioManager.Instance.PlaySFX("Checkpoint");
            isChecked = true;

        };

    }

}