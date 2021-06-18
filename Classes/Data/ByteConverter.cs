using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace TurkmenBuildProjectServer.Classes
{
    public static class ByteConverter
    {
        public static byte[] ConvertToByteArray(Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] byteArray = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            return byteArray;
        }

        public static Image ConvertToImage(byte[] byteArray)
        {
            MemoryStream memoryStream = new MemoryStream(byteArray);
            Image image = Image.FromStream(memoryStream);
            return image;
        }
    }
}