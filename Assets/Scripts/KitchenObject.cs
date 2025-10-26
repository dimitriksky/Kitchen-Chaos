using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSO;
    }
    public void SetClearCounter(ClearCounter counter) {
        if (clearCounter != null) {
            clearCounter.ClearKitchenObject();
        }

        clearCounter = counter;
        if (counter.HasKitchenObject()) {
            Debug.LogError("Counter already has a kitchen object!");
        }
        counter.SetKitchenObject(this);

        transform.parent = counter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public ClearCounter GetClearCounter() {
        return clearCounter;
    }
}
