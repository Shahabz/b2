using UnityEngine;
using System.Collections;
using System;
using InControl;

namespace CustomProfileExample
{

	public class KeyboardController : CustomInputDeviceProfile {
		public KeyboardController () {
			// Use this for initialization
			Name = "Keyboard";
			Meta = "Player Controller for keyboard and Xbox";

			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			LowerDeadZone = 0.0f;
			UpperDeadZone = 1.0f;

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Interact",
					Target = InputControlType.Action1,
					Source = KeyCodeButton(KeyCode.E)
				}
			};

			AnalogMappings = new[]
			{
				new InputControlMapping {
					Handle = "Rotate Left",
					Target = InputControlType.LeftStickLeft,
					Source = KeyCodeButton(KeyCode.A)
				},

				new InputControlMapping {
					Handle = "Rotate Right",
					Target = InputControlType.LeftStickRight,
					Source = KeyCodeButton(KeyCode.D)
				},

				new InputControlMapping {
					Handle = "Move Forward",
					Target = InputControlType.LeftStickUp,
					Source = KeyCodeButton(KeyCode.W)
				},

				new InputControlMapping {
					Handle = "Move Backward",
					Target = InputControlType.LeftStickDown,
					Source = KeyCodeButton(KeyCode.S)
				},
			};
		}	
	}
}