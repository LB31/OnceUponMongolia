using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapController : MonoBehaviour
{
    public Transform SnapParent;
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
    }

    // Called in XRDirectInteractor by On Selected Exited Interactor Event by hands/rays
    [ContextMenu("CheckIfSnap")]
    public async void CheckIfSnap()
    {
        if (InCollider)
        {
            Destroy(GrabbedObject);
            SelectedSnapZone.GetComponent<MeshRenderer>().enabled = false;
            SelectedSnapZone.GetChild(0).gameObject.SetActive(true);

            InCollider = false;       
            SelectedSnapZone = null;
        }
        else
        {
            await Task.Delay(500);
            Rigidbody rg = GrabbedObject.GetComponent<Rigidbody>();
            rg.isKinematic = false;
            rg.useGravity = true;

            GrabbedObject.GetComponent<Collider>().isTrigger = false;
        }
        GrabbedObject = null;
    }

    // Called in XRGrabInteractable by On Select Entered Interactable Event by grabbed object
    public void RegisterGrabbedOjbect(GameObject obj)
    {
        GrabbedObject = obj;
        GrabbedObject.GetComponent<Collider>().isTrigger = true;
        GrabbedObject.transform.localScale *= 3;
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
