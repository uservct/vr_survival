using UnityEngine;

public class CookZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Cookable cookable = other.GetComponent<Cookable>();
        if (cookable != null)
        {
            cookable.StartCooking();
            Debug.Log($"🔥 {other.name} bắt đầu nấu trong vùng lửa!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Cookable cookable = other.GetComponent<Cookable>();
        if (cookable != null)
        {
            cookable.StopCooking();
            Debug.Log($"🧊 {other.name} rời vùng lửa, dừng nấu!");
        }
    }
}
