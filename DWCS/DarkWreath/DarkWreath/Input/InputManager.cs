using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DarkWreath.Input
{
	public enum MouseButton
	{
		None = 0,
		Left,
		Middle,
		Right
	}

	public enum GamePadButton
	{
		None = 0,
		X,
		Y,
		A,
		B,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,
		LeftShoulder,
		RightShoulder,
		LeftStick,
		RightStick,
		Back,
		BigButton,
		Start
	}

	public class VirtualButton
	{
		public List<Keys> Keys = new List<Keys>();
		public List<MouseButton> MouseButtons = new List<MouseButton>();
		public List<GamePadButton> GamePadButtons = new List<GamePadButton>();

		public bool IsDown() => InputManager.IsDown(this);
		public bool IsPressed() => InputManager.IsPressed(this);
		public bool IsReleased() => InputManager.IsReleased(this);
	}

	public static class InputManager
	{
		private static KeyboardState prevKeyboardState;
		private static KeyboardState keyboardState;

		private static MouseState prevMouseState;
		private static MouseState mouseState;

		private static GamePadState[] prevGamePadState;
		private static GamePadState[] gamePadState;

		public const int TabSize = 4;

		static InputManager()
		{
			prevGamePadState = new GamePadState[4];
			gamePadState = new GamePadState[4];
		}

		public static void Update()
		{
			prevKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();

			prevMouseState = mouseState;
			mouseState = Mouse.GetState();

			prevGamePadState[0] = gamePadState[0];
			prevGamePadState[1] = gamePadState[1];
			prevGamePadState[2] = gamePadState[2];
			prevGamePadState[3] = gamePadState[3];

			gamePadState[0] = GamePad.GetState(PlayerIndex.One);
			gamePadState[1] = GamePad.GetState(PlayerIndex.Two);
			gamePadState[2] = GamePad.GetState(PlayerIndex.Three);
			gamePadState[3] = GamePad.GetState(PlayerIndex.Four);
		}

		public static Vector2 MouseScreenPosition()
		{
			return mouseState.Position.ToVector2();
		}

		public static Vector2 MouseWorldPosition(Matrix transform)
		{
			Vector2 mousePos = MouseScreenPosition();
			Matrix inverse = Matrix.Invert(transform);
			return Vector2.Transform(mousePos, inverse);
		}

		public static Vector2 MouseWorldPosition(Camera camera)
		{
			return MouseWorldPosition(camera.GetViewMatrix());
		}

		public static Vector2 MouseWorldPosition()
		{
			return MouseWorldPosition(Game1.MainCamera);
		}

		public static Rectangle MouseScreenRectangle()
		{
			var pos = MouseScreenPosition().ToPoint();
			return new Rectangle(pos.X, pos.Y, 1, 1);
		}

		public static Rectangle MouseWorldRectangle(Matrix transform)
		{
			var pos = MouseWorldPosition(transform).ToPoint();
			return new Rectangle(pos.X, pos.Y, 1, 1);
		}

		public static Rectangle MouseWorldRectangle(Camera camera)
		{
			var pos = MouseWorldPosition(camera.GetViewMatrix()).ToPoint();
			return new Rectangle(pos.X, pos.Y, 1, 1);
		}

		public static int MouseScroll()
		{
			return mouseState.ScrollWheelValue / 120;
		}

		public static int MouseScrollDelta()
		{
			return (mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue) / 120;
		}

		public static bool IsDown(VirtualButton button, PlayerIndex index = PlayerIndex.One)
		{
			foreach (var key in button.Keys)
			{
				if (IsDown(key))
					return true;
			}

			foreach (var mb in button.MouseButtons)
			{
				if (IsDown(mb))
					return true;
			}

			foreach (var btn in button.GamePadButtons)
			{
				if (IsDown(btn, index))
					return true;
			}

			return false;
		}

		public static bool IsPressed(VirtualButton button, PlayerIndex index = PlayerIndex.One)
		{
			foreach (var key in button.Keys)
			{
				if (IsPressed(key))
					return true;
			}

			foreach (var mb in button.MouseButtons)
			{
				if (IsPressed(mb))
					return true;
			}

			foreach (var btn in button.GamePadButtons)
			{
				if (IsPressed(btn, index))
					return true;
			}

			return false;
		}

		public static bool IsReleased(VirtualButton button, PlayerIndex index = PlayerIndex.One)
		{
			foreach (var key in button.Keys)
			{
				if (IsReleased(key))
					return true;
			}

			foreach (var mb in button.MouseButtons)
			{
				if (IsReleased(mb))
					return true;
			}

			foreach (var btn in button.GamePadButtons)
			{
				if (IsReleased(btn, index))
					return true;
			}

			return false;
		}

		public static bool IsDown(MouseButton button)
		{
			if (button == MouseButton.Left)
			{
				return mouseState.LeftButton == ButtonState.Pressed;
			}
			else if (button == MouseButton.Middle)
			{
				return mouseState.MiddleButton == ButtonState.Pressed;
			}
			else if (button == MouseButton.Right)
			{
				return mouseState.RightButton == ButtonState.Pressed;
			}

			return false;
		}

		public static bool IsPressed(MouseButton button)
		{
			if (button == MouseButton.Left)
			{
				return (
					mouseState.LeftButton == ButtonState.Pressed &&
					prevMouseState.LeftButton == ButtonState.Released
				);
			}
			else if (button == MouseButton.Middle)
			{
				return (
					mouseState.MiddleButton == ButtonState.Pressed &&
					prevMouseState.MiddleButton == ButtonState.Released
				);
			}
			else if (button == MouseButton.Right)
			{
				return (
					mouseState.RightButton == ButtonState.Pressed &&
					prevMouseState.RightButton == ButtonState.Released
				);
			}

			return false;
		}

		public static bool IsReleased(MouseButton button)
		{
			if (button == MouseButton.Left)
			{
				return (
					mouseState.LeftButton == ButtonState.Released &&
					prevMouseState.LeftButton == ButtonState.Pressed
				);
			}
			else if (button == MouseButton.Middle)
			{
				return (
					mouseState.MiddleButton == ButtonState.Released &&
					prevMouseState.MiddleButton == ButtonState.Pressed
				);
			}
			else if (button == MouseButton.Right)
			{
				return (
					mouseState.RightButton == ButtonState.Released &&
					prevMouseState.RightButton == ButtonState.Pressed
				);
			}

			return false;
		}

		public static bool IsDown(Keys key)
		{
			return keyboardState.IsKeyDown(key);
		}

		public static bool IsPressed(Keys key)
		{
			return (
				keyboardState.IsKeyDown(key) &&
				prevKeyboardState.IsKeyUp(key)
			);
		}

		public static bool IsReleased(Keys key)
		{
			return (
				keyboardState.IsKeyUp(key) &&
				prevKeyboardState.IsKeyDown(key)
			);
		}

		public static bool Ctrl()
		{
			return IsDown(Keys.LeftControl) || InputManager.IsDown(Keys.RightControl);
		}

		public static bool Shift()
		{
			return IsDown(Keys.LeftShift) || InputManager.IsDown(Keys.RightShift);
		}

		public static bool Alt()
		{
			return IsDown(Keys.LeftAlt) || InputManager.IsDown(Keys.RightAlt);
		}

		public static bool IsDown(GamePadButton button, PlayerIndex player = PlayerIndex.One)
		{
			var state = gamePadState[(int)player];

			switch (button)
			{
				case GamePadButton.X:
					return state.Buttons.X == ButtonState.Pressed;

				case GamePadButton.Y:
					return state.Buttons.Y == ButtonState.Pressed;

				case GamePadButton.A:
					return state.Buttons.A == ButtonState.Pressed;

				case GamePadButton.B:
					return state.Buttons.B == ButtonState.Pressed;

				case GamePadButton.DPadUp:
					return state.DPad.Up == ButtonState.Pressed;

				case GamePadButton.DPadDown:
					return state.DPad.Down == ButtonState.Pressed;

				case GamePadButton.DPadLeft:
					return state.DPad.Left == ButtonState.Pressed;

				case GamePadButton.DPadRight:
					return state.DPad.Right == ButtonState.Pressed;

				case GamePadButton.LeftShoulder:
					return state.Buttons.LeftShoulder == ButtonState.Pressed;

				case GamePadButton.RightShoulder:
					return state.Buttons.RightShoulder == ButtonState.Pressed;

				case GamePadButton.LeftStick:
					return state.Buttons.LeftStick == ButtonState.Pressed;

				case GamePadButton.RightStick:
					return state.Buttons.RightStick == ButtonState.Pressed;

				case GamePadButton.Back:
					return state.Buttons.Back == ButtonState.Pressed;

				case GamePadButton.BigButton:
					return state.Buttons.BigButton == ButtonState.Pressed;

				case GamePadButton.Start:
					return state.Buttons.Start == ButtonState.Pressed;
			}

			return false;
		}

		public static bool IsPressed(GamePadButton button, PlayerIndex player = PlayerIndex.One)
		{
			var state = gamePadState[(int)player];
			var prevState = prevGamePadState[(int)player];

			switch (button)
			{
				case GamePadButton.X:
					return (
						state.Buttons.X == ButtonState.Pressed &&
						prevState.Buttons.X == ButtonState.Released
					);

				case GamePadButton.Y:
					return (
						state.Buttons.Y == ButtonState.Pressed &&
						prevState.Buttons.Y == ButtonState.Released
					);

				case GamePadButton.A:
					return (
						state.Buttons.A == ButtonState.Pressed &&
						prevState.Buttons.A == ButtonState.Released
					);

				case GamePadButton.B:
					return (
						state.Buttons.B == ButtonState.Pressed &&
						prevState.Buttons.B == ButtonState.Released
					);

				case GamePadButton.DPadUp:
					return (
						state.DPad.Up == ButtonState.Pressed &&
						prevState.DPad.Up == ButtonState.Released
					);

				case GamePadButton.DPadDown:
					return (
						state.DPad.Down == ButtonState.Pressed &&
						prevState.DPad.Down == ButtonState.Released
					);

				case GamePadButton.DPadLeft:
					return (
						state.DPad.Left == ButtonState.Pressed &&
						prevState.DPad.Left == ButtonState.Released
					);

				case GamePadButton.DPadRight:
					return (
						state.DPad.Right == ButtonState.Pressed &&
						prevState.DPad.Right == ButtonState.Released
					);

				case GamePadButton.LeftShoulder:
					return (
						state.Buttons.LeftShoulder == ButtonState.Pressed &&
						prevState.Buttons.LeftShoulder == ButtonState.Released
					);

				case GamePadButton.RightShoulder:
					return (
						state.Buttons.RightShoulder == ButtonState.Pressed &&
						prevState.Buttons.RightShoulder == ButtonState.Released
					);

				case GamePadButton.LeftStick:
					return (
						state.Buttons.LeftStick == ButtonState.Pressed &&
						prevState.Buttons.LeftStick == ButtonState.Released
					);

				case GamePadButton.RightStick:
					return (
						state.Buttons.RightStick == ButtonState.Pressed &&
						prevState.Buttons.RightStick == ButtonState.Released
					);

				case GamePadButton.Back:
					return (
						state.Buttons.Back == ButtonState.Pressed &&
						prevState.Buttons.Back == ButtonState.Released
					);

				case GamePadButton.BigButton:
					return (
						state.Buttons.BigButton == ButtonState.Pressed &&
						prevState.Buttons.BigButton == ButtonState.Released
					);

				case GamePadButton.Start:
					return (
						state.Buttons.Start == ButtonState.Pressed &&
						prevState.Buttons.Start == ButtonState.Released
					);
			}

			return false;
		}

		public static bool IsReleased(GamePadButton button, PlayerIndex player = PlayerIndex.One)
		{
			var state = gamePadState[(int)player];
			var prevState = prevGamePadState[(int)player];

			switch (button)
			{
				case GamePadButton.X:
					return (
						state.Buttons.X == ButtonState.Released &&
						prevState.Buttons.X == ButtonState.Pressed
					);

				case GamePadButton.Y:
					return (
						state.Buttons.Y == ButtonState.Released &&
						prevState.Buttons.Y == ButtonState.Pressed
					);

				case GamePadButton.A:
					return (
						state.Buttons.A == ButtonState.Released &&
						prevState.Buttons.A == ButtonState.Pressed
					);

				case GamePadButton.B:
					return (
						state.Buttons.B == ButtonState.Released &&
						prevState.Buttons.B == ButtonState.Pressed
					);

				case GamePadButton.DPadUp:
					return (
						state.DPad.Up == ButtonState.Released &&
						prevState.DPad.Up == ButtonState.Pressed
					);

				case GamePadButton.DPadDown:
					return (
						state.DPad.Down == ButtonState.Released &&
						prevState.DPad.Down == ButtonState.Pressed
					);

				case GamePadButton.DPadLeft:
					return (
						state.DPad.Left == ButtonState.Released &&
						prevState.DPad.Left == ButtonState.Pressed
					);

				case GamePadButton.DPadRight:
					return (
						state.DPad.Right == ButtonState.Released &&
						prevState.DPad.Right == ButtonState.Pressed
					);

				case GamePadButton.LeftShoulder:
					return (
						state.Buttons.LeftShoulder == ButtonState.Released &&
						prevState.Buttons.LeftShoulder == ButtonState.Pressed
					);

				case GamePadButton.RightShoulder:
					return (
						state.Buttons.RightShoulder == ButtonState.Released &&
						prevState.Buttons.RightShoulder == ButtonState.Pressed
					);

				case GamePadButton.LeftStick:
					return (
						state.Buttons.LeftStick == ButtonState.Released &&
						prevState.Buttons.LeftStick == ButtonState.Pressed
					);

				case GamePadButton.RightStick:
					return (
						state.Buttons.RightStick == ButtonState.Released &&
						prevState.Buttons.RightStick == ButtonState.Pressed
					);

				case GamePadButton.Back:
					return (
						state.Buttons.Back == ButtonState.Released &&
						prevState.Buttons.Back == ButtonState.Pressed
					);

				case GamePadButton.BigButton:
					return (
						state.Buttons.BigButton == ButtonState.Released &&
						prevState.Buttons.BigButton == ButtonState.Pressed
					);

				case GamePadButton.Start:
					return (
						state.Buttons.Start == ButtonState.Released &&
						prevState.Buttons.Start == ButtonState.Pressed
					);
			}

			return false;
		}

		public static Vector2 ThumbStickLeft(PlayerIndex player = PlayerIndex.One)
		{
			return gamePadState[(int)player].ThumbSticks.Left;
		}

		public static Vector2 ThumbStickRight(PlayerIndex player = PlayerIndex.One)
		{
			return gamePadState[(int)player].ThumbSticks.Right;
		}

		public static float TriggerLeft(PlayerIndex player = PlayerIndex.One)
		{
			return gamePadState[(int)player].Triggers.Left;
		}

		public static float TriggerRight(PlayerIndex player = PlayerIndex.One)
		{
			return gamePadState[(int)player].Triggers.Right;
		}
	}
}
