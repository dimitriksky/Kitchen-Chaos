using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectdCounterChangedEventArgs> OnSelectdCounterChanged;
    public class OnSelectdCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    void Start() {
       gameInput.OnInteractAction += GameInput_OnInteraction;
    }

    private void GameInput_OnInteraction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact();
        }
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDirection = moveDir;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit,
            interactDistance, countersLayerMask)) {
           if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                // Has ClearCounter
                if (clearCounter != selectedCounter) {
                    SetSelectedCounter(clearCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();


        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;


        bool canMove = false;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

        foreach (Vector3 direction in new[] { moveDir, moveDirX, moveDirZ }) {
            canMove = !Physics.CapsuleCast(transform.position,
                        transform.position + Vector3.up * playerHeight, playerRadius,
                        direction, moveDistance);

            if (canMove)
            {
                moveDir = direction;
                break;
            }
        }

        if (canMove) {
            // Can move in a valid direction
            transform.position += moveDistance * moveDir;
        }


        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,
            Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectdCounterChanged.Invoke(this, new OnSelectdCounterChangedEventArgs {
                        selectedCounter = selectedCounter
                        });
    }

}
