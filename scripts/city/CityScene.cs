// Главная сцена города. Генерирует здания-заглушки для тестирования камеры и взаимодействия.
// При клике на здание отображает всплывающую панель с его данными.

using Godot;
using DayByDaySim.UI;

namespace DayByDaySim.City
{
	public partial class CityScene : Node2D
	{
		private const int BuildingCount = 30;
		private const int GridCols = 6;
		private const int BuildingW = 110;
		private const int BuildingH = 70;
		private const int Spacing = 15;

		private BuildingPopup _popup;

		private static readonly string[] Districts = { "Деловой", "Жилой", "Промзона" };

		public override void _Ready()
		{
			_popup = GetNode<BuildingPopup>("CanvasLayer/BuildingPopup");
			GenerateBuildings();
		}

		private void GenerateBuildings()
		{
			var rng = new RandomNumberGenerator();
			rng.Randomize();

			for (int i = 0; i < BuildingCount; i++)
			{
				int col = i % GridCols;
				int row = i / GridCols;

				var building = new Building();
				building.Init(
					label: $"Здание_{i + 1}",
					district: Districts[i % Districts.Length],
					color: new Color(
						rng.RandfRange(0.25f, 0.55f),
						rng.RandfRange(0.25f, 0.55f),
						rng.RandfRange(0.45f, 0.80f)
					),
					position: new Vector2(
						col * (BuildingW + Spacing),
						row * (BuildingH + Spacing)
					)
				);

				building.Clicked += OnBuildingClicked;
				AddChild(building);
			}
		}

		private void OnBuildingClicked(Building building)
		{
			var screenPos = GetViewport().GetMousePosition();
			_popup.ShowAt(building, screenPos);
		}
	}
}
