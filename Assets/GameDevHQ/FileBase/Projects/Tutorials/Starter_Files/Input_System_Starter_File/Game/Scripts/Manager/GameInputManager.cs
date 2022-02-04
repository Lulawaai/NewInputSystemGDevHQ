using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInputManager : MonoBehaviour
{
	private GameInputActions _input;

	public static event Action<Vector2> OnPlayerMove;

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();
    }

    void Update()
    {
		Vector2 move;
		move = _input.PlayerInput.Move.ReadValue<Vector2>();

		OnPlayerMove?.Invoke(move);
    }
}
