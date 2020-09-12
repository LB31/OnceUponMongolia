using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapController : MonoBehaviour
{
    public Transform SnapParent;
    public bool InCollider;
    public GameObject GrabbedObject;
    public Transform SelectedSnapZone;

    private void Start()
    {
        foreach (Transform housePart in SnapParent)
        {
            CollisionDetector cs = housePart.gameObject.AddComponent<CollisionDetector>();
            cs.SnapController = this;
        }
    }

    [ContextMenu("Do Something")]
    public async void CheckIfSnap()
    {
        if (InCollider)
        {
            Destroy(GrabbedObject);
            SelectedSnapZone.GetComponent<MeshRenderer>().enabled = false;
            SelectedSnapZone.GetChild(0).gameObject.SetActive(true);

            InCollider = false;
            GrabbedObject = null;
            SelectedSnapZone = null;
            Debug.LogError("in collider");
        }
        else
        {
            Debug.LogError("not in collider");
            await Task.Delay(500);
            GrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            GrabbedObject.GetComponent<Rigidbody>().useGravity = true;

            GrabbedObject.GetComponent<Collider>().isTrigger = false;

            GrabbedObject = null;
        }
    }

    public void RegisterGrabbedOjbect(GameObject obj)
    {
        GrabbedObject = obj;
        GrabbedObject.GetComponent<Collider>().isTrigger = true;
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
            SnapController.SelectedSnapZone = this.transform;
            print("trigger enter");
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
            print("trigger exit");
        }
    }
}
