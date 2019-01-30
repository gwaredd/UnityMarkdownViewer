////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace MG.MDV
{
    public class Context
    {
        public Context( GUISkin skin, IActions actions )
        {
            mStyleConverter = new StyleConverter( skin );
            Actions = actions;

            Apply( Style.Default );
        }

        StyleConverter  mStyleConverter;
        GUIStyle        mStyleGUI;

        public IActions Actions         { get; private set; }
        public float    LineHeight      { get { return mStyleGUI.lineHeight; } }
        public float    MinWidth        { get { return LineHeight * 2.0f; } }
        public float    IndentSize      { get { return LineHeight * 1.5f; } }

        public void     Reset()                         { Apply( Style.Default ); }
        public GUIStyle Apply( Style style )            { mStyleGUI = mStyleConverter.Apply( style ); return mStyleGUI; }
        public Vector2  CalcSize( GUIContent content )  { return mStyleGUI.CalcSize( content ); }
    }
}
