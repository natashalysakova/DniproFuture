﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace DniproFuture.Models.Extentions
{
    public static class HttpPostedFileBaseExtention
    {

        public static void CropAndSave(this HttpPostedFileBase photo, string path)
        {
            if (photo.ContentType.Contains("image"))
            {

                Bitmap original = new Bitmap(photo.InputStream);
                int newSize = 0;
                int startI = 0, startJ = 0;
                if (original.Width > original.Height)
                {
                    newSize = original.Height;
                    startI = (original.Width - newSize)/2;
                }
                else
                {
                    newSize = original.Width;
                    startJ = (original.Height - newSize)/2;
                }

                int k = 0, l = 0;

                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();

                //Bitmap crop = new Bitmap(newSize, newSize);
                //for (int i = startI; i < startI + newSize; i++)
                //{
                //    l = 0;
                //    for (int j = startJ; j < startJ + newSize; j++)
                //    {
                //        crop.SetPixel(k, l, original.GetPixel(i, j));
                //        l++;
                //    }
                //    k++;
                //}


                Bitmap crop = new Bitmap(newSize, newSize, PixelFormat.Format32bppArgb);
                BitmapData bmd = crop.LockBits(new Rectangle(0, 0, crop.Width, crop.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    crop.PixelFormat);

                const int pixelSize = 4;
                unsafe
                {
                    for (int y = startJ; y < startJ + newSize; y++)
                    {
                        k = 0;
                        byte* row = (byte*) bmd.Scan0 + (l*bmd.Stride);
                        for (int x = startI; x < startI + newSize; x++)
                        {
                            Color old = original.GetPixel(x, y);
                            row[k*pixelSize] = old.B; //Blue  0-255
                            row[k*pixelSize + 1] = old.G; //Green 0-255
                            row[k*pixelSize + 2] = old.R; //Red   0-255
                            row[k*pixelSize + 3] = old.A; //Alpha 0-255
                            k++;
                        }
                        l++;
                    }
                }

                crop.UnlockBits(bmd);

                //stopwatch.Stop();
                //Debug.WriteLine(stopwatch.Elapsed);

                crop.Save(path);
            }
            else
            {
                throw new FormatException("Loaded file is not image");
            }
        }

        public static List<string> GetPhotosList(this HttpPostedFileBase[] photos, string path)
        {
            var photosList = new List<string>();
            foreach (var photo in photos)
            {

                if (photo != null)
                {
                    string[] splited = photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string extention = splited.Last();
                    var filename =
                        Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + "." +
                        extention;

                    var filePath = Path.Combine(path, filename);

                    if (photosList.Count == 0)
                    {
                        photo.CropAndSave(filePath);
                    }
                    else
                    {
                        photo.SaveAs(filePath);
                    }

                    photosList.Add(filename);
                }
            }
            return photosList;
        }

        public static List<string> GetPhotosList(this HttpPostedFileBase[] photos, string path, OldPhotoModel[] oldPhotos)
        {
            List<string> photosList = new List<string>();

            foreach (var photo in photos)
            {
                if (photo != null)
                {
                    string[] splited = photo.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string extention = splited.Last();
                    var filename =
                        Path.GetRandomFileName().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + "." +
                        extention;
                    var filePath = Path.Combine(path, filename);

                    if (photosList.Count == 0 && oldPhotos==null)
                    {
                        photo.CropAndSave(filePath);
                    }
                    else if (photosList.Count == 0 && oldPhotos!=null && (oldPhotos.Count(x => x.IsLeave) == 0 || oldPhotos[0].IsLeave == false))
                    {
                        photo.CropAndSave(filePath);
                    }
                    else
                    {
                        photo.SaveAs(filePath);
                    }

                    photosList.Add(filename);
                }
            }
            return photosList;
        }
    }
}