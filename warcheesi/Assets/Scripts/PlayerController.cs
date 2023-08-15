using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    private InputActions inputActions;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Click.performed += OnPerformedClick;

    }

    private void OnPerformedClick(CallbackContext obj)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out Square square))
            {
                Debug.Log($"square {square.name}");
                GameManager.Instance.SelectSquare(square);
            }
            else if (hitInfo.collider.GetComponentInParent<Token>() != null)
            {
                var token = hitInfo.collider.GetComponentInParent<Token>();
                Debug.Log($"token {token.name} {token.GetTeam()}");
                GameManager.Instance.SelectToken(token);
            }
            else
            {
                GameManager.Instance.DeselectCurrentToken();
            }
        }
    }
}
