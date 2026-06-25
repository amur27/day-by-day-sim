// HUD с игровыми часами и кнопками управления скоростью времени.
// Располагается в правом верхнем углу экрана через CanvasLayer-родитель.

using Godot;
using DayByDaySim.Gameplay;

namespace DayByDaySim.UI
{
	public partial class TimeHUD : HBoxContainer
	{
		private Label _timeLabel;
		private Label _dateLabel;
		private TimeManager _time;

		public override void _Ready()
		{
			_time = GetNode<TimeManager>("/root/TimeManager");

			AddThemeConstantOverride("separation", 8);

			AddSpeedButton("||", TimeSpeed.Paused);
			AddSpeedButton("x1", TimeSpeed.Normal);
			AddSpeedButton("x2", TimeSpeed.Fast);
			AddSpeedButton("x4", TimeSpeed.VeryFast);

			var sep = new VSeparator();
			AddChild(sep);

			_dateLabel = new Label();
			AddChild(_dateLabel);

			_timeLabel = new Label();
			AddChild(_timeLabel);

			// Прижать к правому верхнему углу
			AnchorLeft   = 1f;
			AnchorRight  = 1f;
			AnchorTop    = 0f;
			AnchorBottom = 0f;
			GrowHorizontal = GrowDirection.Begin;
			OffsetRight  = -8f;
			OffsetTop    = 8f;
		}

		private void AddSpeedButton(string text, TimeSpeed speed)
		{
			var btn = new Button { Text = text, ToggleMode = true };
			btn.Pressed += () =>
			{
				_time.SetSpeed(speed);
				UpdateButtonStates();
			};
			btn.Name = $"Btn_{speed}";
			AddChild(btn);
		}

		private void UpdateButtonStates()
		{
			foreach (var child in GetChildren())
			{
				if (child is Button btn && btn.Name.ToString().StartsWith("Btn_"))
				{
					var speedName = btn.Name.ToString().Replace("Btn_", "");
					btn.ButtonPressed = speedName == _time.Speed.ToString();
				}
			}
		}

		public override void _Process(double delta)
		{
			_timeLabel.Text = _time.GetTimeString();
			_dateLabel.Text = _time.GetDateString();
		}
	}
}
