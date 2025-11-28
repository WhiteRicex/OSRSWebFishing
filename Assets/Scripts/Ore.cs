using UnityEngine;

public class Ore : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField]
    Renderer _oreVisual;
    [SerializeField]
    Material _oreMat;
    [SerializeField]
    Material _depletedMat;

    bool _mined;

    float _oreRegenDelay = 3;

    void Regenerate()
    {
        _mined = false;

        _oreVisual.material = _oreMat;
    }

    public void Mine()
    {
        Debug.LogWarning("RICE TODO: Convert this into an event SO");

        if(_mined)
        {
            Debug.Log("Rice Error: unable to mine - ore empty");
        }

        _mined = true;

        _oreVisual.material = _depletedMat;

        Invoke(nameof(Regenerate), _oreRegenDelay);
    }
}
