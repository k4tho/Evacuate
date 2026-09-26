using UnityEngine;
using System;  

public class IntroCutScene : MonoBehaviour
{
    private Animator animator;
    public CanvasGroup blackFade;
    public event Action OnIntroFinished;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.Play("Wake");
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            blackFade.alpha = 0f;
            animator.enabled = false;
            
            OnIntroFinished?.Invoke();
            
            Destroy(this);
        }
    }
}
