using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Spellpath.Input
{
	public class InputState
	{
		public Vector2 Movement;
		public bool Jump;
	}

	public interface IInputController
	{
		public void ModifyState(InputState state);
	}

	public class PlayerInputController : IInputController
	{
		public void ModifyState(InputState state)
		{
			state.Movement.X = InputManager.IsDown(Keys.D) - lev::Input::inst()->down_key(lev::KEY_A);
			state.Movement.Y = InputManager.IsDown(Keys.S) - lev::Input::inst()->down_key(lev::KEY_W);

			state.jump = lev::Input::inst()->pressed_key(lev::KEY_SPACE);
			
			if (InputManager.IsDown(Keys.A)) delta.X -= 0.5f;
			if (InputManager.IsDown(Keys.D)) delta.X += 0.5f;
			if (InputManager.IsDown(Keys.W)) delta.Y -= 0.5f;
			if (InputManager.IsDown(Keys.S)) delta.Y += 0.5f;
		}
	}

	public class DialogueInputController : IInputController
	{
		public void ModifyState(InputState state)
		{
		}
	}

	public class NavigationInputController : IInputController
	{
		public void ModifyState(InputState state)
		{
		}
	}

	public class InputProvider
	{
		private PlayerInputController m_playerInput;
		private DialogueInputController m_dialogueInput;
		private NavigationInputController m_navigationInput;

		public Action OnJump;

		public InputProvider()
		{
			m_playerInput = new PlayerInputController();
			m_dialogueInput = new DialogueInputController();
			m_navigationInput = new NavigationInputController();
		}

		public InputState GetState()
		{
			InputState state = new InputState();

			m_playerInput.ModifyState(state);
			m_dialogueInput.ModifyState(state);
			m_navigationInput.ModifyState(state);

			return state;
		}
	}
}
