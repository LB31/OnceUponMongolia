using System;
using System.Collections;
using Layouter;
using UnityEngine;
using UnityEngine.EventSystems;

public class Book : MonoBehaviour
{
    [Header("Animated Pages")]
    [SerializeField] private Page page1 = null;
    [SerializeField] private Page page2 = null;
    [SerializeField] private Page page3 = null;

    [Header("Page Content")]
    [SerializeField] private PageLayout[] pages = new PageLayout[0];
    
    private int _contentIndex;
    private int _pageIndex = 1;
    private bool _animating;
    
    private Page LeftPage => GetPageByIndex(_pageIndex);
    private Page RightPage => GetPageByIndex(_pageIndex + 1);
    private Page StandbyPage => GetPageByIndex(_pageIndex + 2);

    private void Start()
    {
        StandbyPage.Disable();
        LeftPage.EnableLeft(GetContentByIndex(_contentIndex));
        RightPage.EnableRight(GetContentByIndex(_contentIndex + 1));
        TriggerPages();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            GoToContent(_contentIndex + 2);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GoToContent(_contentIndex - 2);
        }
    }
    
    public void GoToContent(int index)
    {
        if (_animating)
        {
            Debug.Log($"Book is still animating and cant go to page {index}!");
            return;
        }

        if (index < 0 || index > pages.Length - 2)
        {
            Debug.LogWarning($"Page index {index} is out of range!");
            return;
        }

        var oldIndex = _contentIndex;
        _contentIndex = index % 2 == 1 ? index - 1 : index;

        if (oldIndex == _contentIndex)
        {
            Debug.Log($"Already on page {_contentIndex}. No need to go there!");
            return;
        }
        
        StartCoroutine(_contentIndex < oldIndex ? AnimateBackward() : AnimateForward());
    }

    private IEnumerator AnimateBackward()
    {
        _animating = true;
        LeftPage.GoFromLeftToRight(GetContentByIndex(_contentIndex + 1));
        StandbyPage.EnableLeft(GetContentByIndex(_contentIndex));
        
        yield return new WaitForSeconds(LeftPage.GoLeftAnimationLength);
        _animating = false;
        
        _pageIndex--;
        StandbyPage.Disable();
        
        TriggerPages();
    }

    private IEnumerator AnimateForward()
    {
        _animating = true;
        RightPage.GoFromRightToLeft(GetContentByIndex(_contentIndex));
        StandbyPage.EnableRight(GetContentByIndex(_contentIndex + 1));
        
        yield return new WaitForSeconds(RightPage.GoRightAnimationLength);
        _animating = false;
        
        _pageIndex++;
        StandbyPage.Disable();
        
        TriggerPages();
    }

    private void TriggerPages()
    {
        LeftPage.TriggerLeftContent();
        RightPage.TriggerRightContent();
    }

    private PageLayout GetContentByIndex(int index)
    {
        if (index < 0 || index >= pages.Length)
        {
            return null;
        }
        
        return pages[index];
    }
    
    private Page GetPageByIndex(int index)
    {
        index = (index % 3 + 3) % 3;
        
        switch (index)
        {
            case 1: return page1;
            case 2: return page2;
            case 0: return page3;
            default: return null;
        }
    }
}