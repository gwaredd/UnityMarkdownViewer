////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class Context
    {
        public StyleCache   StyleLayout;
        public GUIStyle     StyleGUI;
        public IActions     Actions;

        public float LineHeight { get { return StyleGUI.lineHeight; } }
        public float MinWidth   { get { return LineHeight * 2.0f; } }
        public float IndentSize { get { return LineHeight * 2.0f; } }

        public GUIStyle Apply( Style style )            { StyleGUI = StyleLayout.Apply( style ); return StyleGUI; }
        public GUIStyle Reset()                         { return Apply( new Style() ); }
        public Vector2  CalcSize( GUIContent content )  { return StyleGUI.CalcSize( content ); }
    }
}
