using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResourceItem : MonoBehaviour
{
    public CraftingManager.ResourceType resourceType; 
    public int amount = 1;
    public bool isTool = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on this object!");
            return;
        }

        grabInteractable.selectExited.AddListener(OnSelectExited);
    }
    
    // Xử lý khi người chơi nhả vật thể
    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!isTool)
        {
            if (CraftingManager.instance != null)
            {
                CraftingManager.instance.AddResource(resourceType, amount);
            }
            Destroy(gameObject);
        }
        else
        {
            // Logic cho công cụ sẽ được xử lý bởi XR Socket Interactor
            
        }
    }
}