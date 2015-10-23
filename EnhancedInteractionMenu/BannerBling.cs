using System.Drawing;
using GTA;
using GTA.Native;
using NativeUI;

namespace EnhancedInteractionMenu
{
    public class BannerBling
    {
        private Scaleform _scaleform;

        public BannerBling()
        {
            _scaleform = new Scaleform(0);
            _scaleform.Load("MP_MENU_GLARE");
        }

        public void Draw()
        {
            var safe = UIMenu.GetSafezoneBounds();
            var res = UIMenu.GetScreenResolutionMantainRatio();
            _scaleform.CallFunction("SET_DATA_SLOT", 0.0f );
            _scaleform.Render2D();
            return;
            //_scaleform.Render2DScreenSpace(new PointF(safe.X/1920f, safe.Y/1080f), new PointF(431/1920f, 107/1080f));

            int screenw = Game.ScreenResolution.Width;
            int screenh = Game.ScreenResolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;

            float w = (431 / width);
            float h = (107 / height);
            float x = (safe.X/ width) + w * 0.5f;
            float y = (safe.Y / height) + h * 0.5f;

            UI.Notify(w + " " + h + " " + x + " " + y);
            Function.Call(Hash.DRAW_SCALEFORM_MOVIE, _scaleform.Handle, x, y, w, h, 255, 255, 255, 255);
        }
    }
}