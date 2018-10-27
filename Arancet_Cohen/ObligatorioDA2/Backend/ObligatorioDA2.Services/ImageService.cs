using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class ImageService : IImageService
    {
        private string imagesPath;

        public ImageService(string aPath)
        {
            imagesPath = aPath;
        }
    }
}
