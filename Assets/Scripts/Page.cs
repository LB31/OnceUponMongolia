using System.Linq;
using Layouter;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public  class Page : MonoBehaviour
{
    [SerializeField] private RectTransform leftTransformParent = null;
    [SerializeField] private RectTransform rightTransformParent = null;
    
    private Animator _animator;
    
    private static readonly int GoRight = Animator.StringToHash("goRight");
    private static readonly int GoLeft = Animator.StringToHash("goLeft");
    private static readonly int Right = Animator.StringToHash("right");
    private static readonly int Left = Animator.StringToHash("left");

    private AnimationClip _goRightAnimation;
    private AnimationClip _goLeftAnimation;

    private PageLayout _leftContent;
    private PageLayout _rightContent;

    public float GoRightAnimationLength => _goRightAnimation.length;
    public float GoLeftAnimationLength => _goLeftAnimation.length;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _goRightAnimation = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name.Equals("GoRight"));
        _goLeftAnimation = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name.Equals("GoLeft"));
    }

    public void GoFromRightToLeft(PageLayout leftPageLayout)
    {
        SetLeftPage(leftPageLayout);
        
        _animator.SetTrigger(Right);
        _animator.SetTrigger(GoRight);
    }

    public void GoFromLeftToRight(PageLayout rightPageLayout)
    {
        SetRightPage(rightPageLayout);
        
        _animator.SetTrigger(Left);
        _animator.SetTrigger(GoLeft);
    }

    public void EnableLeft(PageLayout leftPageLayout)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Left);
        
        SetLeftPage(leftPageLayout);
    }
    
    public void EnableRight(PageLayout rightPageLayout)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Right);
        
        SetRightPage(rightPageLayout);
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
        _leftContent = Instantiate(page, leftTransformParent);
    }

    private void SetRightPage(PageLayout page)
    {
        if (page == null)
        {
            return;
        }

        if(rightTransformParent.childCount > 0) Destroy(rightTransformParent.GetChild(0).gameObject);
        _rightContent = Instantiate(page, rightTransformParent);
    }

    public void TriggerLeftContent()
    {
        if(_leftContent != null) _leftContent.TriggerAll();
    }

    public void TriggerRightContent()
    {
        if(_rightContent != null) _rightContent.TriggerAll();
    }
}