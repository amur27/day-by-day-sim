// Нода здания: Area2D с визуальным полигоном и коллайдером для обработки кликов мышью.
// Хранит базовые данные здания и испускает сигнал Clicked при нажатии левой кнопкой мыши.

using Godot;

namespace DayByDaySim.City
{
	public partial class Building : Area2D
	{
		[Signal]
		public delegate void ClickedEventHandler(Building building);

		public string BuildingLabel { get; private set; } = "Здание";
		public string District { get; private set; } = "—";

		private static readonly Vector2[] _shape = new Vector2[]
		{
			new Vector2(0, 0),
			new Vector2(110, 0),
			new Vector2(110, 70),
			new Vector2(0, 70)
		};

		public void Init(string label, string district, Color color, Vector2 position)
		{
			BuildingLabel = label;
			District = district;
			Position = position;
			Name = label;

			AddChild(new Polygon2D { Polygon = _shape, Color = color });
			AddChild(new CollisionPolygon2D { Polygon = _shape });
		}

		public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
		{
			if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left && mb.Pressed)
				EmitSignal(SignalName.Clicked, this);
		}
	}
}
