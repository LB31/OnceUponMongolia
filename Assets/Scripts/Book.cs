using System.Collections;
using Layouter;
using UnityEngine;

public class Book : MonoBehaviour
{
    [Header("Animated Pages")]
    [SerializeField] private Page page1 = null;
    [SerializeField] private Page page2 = null;
    [SerializeField] private Page page3 = null;

    [Header("Page Content")]
    [SerializeField] private PageLayout[] pages;
    
    private int _pageIndex;
    private bool _animating;
    
    private Page LeftPage => GetPageByIndex(_pageIndex);
    private Page RightPage => GetPageByIndex(_pageIndex + 2);
    private Page StandbyPage => GetPageByIndex(_pageIndex + 4);

    private void Start()
    {
        StandbyPage.Disable();
        LeftPage.EnableLeft(null, GetPageLayoutByIndex(0));
        RightPage.EnableRight(GetPageLayoutByIndex(1), GetPageLayoutByIndex(2));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryForward();
        }
        if (Input.GetMouseButtonDown(1))
        {
            TryBackward();
        }
    }

    private void TryForward()
    {
        if (_animating)
            return;

        if (_pageIndex == pages.Length - 2)
            return;
        
        StartCoroutine(Forward());
    }

    private void TryBackward()
    {
        if (_animating)
            return;

        if (_pageIndex == 0)
            return;
        
        StartCoroutine(Backward());
    }

    private IEnumerator Forward()
    {
        var newPageIndex = _pageIndex + 2;
        
        _animating = true;
        RightPage.GoFromRightToLeft();
        StandbyPage.EnableRight(GetPageLayoutByIndex(newPageIndex + 1), GetPageLayoutByIndex(newPageIndex + 2));
        
        yield return new WaitForSeconds(RightPage.GoRightAnimationLength);
        
        _animating = false;
        LeftPage.Disable();
        
        _pageIndex = newPageIndex;
    }
    
    private IEnumerator Backward()
    {
        var newPageIndex = _pageIndex - 2;
        
        _animating = true;
        LeftPage.GoFromLeftToRight();
        StandbyPage.EnableLeft(GetPageLayoutByIndex(newPageIndex - 1), GetPageLayoutByIndex(newPageIndex));
        
        yield return new WaitForSeconds(LeftPage.GoLeftAnimationLength);
        
        _animating = false;
        RightPage.Disable();
        
        _pageIndex = newPageIndex;
    }

    private PageLayout GetPageLayoutByIndex(int index)
    {
        if (index < 0 || index >= pages.Length)
        {
            return null;
        }
        
        return pages[index];
    }
    
    /*
     * In:  -2 | -1 | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | ...
     * Out:  3 |  1 | 1 | 2 | 2 | 3 | 3 | 1 | 1 | 2 | ...
     */
    private Page GetPageByIndex(int index)
    {
        index = Mathf.CeilToInt(index / 2f) + 1;
        index = (index % 3 + 3) % 3;
        
        switch (index)
        {
            case 1: return page1;
            case 2: return page2;
            case 0: return page3;
        }

        return null;
    }
}