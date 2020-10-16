using System;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using Microsoft.VisualBasic;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = @"C:\Users\MyUser\Pictures\test.bmp"; //Insert path to file here.
            var fs = new FileStream(path, FileMode.Open);
            var fileSize = fs.Length;
            var data = new byte[fileSize];
            fs.Read(data, 0, 24);
            var binArray = new string[data.Length];

            for (int i = 0; i < binArray.Length; i++)
            {
                //Console.Write(data[i].ToString("X2") + " ");
                binArray[i] = data[i].ToString("X2");
            }

            if (CheckPng(binArray))
            {
                string[] hexWidth = { binArray[16], binArray[17], binArray[18], binArray[19] };
                string[] hexHeight = { binArray[20], binArray[21], binArray[22], binArray[23] };
                int decWidth = CalculatePngSize(hexWidth);
                int decHeight = CalculatePngSize(hexHeight);
                Console.WriteLine($"It is a .png file with the size: {decWidth}x{decHeight} pixels");
            }

            else if (CheckBmp(binArray))
            {
                string[] hexWidth = { binArray[18], binArray[19], binArray[20], binArray[21] };
                string[] hexHeight = { binArray[22], binArray[23], binArray[24], binArray[25] };
                int decWidth = CalculateBmpSize(hexWidth);
                int decHeight = CalculateBmpSize(hexHeight);
                Console.WriteLine($"It is a .bmp file with the size: {decWidth}x{decHeight} pixels");
            }

            else Console.WriteLine("Invalid file format! Check if file has .bmp or .png extension!");
        }

        public static bool CheckPng(string[] binArray)
        {
            string[] pngArray = { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            string[] dataArray = { binArray[0], binArray[1], binArray[2], binArray[3], binArray[4], binArray[5], binArray[6], binArray[7] };
            return dataArray.SequenceEqual(pngArray);
        }
        public static bool CheckBmp(string[] binArray)
        {
            string[] bmpArray = { "42", "4D", };
            string[] dataArray = { binArray[0], binArray[1] };
            return dataArray.SequenceEqual(bmpArray);
        }

        public static int CalculatePngSize(string[] size)
        {
            string hex = size[0] + size[1] + size[02] + size[03];
            return Convert.ToInt32(hex, 16);
        }

        public static int CalculateBmpSize(string[] size)
        {
            return DecToHex(size[0]) + DecToHex(size[1]) * 256 + (int) Math.Pow(DecToHex(size[2]), 2) + (int)Math.Pow(DecToHex(size[3]), 3);
        }

        public static int DecToHex(string num)
        {
            return Convert.ToInt32(num, 16);
        }

    }
}
