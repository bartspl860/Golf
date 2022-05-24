using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class CollisionEffect : MonoBehaviour
{
    [SerializeField]
    private string sound;
    [SerializeField]
    private GameObject particleEffect;    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlaySound(sound);

        var hit = Instantiate(particleEffect);
        hit.transform.position = collision.contacts[0].point;
        
        Destroy(hit, 3);
    }
}
