using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IImageService
    {
        string SaveImage(string imageName,string image);
        byte[] ReadImage(string imagePath);
    }
}
