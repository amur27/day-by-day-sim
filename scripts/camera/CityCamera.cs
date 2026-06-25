// Управление камерой города: зум колёсиком к позиции курсора (плавный lerp),
// перемещение правой кнопкой мыши.

using Godot;

namespace DayByDaySim.City
{
    public partial class CityCamera : Camera2D
    {
        private const float ZoomStep  = 0.1f;
        private const float ZoomMin   = 0.3f;
        private const float ZoomMax   = 3.0f;
        private const float ZoomSpeed = 12f; // коэффициент плавности (lerp в _Process)

        private float   _targetZoom;
        private Vector2 _focusWorld;  // мировая точка под курсором в начале зума
        private Vector2 _focusScreen; // её смещение от центра экрана
        private bool    _zooming;

        private bool _panning;

        public override void _Ready()
        {
            _targetZoom = Zoom.X;
        }

        public override void _Process(double delta)
        {
            if (!_zooming) return;

            float newZoom = Mathf.Lerp(Zoom.X, _targetZoom, ZoomSpeed * (float)delta);
            Zoom     = new Vector2(newZoom, newZoom);
            // Удерживаем мировую точку под курсором на месте при каждом шаге анимации
            Position = _focusWorld - _focusScreen / newZoom;

            if (Mathf.Abs(newZoom - _targetZoom) < 0.001f)
            {
                Zoom     = new Vector2(_targetZoom, _targetZoom);
                Position = _focusWorld - _focusScreen / _targetZoom;
                _zooming = false;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mb)
            {
                switch (mb.ButtonIndex)
                {
                    case MouseButton.WheelUp:
                        StartZoom(ZoomStep, mb.Position);
                        break;
                    case MouseButton.WheelDown:
                        StartZoom(-ZoomStep, mb.Position);
                        break;
                    case MouseButton.Right:
                        _panning = mb.Pressed;
                        break;
                }
            }

            if (@event is InputEventMouseMotion motion && _panning)
            {
                var move = motion.Relative / Zoom;
                Position -= move;
                // Сдвигаем фокус вместе с камерой — зум после пана остаётся точным
                if (_zooming) _focusWorld -= move;
            }
        }

        private void StartZoom(float delta, Vector2 mouseScreenPos)
        {
            _targetZoom = Mathf.Clamp(_targetZoom + delta, ZoomMin, ZoomMax);

            // Рассчитываем по текущему состоянию камеры (не по target),
            // чтобы быстрые последовательные скроллы не накапливали ошибку
            Vector2 viewportCenter = GetViewport().GetVisibleRect().Size / 2f;
            _focusScreen = mouseScreenPos - viewportCenter;
            _focusWorld  = Position + _focusScreen / Zoom.X;
            _zooming     = true;
        }
    }
}
