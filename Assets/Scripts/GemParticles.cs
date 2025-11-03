using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemParticles : MonoBehaviour
{
    [SerializeField]
    ParticleSystem gemPuff;

    [SerializeField]
    ParticleSystem gemExhale;

    public void playGemPuff()
    {
        gemPuff.Play();
    }

    public void playGemExhale()
    {
        gemExhale.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
