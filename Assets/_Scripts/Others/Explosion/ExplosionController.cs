using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    void Start()
    {
        // Al iniciar, genera el sonido de la explosion y se destruye en 0,5 seg
        AudioSource explosionSound = GetComponent<AudioSource>();
        explosionSound.enabled = true;
        explosionSound.Play();
        Destroy(gameObject, 0.5f);
    }
}