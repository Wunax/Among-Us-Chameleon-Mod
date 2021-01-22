using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnhollowerBaseLib;

using Hud = PIEFJFEOGOL;
using HudManager = PPAEIPHJPDH<PIEFJFEOGOL>;

namespace ChameleonMod
{
    public class CustomButton
    {
        public GameObject gameObject { get; }
        public Texture2D texture { get; }
        public SpriteRenderer spriteRenderer { get; }
        public CustomText textDuration;
        public DateTime lastUsed;
        public float cooldown;
        public float duration;
        public bool isActive;
        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");

        public static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            var il2cppArray = (Il2CppStructArray<byte>)data;
            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
        }

        public CustomButton(Hud __instance, float cd, float dur, String pathAsset)
        {
            gameObject = new GameObject();
            gameObject.transform.SetParent(__instance.gameObject.transform, false);
            gameObject.transform.localPosition = new Vector3(0, 0);

            cooldown = cd;
            duration = dur;
            Stream myStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathAsset);
            byte[] byteArray = new byte[myStream.Length];
            myStream.Read(byteArray, 0, (int)myStream.Length);
            texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            var il2cppArray = (Il2CppStructArray<byte>)byteArray;
            iCall_LoadImage(texture.Pointer, il2cppArray.Pointer, false);
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            textDuration = new CustomText(__instance, -0.10f, 0.45f, "0", gameObject.transform);
            textDuration.textRenderer.scale = 2;
            textDuration.textRenderer.Centered = true;
            textDuration.gameObject.SetActive(true);

            lastUsed = DateTime.UtcNow.AddSeconds(-duration - cooldown + 15);
        }

        public bool Clicked()
        {
            if (gameObject.activeSelf && Input.GetKeyUp(KeyCode.Mouse0))
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (spriteRenderer.transform.position.x - spriteRenderer.size.x / 2 < mousePos.x &&
                    spriteRenderer.transform.position.x + spriteRenderer.size.x / 2 > mousePos.x &&
                    spriteRenderer.transform.position.y - spriteRenderer.size.y / 2 < mousePos.y &&
                    spriteRenderer.transform.position.y + spriteRenderer.size.y / 2 > mousePos.y)
                    return true;
            }
            return false;
        }

        public float UseTime()
        {
            DateTime now = DateTime.UtcNow;
            var diff = now - lastUsed;
            if (((cooldown * 1000f) + (duration * 1000f) - (float)diff.TotalMilliseconds) < 0)
                return 0;
            return ((cooldown * 1000f) + (duration * 1000f) - (float)diff.TotalMilliseconds) / 1000f;
        }

        public float ActiveTime()
        {
            DateTime now = DateTime.UtcNow;
            var diff = now - lastUsed;
            if (((duration * 1000f) - (float)diff.TotalMilliseconds) < 0)
                return 0;
            return ((duration * 1000f) - (float)diff.TotalMilliseconds) / 1000f;
        }
    }
}
