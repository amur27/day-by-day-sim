// Управление камерой города: зум колёсиком мыши, перемещение правой кнопкой мыши.

using Godot;

namespace DayByDaySim.City
{
	public partial class CityCamera : Camera2D
	{
		private float _zoomStep = 0.1f;
		private float _zoomMin = 0.3f;
		private float _zoomMax = 3.0f;

		private bool _panning = false;

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseButton mouseBtn)
			{
				switch (mouseBtn.ButtonIndex)
				{
					case MouseButton.WheelUp:
						ApplyZoom(_zoomStep);
						break;
					case MouseButton.WheelDown:
						ApplyZoom(-_zoomStep);
						break;
					case MouseButton.Right:
						_panning = mouseBtn.Pressed;
						break;
				}
			}

			if (@event is InputEventMouseMotion mouseMotion && _panning)
			{
				Position -= mouseMotion.Relative / Zoom;
			}
		}

		private void ApplyZoom(float delta)
		{
			float newZoom = Mathf.Clamp(Zoom.X + delta, _zoomMin, _zoomMax);
			Zoom = new Vector2(newZoom, newZoom);
		}
	}
}
