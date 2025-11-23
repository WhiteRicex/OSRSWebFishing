using UnityEngine;

public class Ore : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField]
    Renderer _oreVisual;

    bool _mined;

    float _oreRegenDelay = 3;

    void Regenerate()
    {
        _oreVisual.enabled = true;
        _mined = false;
    }

    void Mine()
    {
        if(_mined)
        {
            Debug.Log("Rice Error: unable to mine - ore empty");
        }

        _oreVisual.enabled = false;
        _mined = true;

        Invoke(nameof(Regenerate), _oreRegenDelay);
    }
}
