using EzySlice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class KnifeCutter : MonoBehaviour
{

    public List<CutMaterial> CutMaterials;
    public bool recursiveSlice;


    private GameObject objToCut;
    private Material crossMat;

    private bool cutting;
    private AudioSource audioSource;

    private void Awake()
    {
        crossMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound()
    {
        audioSource.time = 0.5f;
        audioSource.Play();
    }

    private void PrepareNewHull(GameObject hull)
    {
        float newPos = Random.Range(-0.005f, 0.02f);
        hull.transform.position -= hull.transform.forward * newPos;
        hull.layer = 12;
        MeshCollider col = hull.AddComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;
        hull.AddComponent<XRGrabInteractable>();
        hull.GetComponent<Rigidbody>().useGravity = false;
    }

    [ContextMenu("Do Something")]
    public async void CutObject()
    {
        crossMat = CutMaterials.FirstOrDefault(e => objToCut.name.ToLower().Contains(e.FoodName)).MaterialAfterCut;
        if (!crossMat) return;

        if (!recursiveSlice)
        {
            PlaySound();  

            SlicedHull hull = objToCut.Slice(transform.position, transform.up, crossMat);

            if (hull != null)
            {
                GameObject lowerHull = hull.CreateLowerHull(objToCut, crossMat);
                GameObject upperHull = hull.CreateUpperHull(objToCut, crossMat);

                lowerHull.name = objToCut.name.Split(' ')[0] + " lowerCut";
                upperHull.name = objToCut.name.Split(' ')[0] + " upperCut";

                PrepareNewHull(lowerHull);
                PrepareNewHull(upperHull);

                objToCut.SetActive(false);

                await Task.Delay(1000);
                cutting = false;
            }
        }
        else
        {
            // in here we slice both the parent and all child objects
            SliceObjectRecursive(objToCut, crossMat);

            objToCut.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("entered " + other.name);

        string name = other.name.ToLower();
        if (name.Contains("hand")) return;


        objToCut = other.gameObject;

        if (!cutting)
        {
            cutting = true;
            CutObject();
        }
            

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    cutting = false;
    //    print("trigger exit " + other);
    //}


    public GameObject[] SliceObjectRecursive(GameObject obj, Material crossSectionMaterial)
    {

        // finally slice the requested object and return
        SlicedHull finalHull = objToCut.Slice(transform.position, transform.up, crossMat);

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
                            SlicedHull hull = objToCut.Slice(transform.position, transform.up, crossMat);

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
