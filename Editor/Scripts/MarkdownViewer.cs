using Markdig;
using UnityEditor;
using UnityEngine;
using Markdig.Extensions.JiraLinks;

namespace MG.MDV
{
    public class MarkdownViewer
    {
        public static readonly Vector2 Margin = new Vector2( 6.0f, 4.0f );

        private GUISkin         mSkin            = null;
        private string          mText            = string.Empty;
        private string          mCurrentPath     = string.Empty;
        private HandlerImages   mHandlerImages   = new HandlerImages();
        private HandlerNavigate mHandlerNavigate = new HandlerNavigate();

        private Layout          mLayout          = null;
        private bool            mRaw             = false;

        private static History  mHistory         = new History();

        public MarkdownViewer( GUISkin skin, string path, string content )
        {
            mSkin        = skin;
            mCurrentPath = path;
            mText        = content;

            mHistory.OnOpen( mCurrentPath );
            mLayout = ParseDocument();

            mHandlerImages.CurrentPath   = mCurrentPath;

            mHandlerNavigate.CurrentPath = mCurrentPath;
            mHandlerNavigate.History     = mHistory;
            mHandlerNavigate.FindBlock   = ( id ) => mLayout.Find( id );
            mHandlerNavigate.ScrollTo    = ( pos ) => {}; // TODO: mScrollPos.y = pos;
        }


        //------------------------------------------------------------------------------

        public bool Update()
        {
            return mHandlerImages.Update();
        }


        //------------------------------------------------------------------------------

        Layout ParseDocument()
        {
            var context  = new Context( mSkin, mHandlerImages, mHandlerNavigate );
            var builder  = new LayoutBuilder( context );
            var renderer = new RendererMarkdown( builder );

            var pipelineBuilder = new MarkdownPipelineBuilder()
                .UseAutoLinks()
            ;

            if( !string.IsNullOrEmpty( Preferences.JIRA ) )
            {
                pipelineBuilder.UseJiraLinks( new JiraLinkOptions( Preferences.JIRA ) );
            }

            var pipeline = pipelineBuilder.Build();
            pipeline.Setup( renderer );

            var doc = Markdown.Parse( mText, pipeline );
            renderer.Render( doc );

            return builder.GetLayout();
        }


        //------------------------------------------------------------------------------

        public void Draw()
        {
            GUI.skin    = mSkin;
            GUI.enabled = true;

            // clear background

            var rectFullScreen = new Rect( 0.0f, 0.0f, Screen.width, Screen.height );
            GUI.DrawTexture( rectFullScreen, GUI.skin.window.normal.background, ScaleMode.StretchToFill, false );


            // content rect

            var contentWidth = EditorGUIUtility.currentViewWidth - GUI.skin.verticalScrollbar.fixedWidth - 2.0f * Margin.x;


            // draw content

            if( mRaw )
            {
                EditorGUILayout.SelectableLabel( mText, GUI.skin.GetStyle( "raw" ) );
            }
            else
            {
                // reserve the space

                #if !UNITY_2018
                    GUILayout.Space( mLayout.Height );
                #else
                    GUILayout.FlexibleSpace();
                #endif

                DrawMarkdown( contentWidth );
            }

            DrawToolbar( contentWidth );
        }

        //------------------------------------------------------------------------------

        void DrawToolbar( float contentWidth )
        {
            var style  = GUI.skin.button;
            var size   = style.fixedHeight;
            var btn    = new Rect( Margin.x + contentWidth - size, Margin.y, size, size );

            if( GUI.Button( btn, string.Empty, GUI.skin.GetStyle( mRaw ? "btnRaw" : "btnFile" ) ) )
            {
                mRaw = !mRaw;
            }

            if( mRaw == false )
            {
                if( mHistory.CanForward )
                {
                    btn.x -= size;

                    if( GUI.Button( btn, string.Empty, GUI.skin.GetStyle( "btnForward" ) ) )
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>( mHistory.Forward() );
                    }
                }

                if( mHistory.CanBack )
                {
                    btn.x -= size;

                    if( GUI.Button( btn, string.Empty, GUI.skin.GetStyle( "btnBack" ) ) )
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>( mHistory.Back() );
                    }
                }
            }
        }

        //------------------------------------------------------------------------------

        void DrawMarkdown( float width )
        {
            switch( Event.current.type )
            {
                case EventType.Ignore:
                    break;

                case EventType.ContextClick:
                    var menu = new GenericMenu();
                    menu.AddItem( new GUIContent( "View Source" ), false, () => mRaw = !mRaw );
                    menu.ShowAsContext();

                    break;

                case EventType.Layout:
                    mLayout.Arrange( width );
                    break;

                default:
                    mLayout.Draw();
                    break;
            }
        }
    }
}
