using System.Linq;
using Layouter;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public  class Page : MonoBehaviour
{
    [SerializeField] private RectTransform leftTransformParent;
    [SerializeField] private RectTransform rightTransformParent;
    
    private Animator _animator;
    
    private static readonly int GoRight = Animator.StringToHash("goRight");
    private static readonly int GoLeft = Animator.StringToHash("goLeft");
    private static readonly int Right = Animator.StringToHash("right");
    private static readonly int Left = Animator.StringToHash("left");

    private AnimationClip _goRightAnimation;
    private AnimationClip _goLeftAnimation;

    public float GoRightAnimationLength => _goRightAnimation.length;
    public float GoLeftAnimationLength => _goLeftAnimation.length;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _goRightAnimation = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name.Equals("GoRight"));
        _goLeftAnimation = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name.Equals("GoLeft"));
    }

    public void GoFromRightToLeft()
    {
        _animator.SetTrigger(Right);
        _animator.SetTrigger(GoRight);
    }

    public void GoFromLeftToRight()
    {
        _animator.SetTrigger(Left);
        _animator.SetTrigger(GoLeft);
    }

    public void EnableLeft(PageLayout pageLayout1, PageLayout pageLayout2)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Left);
        
        SetRightPage(pageLayout1);
        SetLeftPage(pageLayout2);
    }
    
    public void EnableRight(PageLayout pageLayout1, PageLayout pageLayout2)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Right);
        
        SetRightPage(pageLayout1);
        SetLeftPage(pageLayout2);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void SetLeftPage(PageLayout page)
    {
        if (page == null)
        {
            return;
        }

        if(leftTransformParent.childCount > 0) Destroy(leftTransformParent.GetChild(0).gameObject);
        Instantiate(page, leftTransformParent);
    }

    private void SetRightPage(PageLayout page)
    {
        if (page == null)
        {
            return;
        }

        if(rightTransformParent.childCount > 0) Destroy(rightTransformParent.GetChild(0).gameObject);
        Instantiate(page, rightTransformParent);
    }
}