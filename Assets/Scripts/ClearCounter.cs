using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public void Interact() {
        print("Interact");
        Transform kitchenObject = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
        kitchenObject.localPosition = Vector3.zero;
    }
}
