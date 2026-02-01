using UnityEngine;

public class Mask : MonoBehaviour
{
    public MeshRenderer eyes;
    public MeshRenderer nose;
    public MeshRenderer mouth;

    private GameController _gameController;

    void Awake()
    {
        _gameController = GameObject.FindFirstObjectByType<Main>().GameController;
    }

    public void SetIndex(int growthStage, int index)
    {
        Debug.Log(_gameController.GetTextures(growthStage)[index]);
        if (growthStage == 0)
        {
            eyes.material.SetTexture(Shader.PropertyToID("_BaseMap"), _gameController.GetTextures(growthStage)[index]);
        }
        else if (growthStage == 1)
        {
            nose.material.SetTexture(Shader.PropertyToID("_BaseMap"), _gameController.GetTextures(growthStage)[index]);
        }
        else
        {
            mouth.material.SetTexture(Shader.PropertyToID("_BaseMap"), _gameController.GetTextures(growthStage)[index]);
        }
    }
}