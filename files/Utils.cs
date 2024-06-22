using System.IO;
using System.Reflection;
using UnityEngine;

namespace CustomLocalColors
{
    internal class Utils
    {
        // Token: 0x0600000B RID: 11 RVA: 0x00002350 File Offset: 0x00000550
        public static Texture2D LoadDLLTexture(string path)
        {
            Texture2D result;
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                byte[] array = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(array, 0, array.Length);
                Texture2D texture2D = new Texture2D(256, 256);
                ImageConversion.LoadImage(texture2D, array);
                result = texture2D;
            }
            return result;
        }

        public static Sprite LoadSprite(string path)
        {
            Texture2D result;
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                byte[] array = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(array, 0, array.Length);
                Texture2D texture2D = new Texture2D(256, 256);
                ImageConversion.LoadImage(texture2D, array);
                result = texture2D;
            }
            return Sprite.Create(result, new Rect(0f, 0f, result.width, result.height), new Vector2(0.5f, 0.5f), 45f);
        }
    }
}
