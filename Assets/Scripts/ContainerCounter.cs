using UnityEngine;

public class ContainerCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event System.EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            OnPlayerGrabbedObject?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
