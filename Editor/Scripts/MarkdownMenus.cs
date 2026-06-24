////////////////////////////////////////////////////////////////////////////////

using System.IO;
using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    public class Menus
    {

        static bool IsMarkdownSelected()
        {
            var path = AssetDatabase.GetAssetPath( Selection.activeObject );
            if( string.IsNullOrEmpty( path ) ) return false;
            var ext = Path.GetExtension( path ).ToLower();
            return ext == ".md" || ext == ".markdown";
        }

        static string GetFilePath( string filename )
        {
            var path = AssetDatabase.GetAssetPath( Selection.activeObject );

            if( string.IsNullOrEmpty( path ) )
            {
                path = "Assets";
            }
            else if( AssetDatabase.IsValidFolder( path ) == false )
            {
                path = Path.GetDirectoryName( path );
            }

            return AssetDatabase.GenerateUniqueAssetPath( path + "/" + filename );
        }


        // create

        [MenuItem( "Assets/Create/Markdown" )]
        static void CreateMarkdown()
        {
            var filepath = GetFilePath( "NewMarkdown.md" );
            var writer   = File.CreateText( filepath );

            var template = EditorGUIUtility.Load( "MarkdownTemplate.md" ) as TextAsset;

            if( template != null )
            {
                writer.Write( template.text );
            }
            else
            {
                writer.Write( "# Markdown\n" );
            }

            writer.Close();

            AssetDatabase.ImportAsset( filepath );

            Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>( filepath );
        }

        // create sub-menu

        [MenuItem( "Assets/Markdown/Create" )]
        static void CreateMarkdownSubmenu()
        {
            CreateMarkdown();
        }

        // open

        [MenuItem( "Assets/Markdown/Open", false, 1000 )]
        static void OpenMarkdown()
        {
            var asset = Selection.activeObject as TextAsset;
            if( asset == null ) return;
            var window = EditorWindow.GetWindow<MarkdownWindow>( "Markdown Viewer" );
            window.LoadMarkdownFile( asset );
            window.Show();
        }

        [MenuItem( "Assets/Markdown/Open", true )]
        static bool OpenMarkdownValidate() => IsMarkdownSelected();


        // edit

        [MenuItem( "Assets/Markdown/Edit", false, 1001 )]
        static void EditMarkdown()
        {
            var path = AssetDatabase.GetAssetPath( Selection.activeObject );
            if( !string.IsNullOrEmpty( path ) )
            {
                EditorUtility.OpenWithDefaultApp( path );
            }
        }

        [MenuItem( "Assets/Markdown/Edit", true )]
        static bool EditMarkdownValidate() => IsMarkdownSelected();
    }
}
