using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    // PLAYER //
    public void PlayFootstep()
    {
        Radio.Instance.PlaySFX("SFX/PlayerWalking", audioSource);
    }
//maconha//
    public void PlayShotgunShot()
    {
        Radio.Instance.PlaySFX("SFX/ShotgunShot", audioSource);
    }

    public void PlayShotgunReload()
    {
        Radio.Instance.PlaySFX("SFX/ShotgunReload", audioSource);
    }
}
