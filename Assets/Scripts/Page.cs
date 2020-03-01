using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public  class Page : MonoBehaviour
{
    [SerializeField] private Renderer leftRenderer;
    [SerializeField] private Renderer rightRenderer;
    
    private Animator _animator;
    
    private static readonly int goRight = Animator.StringToHash("goRight");
    private static readonly int goLeft = Animator.StringToHash("goLeft");
    private static readonly int right = Animator.StringToHash("right");
    private static readonly int left = Animator.StringToHash("left");

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
        _animator.SetTrigger(right);
        _animator.SetTrigger(goRight);
    }

    public void GoFromLeftToRight()
    {
        _animator.SetTrigger(left);
        _animator.SetTrigger(goLeft);
    }

    public void EnableLeft(Texture2D background1, Texture2D background2)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(left);
        
        SetRightTexture(background1);
        SetLeftTexture(background2);
    }
    
    public void EnableRight(Texture2D background1, Texture2D background2)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(right);
        
        SetRightTexture(background1);
        SetLeftTexture(background2);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void SetLeftTexture(Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }
        
        leftRenderer.material.mainTexture = texture;
    }

    private void SetRightTexture(Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }
        
        rightRenderer.material.mainTexture = texture;
    }
}