using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapController : MonoBehaviour
{
    public Transform SnapParent;
    public BuildPart AllWoodenParts;
    public BuildPart AllBlanketParts;

    [HideInInspector] public bool InCollider;
    [HideInInspector] public Transform SelectedSnapZone;
    private GameObject GrabbedObject;
    

    private void Start()
    {
        foreach (Transform housePart in SnapParent)
        {
            CollisionDetector cs = housePart.gameObject.AddComponent<CollisionDetector>();
            cs.SnapController = this;
        }

        AllWoodenParts.AllParts[0].SetActive(true);
        AllBlanketParts.AllParts[0].SetActive(true);
    }

    // Called in XRDirectInteractor by On Selected Exited Interactor Event by hands/rays
    [ContextMenu("CheckIfSnap")]
    public async void CheckIfSnap()
    {
        if (InCollider)
        {          
            SelectedSnapZone.GetComponent<MeshRenderer>().enabled = false;
            SelectedSnapZone.GetChild(0).gameObject.SetActive(true);

            InCollider = false;       
            SelectedSnapZone = null;

            // Prepare next object
            List<BuildPart.ConstructionParts> AllConstructionParts = new List<BuildPart.ConstructionParts>();

            // Get rid of reference of snapped Yurt Part from 'Blanket' and activate the next if there is one
            if (AllBlanketParts.AllParts.Contains(GrabbedObject))
            {
                AllBlanketParts.AllParts.RemoveAt(0);
                if (AllBlanketParts.AllParts.Count > 0) AllBlanketParts.AllParts[0].SetActive(true);
                AllConstructionParts = AllBlanketParts.AllPartsToRemove;
                
            }
            // Get rid of reference of snapped Yurt Part from 'Log' and activate the next if there is one
            else
            {
                AllWoodenParts.AllParts.RemoveAt(0);
                if(AllWoodenParts.AllParts.Count > 0) AllWoodenParts.AllParts[0].SetActive(true);
                AllConstructionParts = AllWoodenParts.AllPartsToRemove;
            }
            // Remove Log or Blanket Row in Scene
            foreach (GameObject item in AllConstructionParts[0].PartsToRemove)
            {
                Destroy(item);
            }
            AllConstructionParts.RemoveAt(0);
            // Get rid of snapped object
            Destroy(GrabbedObject);
        }
        else
        {
            if (!GrabbedObject) return;
            await Task.Delay(500);
            //Rigidbody rg = GrabbedObject.GetComponent<Rigidbody>();
            //rg.isKinematic = false;
            //rg.useGravity = true;
            GrabbedObject.GetComponent<MeshRenderer>().enabled = false;
            GrabbedObject.GetComponent<Collider>().isTrigger = false;
            GrabbedObject.GetComponent<ItemRespawner>().ReturnToOriginalPos();
        }

        GrabbedObject = null;

        // TODO check if yurt is ready
        if (AllBlanketParts.AllParts.Count == 0 &&AllWoodenParts.AllParts.Count == 0)
        {
            Debug.Log("READY WITH BUILDING YURT");
            PlayMakerFSM.BroadcastEvent("ContinueStory");
        }

        
    }

    // Called in XRGrabInteractable by On Select Entered Interactable Event by grabbed object
    public void RegisterGrabbedOjbect(GameObject obj)
    {
        GrabbedObject = obj;
        GrabbedObject.GetComponent<Collider>().isTrigger = true;
        GrabbedObject.GetComponent<MeshRenderer>().enabled = true;
        float newScale = 0.2f;
        GrabbedObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }



}

public class CollisionDetector : MonoBehaviour
{
    public SnapController SnapController;
    public MeshRenderer MeshRenderer;

    private void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When snapping part with the same name as snap zone has triggered
        if (other.name.Equals(name))
        {
            MeshRenderer.enabled = true;
            SnapController.InCollider = true;
            //SnapController.GrabbedObject = other.gameObject;
            SnapController.SelectedSnapZone = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals(name))
        {
            MeshRenderer.enabled = false;
            SnapController.InCollider = false;
            //SnapController.GrabbedObject = null;
            SnapController.SelectedSnapZone = null;
        }
    }
}

[Serializable]
public class BuildPart
{
    public List<GameObject> AllParts;
    // Blankets and Logs
    public List<ConstructionParts> AllPartsToRemove;

    [Serializable]
    public struct ConstructionParts
    {
        public List<GameObject> PartsToRemove;
    }
}
