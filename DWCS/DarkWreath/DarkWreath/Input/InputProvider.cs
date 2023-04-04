using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DarkWreath.Input
{
	// TODO: REFACTOR THIS
	
	public struct InputState
	{
		public Vector2 Movement;
		public bool FireButtonPressed;
		public bool MarauderModeToggle;
		public bool DoReload; // NO IDEA WHAT TO CALL THIS (YOU REMOVE THE AMMO AND CAN RELOAD THE WEAPON)
		public bool StartReload;
		public bool QuitReload;
		public bool AimDownSights;
	}

	public interface IInputController
	{
		public void ModifyState(ref InputState state);
	}

	public class PlayerInputController : IInputController
	{
		public void ModifyState(ref InputState state)
		{
			state.Movement.X = Convert.ToInt32(InputManager.IsDown(Keys.D)) - Convert.ToInt32(InputManager.IsDown(Keys.A));
			state.Movement.Y = Convert.ToInt32(InputManager.IsDown(Keys.S)) - Convert.ToInt32(InputManager.IsDown(Keys.W));
			
			state.FireButtonPressed = InputManager.IsPressed(MouseButton.Left);
			state.MarauderModeToggle = InputManager.IsPressed(Keys.Q);
			state.DoReload = InputManager.IsDown(Keys.G);
			state.StartReload = InputManager.IsPressed(Keys.G);
			state.QuitReload = InputManager.IsReleased(Keys.G);
			state.AimDownSights = InputManager.IsDown(MouseButton.Right);
		}
	}

	public class DialogueInputController : IInputController
	{
		public void ModifyState(ref InputState state)
		{
		}
	}

	public class NavigationInputController : IInputController
	{
		public void ModifyState(ref InputState state)
		{
		}
	}

	public interface IInputProvider
	{
		public InputState GetState();
	}

	public class InputProvider : IInputProvider
	{
		private PlayerInputController playerInput;
		private DialogueInputController dialogueInput;
		private NavigationInputController navigationInput;

		public Action OnJump;

		public InputProvider()
		{
			playerInput = new PlayerInputController();
			dialogueInput = new DialogueInputController();
			navigationInput = new NavigationInputController();
		}

		public InputState GetState()
		{
			InputState state = new InputState();

			playerInput.ModifyState(ref state);
			dialogueInput.ModifyState(ref state);
			navigationInput.ModifyState(ref state);

			return state;
		}
	}
}
