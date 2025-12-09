using UnityEngine;

public class PlayerHandAnimatorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator handAnimator;

    public void PlayCast()
    {
        handAnimator.SetBool("isCasting", true);
    }

    public void PlayUltimateCast()
    {
        handAnimator.SetBool("isUltimateCasting", true);
    }

    public void PlayFailedCast()
    {
        handAnimator.SetBool("castFailed", true);
    }

    public void ReturnToIdle()
    {
        handAnimator.SetBool("isCasting", false);
        handAnimator.SetBool("isUltimateCasting", false);
        handAnimator.SetBool("castFailed", false);
    }
}
