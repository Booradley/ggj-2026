using UnityEngine;

public class SoundPlayerVolume1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField, Range(0f, 100f)]
    public float radius = 10f;

    [SerializeField, Range(-100f,10f)]
    public float level = -6f;

    [SerializeField]
    public bool playOnLoad = false;

    [SerializeField]
    public AudioSource file;
}
