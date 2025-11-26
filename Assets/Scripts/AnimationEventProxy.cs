using UnityEngine;

public class AnimationEventProxy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;

    public void PlayFootstep()
    {
        Radio.Instance.PlaySFX("SFX/PlayerWalking", audioSource);
    }
}
