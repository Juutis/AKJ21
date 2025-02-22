using UnityEngine;
using UnityEngine.Events;

public class UICurtainTransition : MonoBehaviour
{
    private bool isShowing = false;
    private bool isHiding = false;
    private UnityAction showCallback;
    private UnityAction hideCallback;

    [SerializeField]
    private Animator animator;

    public bool Show(UnityAction afterCallback)
    {
        if (isShowing)
        {
            return false;
        }
        showCallback = afterCallback;
        animator.Play("curtainsShow");
        isShowing = true;
        return true;
    }

    public bool Hide(UnityAction afterCallback)
    {
        if (isHiding)
        {
            return false;
        }
        hideCallback = afterCallback;
        animator.Play("curtainsHide");
        isHiding = true;
        return true;
    }

    public void ShowFinished()
    {
        isShowing = false;
        showCallback?.Invoke();
    }


    public void HideFinished()
    {
        isHiding = false;
        hideCallback?.Invoke();
    }
}
