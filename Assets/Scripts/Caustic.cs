using UnityEngine;

public class Caustic : MonoBehaviour
{

    float fps = 30.0f;

    [SerializeField]
    Texture2D[] frames;

    [SerializeField]
    Projector caustic;

    private int frameIndex;

    void Start()
    {
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        caustic.material.SetTexture("_ShadowTex", frames[frameIndex]);
        frameIndex = (frameIndex + 1) % frames.Length;
    }

}
