////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class Context
    {
        public Context( GUISkin skin, IActions actions )
        {
            StyleLayout = new StyleCache( skin );
            Actions     = actions;

            Apply( Style.Default );
        }

        public StyleCache   StyleLayout;
        public GUIStyle     StyleGUI;
        public IActions     Actions;

        public float LineHeight { get { return StyleGUI.lineHeight; } }
        public float MinWidth   { get { return LineHeight * 2.0f; } }
        public float IndentSize { get { return LineHeight * 2.0f; } }

        public void     Reset()                         { Apply( Style.Default ); }
        public GUIStyle Apply( Style style )            { StyleGUI = StyleLayout.Apply( style ); return StyleGUI; }
        public Vector2  CalcSize( GUIContent content )  { return StyleGUI.CalcSize( content ); }
    }
}
