// Всплывающая панель с информацией о здании.
// Создаётся кодом; позиционируется в экранных координатах через CanvasLayer-родитель.

using Godot;
using DayByDaySim.City;
using DayByDaySim.Core;

namespace DayByDaySim.UI
{
	public partial class BuildingPopup : PanelContainer
	{
		private Label _nameLabel;
		private Label _districtLabel;

		public override void _Ready()
		{
			CustomMinimumSize = new Vector2(220, 0);
			Visible = false;

			var vbox = new VBoxContainer();
			vbox.AddThemeConstantOverride("separation", 6);
			AddChild(vbox);

			_nameLabel = new Label { AutowrapMode = TextServer.AutowrapMode.Off };
			vbox.AddChild(_nameLabel);

			_districtLabel = new Label();
			vbox.AddChild(_districtLabel);

			var closeBtn = new Button { Text = L10n.T("ui.close") };
			closeBtn.Pressed += () => Visible = false;
			vbox.AddChild(closeBtn);
		}

		public void ShowAt(Building building, Vector2 screenPos)
		{
			_nameLabel.Text = building.BuildingLabel;
			_districtLabel.Text = $"{L10n.T("building.district.label")} {building.District}";
			Position = screenPos + new Vector2(12, 12);
			Visible = true;
		}

		public void Hide() => Visible = false;
	}
}
