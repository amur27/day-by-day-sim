// Singleton (Autoload) игровых часов.
// Накапливает реальное время, переводит в игровые минуты с учётом множителя скорости.
// Испускает сигналы HourPassed и DayPassed для подписки других систем.

using Godot;

namespace DayByDaySim.Gameplay
{
	public enum TimeSpeed { Paused, Normal, Fast, VeryFast }

	public partial class TimeManager : Node
	{
		[Signal] public delegate void HourPassedEventHandler(int hour);
		[Signal] public delegate void DayPassedEventHandler(int day, int month, int year);

		public int Year { get; private set; } = 2025;
		public int Month { get; private set; } = 1;
		public int Day { get; private set; } = 1;
		public int Hour { get; private set; } = 8;
		public int Minute { get; private set; } = 0;
		public TimeSpeed Speed { get; private set; } = TimeSpeed.Normal;

		// 1 реальная секунда = 1 игровая минута при Normal
		private const float SecondsPerGameMinute = 1.0f;
		private float _accumulated = 0f;

		private static readonly int[] DaysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

		public override void _Process(double delta)
		{
			if (Speed == TimeSpeed.Paused) return;

			float multiplier = Speed switch
			{
				TimeSpeed.Normal   => 1f,
				TimeSpeed.Fast     => 2f,
				TimeSpeed.VeryFast => 4f,
				_                  => 0f
			};

			_accumulated += (float)delta * multiplier;

			while (_accumulated >= SecondsPerGameMinute)
			{
				_accumulated -= SecondsPerGameMinute;
				AdvanceMinute();
			}
		}

		private void AdvanceMinute()
		{
			Minute++;
			if (Minute < 60) return;
			Minute = 0;
			Hour++;
			EmitSignal(SignalName.HourPassed, Hour);

			if (Hour < 24) return;
			Hour = 0;
			Day++;

			if (Day <= DaysInMonth[Month])
			{
				EmitSignal(SignalName.DayPassed, Day, Month, Year);
				return;
			}

			Day = 1;
			Month++;

			if (Month <= 12)
			{
				EmitSignal(SignalName.DayPassed, Day, Month, Year);
				return;
			}

			Month = 1;
			Year++;
			EmitSignal(SignalName.DayPassed, Day, Month, Year);
		}

		public void SetSpeed(TimeSpeed speed) => Speed = speed;

		public string GetTimeString() => $"{Hour:D2}:{Minute:D2}";
		public string GetDateString() => $"{Day:D2}.{Month:D2}.{Year}";
	}
}
