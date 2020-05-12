using System;
using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Generators
{
    public class MapSizeGenerator
    {
        public int Width;
        public int Height;
        
        public MapSizeGenerator(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public MapController NextController()
        {
            Width = Math.Min(89, Height + 2);
            Height = Math.Min(91, Width + 2);
            return new MapController(Width, Height);
        }
    }
}