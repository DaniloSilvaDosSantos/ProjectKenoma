using UnityEngine;

public class FakeStopMotion : MonoBehaviour
{
    public Animator Animator;
    public int FPS = 8;

    private float _time;

    private void OnValidate()
    {
        if (!Animator) Animator = GetComponent<Animator>();

    }

    private void Update()
    {
        _time += Time.deltaTime;
        var updateTime = 1f / FPS;
        Animator.speed = 0;

        if (_time > updateTime)
        {
            _time -= updateTime;
            Animator.speed = updateTime / Time.deltaTime;
        }
    }
}
