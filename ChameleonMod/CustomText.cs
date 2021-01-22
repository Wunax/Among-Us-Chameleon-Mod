using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Hud = PIEFJFEOGOL;
using TextRenderer = AELDHKGBIFD;

namespace ChameleonMod
{
    public class CustomText
    {
        public GameObject gameObject { get; }
        public TextRenderer textRenderer { get;  }

        public CustomText(Hud __instance, float x, float y, string text, Transform parent)
        {
            gameObject = new GameObject();
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = new Vector3(x, y);

            textRenderer = UnityEngine.Object.Instantiate(__instance.TaskText, gameObject.transform);
            textRenderer.Text = text;
        }
    }
}
