using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private float _loadTimeRemaining = 4f;

    // Update is called once per frame
    void Update()
    {
        _loadTimeRemaining -= Time.deltaTime;
        if (_loadTimeRemaining <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
