////////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

namespace MG.MDV
{
    public class Preferences
    {
        private static readonly string KeyJIRA = "MG/MDV/JIRA";

        private static string mJIRA = string.Empty;
        private static bool   mPrefsLoaded = false;

        public static string JIRA
        {
            get
            {
                LoadPrefs();
                return mJIRA;
            }
        }

        private static void LoadPrefs()
        {
            if( !mPrefsLoaded )
            {
                mJIRA = EditorPrefs.GetString( KeyJIRA, "" );
                mPrefsLoaded = true;
            }
        }

        [PreferenceItem( "Markdown" )]
        private static void DrawPreferences()
        {
            LoadPrefs();

            mJIRA = EditorGUILayout.TextField( "JIRA URL", mJIRA );

            if( GUI.changed )
            {
                EditorPrefs.SetString( KeyJIRA, mJIRA );
            }
        }
    }
}
