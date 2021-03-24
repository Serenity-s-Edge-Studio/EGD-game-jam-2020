using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Difficulty Settings", menuName = "Difficulty")]
public class DifficultySettingsSO : ScriptableObject
{
    [SerializeField]
    private int MinVisibility, MaxVisiblity;
    [SerializeField]
    private AnimationCurve VisibilityToTimeRatio;

    public int MaxZombies;
    public float ZombieIncreaseMulti;
    public int StartingMaxZombies;

    public float ComputeVisibility(float time)
    {
        return MinVisibility + VisibilityToTimeRatio.Evaluate(time) * MaxVisiblity;
    }
}
