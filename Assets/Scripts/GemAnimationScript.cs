using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Utilities;

public class GemAnimationScript : MonoBehaviour
{
    private Animator animator;
    private GemStabilityLevel currentLevel;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentLevel = GemStabilityLevel.Stable;
    }

    public void switchAnimation(GemStabilityLevel level)
    {
        switch (currentLevel)
        {
            case GemStabilityLevel.Stable:
                animator.SetBool("isStable", false);
                break;
            case GemStabilityLevel.Wavering:
                animator.SetBool("isWavering", false);
                break;
            case GemStabilityLevel.Disrupted:
                animator.SetBool("isDisrupted", false);
                break;
            case GemStabilityLevel.Unstable:
                animator.SetBool("isUnstable", false);
                break;
        }

        switch (level)
        {
            case GemStabilityLevel.Stable:
                animator.SetBool("isStable", true);
                currentLevel = GemStabilityLevel.Stable;
                break;
            case GemStabilityLevel.Wavering:
                animator.SetBool("isWavering", true);
                currentLevel = GemStabilityLevel.Wavering;
                break;
            case GemStabilityLevel.Disrupted:
                animator.SetBool("isDisrupted", true);
                currentLevel = GemStabilityLevel.Disrupted;
                break;
            case GemStabilityLevel.Unstable:
                animator.SetBool("isUnstable", true);
                currentLevel = GemStabilityLevel.Unstable;
                break;
        }
    }

}
