using UnityEngine;

public class RandomParticleColor : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
    }

    private void Update()
    {
        mainModule.startColor = Random.ColorHSV(
            0f, 1f,
            .7f, 1f,
            .8f, 1f,
            1f, 1f);
    }
}
