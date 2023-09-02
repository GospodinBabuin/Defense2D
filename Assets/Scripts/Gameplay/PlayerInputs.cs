using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public float move;
    public bool atack;
    public bool shop;
    public bool menu;

    public bool cursorLocked = true;

    private void OnMove(InputValue value)
    {
        MoveInput(value.Get<float>());
    }

    private void OnAtack(InputValue value)
    {
        AtackInput(value.isPressed);
    }

    private void OnShop()
    {
        ShopInput(valut.isPressed);
    }

    private void OnMenu(InputValue value)
    {
        MenuInput(value.isPressed)
    }

    private void MoveInput(float newMoveDirection)
    {
        move = newMoveDirection;
    }

    private void AtackInput(bool newAtackState)
    {
        atack = newAtackState;
    }

    private void ShopInput(bool newShopState)
    {
        shop = newShopState;
    }

    private void MenuInput(bool newMenuState)
    {
        menu = newMenuState;
    }

    //private void OnApplicationFocus(bool hasFocus)
    //{
    //    SetCursorState(cursorLocked);
    //}

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}