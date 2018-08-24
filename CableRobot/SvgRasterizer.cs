using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public class SvgRasterizer
    {
        public SvgRasterizer(int gridStrokeWidth, int gridSize)
        {
            _gridSvg = GenerateGridSvg(gridStrokeWidth, gridSize);
        }

        private SvgDocument _gridSvg;

        public static Bitmap Rasterize(SvgDocument doc, int strokeWidth, int gridSize, Point? size = null, bool grid = true)
            => new SvgRasterizer(strokeWidth, gridSize).Rasterize(doc, size, grid);

        public Bitmap Rasterize(SvgDocument doc, Point? size = null, bool grid = true)
        {
            var bm = doc.Draw(size?.X ?? 0, size?.Y ?? 0);
            if (grid)
                using (var b = bm)
                    return RenderGrid(b);
            else
                return bm;
        }

        private SvgDocument GenerateGridSvg(int strokeWidth, int size)
        {
            var gen = new SvgGenerator();

            gen.StrokeColor = "cornflowerblue";

            for (int x = 0; x <= gen.MaxX; x += size)
                gen.DrawLine(new Vector2(x, gen.MinY), new Vector2(x, gen.MaxY));
            for (int x = -size; x >= gen.MinX; x -= size)
                gen.DrawLine(new Vector2(x, gen.MinY), new Vector2(x, gen.MaxY));

            for (int y = 0; y <= gen.MaxY; y += size)
                gen.DrawLine(new Vector2(gen.MinX, y), new Vector2(gen.MaxX, y));
            for (int y = -size; y >= gen.MinY; y -= size)
                gen.DrawLine(new Vector2(gen.MinX, y), new Vector2(gen.MaxX, y));

            return SvgDocument.FromSvg<SvgDocument>(gen.GenerateCode(strokeWidth));
        }

        private Bitmap RenderGrid(Bitmap image)
        {
            var gridBitmap = new Bitmap(image.Width, image.Height);
            var bound = new Rectangle(0, 0, image.Width, image.Height);
            using (var g = Graphics.FromImage(gridBitmap))
            {
                g.FillRectangle(Brushes.White, bound);
                g.DrawImage(_gridSvg.Draw(image.Width, image.Height), bound);
                g.DrawImage(image, bound);
            }
            return gridBitmap;
        }
    }
}
