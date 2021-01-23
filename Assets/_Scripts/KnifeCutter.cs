using EzySlice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCutter : MonoBehaviour
{

    public List<CutMaterial> CutMaterials;

    private GameObject source;
    private Material crossMat;
    public bool recursiveSlice;

    private bool cutting;


    [ContextMenu("Do Something")]
    public void CutObject()
    {
        crossMat = CutMaterials.Find(e => source.name.Contains(e.FoodName)).MaterialAfterCut;
        if (!recursiveSlice)
        {
            
            SlicedHull hull = source.Slice(transform.position, transform.up, crossMat);

            if (hull != null)
            {
                GameObject lowerHull = hull.CreateLowerHull(source, crossMat);
                GameObject upperHull = hull.CreateUpperHull(source, crossMat);

                lowerHull.name = source.name.Split(' ')[0] + " lowerCut";
                upperHull.name = source.name.Split(' ')[0] + " upperCut";

                MeshCollider lowCol = lowerHull.AddComponent<MeshCollider>();
                MeshCollider upCol = upperHull.AddComponent<MeshCollider>();
                lowCol.convex = true;
                lowCol.isTrigger = true;
                upCol.convex = true;
                upCol.isTrigger = true;
                

                print("cut");

                source.SetActive(false);
            }
        }
        else
        {
            // in here we slice both the parent and all child objects
            SliceObjectRecursive(source, crossMat);

            source.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cutting)
        {
            cutting = true;
            source = other.gameObject;
            CutObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        cutting = false;
    }


    public GameObject[] SliceObjectRecursive(GameObject obj, Material crossSectionMaterial)
    {

        // finally slice the requested object and return
        SlicedHull finalHull = source.Slice(transform.position, transform.up, crossMat);

        if (finalHull != null)
        {
            GameObject lowerParent = finalHull.CreateLowerHull(obj, crossMat);
            GameObject upperParent = finalHull.CreateUpperHull(obj, crossMat);

            if (obj.transform.childCount > 0)
            {
                foreach (Transform child in obj.transform)
                {
                    if (child != null && child.gameObject != null)
                    {

                        // if the child has chilren, we need to recurse deeper
                        if (child.childCount > 0)
                        {
                            GameObject[] children = SliceObjectRecursive(child.gameObject, crossSectionMaterial);

                            if (children != null)
                            {
                                // add the lower hull of the child if available
                                if (children[0] != null && lowerParent != null)
                                {
                                    children[0].transform.SetParent(lowerParent.transform, false);
                                }

                                // add the upper hull of this child if available
                                if (children[1] != null && upperParent != null)
                                {
                                    children[1].transform.SetParent(upperParent.transform, false);
                                }
                            }
                        }
                        else
                        {
                            // otherwise, just slice the child object
                            SlicedHull hull = source.Slice(transform.position, transform.up, crossMat);

                            if (hull != null)
                            {
                                GameObject childLowerHull = hull.CreateLowerHull(child.gameObject, crossMat);
                                GameObject childUpperHull = hull.CreateUpperHull(child.gameObject, crossMat);

                                // add the lower hull of the child if available
                                if (childLowerHull != null && lowerParent != null)
                                {
                                    childLowerHull.transform.SetParent(lowerParent.transform, false);
                                }

                                // add the upper hull of the child if available
                                if (childUpperHull != null && upperParent != null)
                                {
                                    childUpperHull.transform.SetParent(upperParent.transform, false);
                                }
                            }
                        }
                    }
                }
            }

            return new GameObject[] { lowerParent, upperParent };
        }

        return null;
    }
}

[Serializable]
public class CutMaterial
{
    public string FoodName;
    public Material MaterialAfterCut;
}
