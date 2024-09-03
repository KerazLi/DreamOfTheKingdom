using UnityEditor;
using UnityEngine;

namespace MStudio
{
    [InitializeOnLoad]
    public class StyleHierarchy
    {
        static string[] _dataArray;//Find ColorPalette GUID
        static string _path;//Get ColorPalette(ScriptableObject) path
        static ColorPalette _colorPalette;

        static StyleHierarchy()
        {
            _dataArray = AssetDatabase.FindAssets("t:ColorPalette");

            if (_dataArray.Length >= 1)
            {    //We have only one color palette, so we use dataArray[0] to get the path of the file
                _path = AssetDatabase.GUIDToAssetPath(_dataArray[0]);

                _colorPalette = AssetDatabase.LoadAssetAtPath<ColorPalette>(_path);

                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
            }
        }

        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            //To make sure there is no error on the first time the tool imported in project
            if (_dataArray.Length == 0) return;

            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);

            if (instance != null)
            {
                for (int i = 0; i < _colorPalette.colorDesigns.Count; i++)
                {
                    var design = _colorPalette.colorDesigns[i];

                    //Check if the name of each gameObject is begin with keyChar in colorDesigns list.
                    if (instance.name.StartsWith(design.keyChar))
                    {
                        //Remove the symbol(keyChar) from the name.
                        string newName = instance.name.Substring(design.keyChar.Length);
                        //Draw a rectangle as a background, and set the color.
                        EditorGUI.DrawRect(selectionRect, design.backgroundColor);

                        //Create a new GUIStyle to match the desing in colorDesigns list.
                        GUIStyle newStyle = new GUIStyle
                        {
                            alignment = design.textAlignment,
                            fontStyle = design.fontStyle,
                            normal = new GUIStyleState()
                            {
                                textColor = design.textColor,
                            }
                        };

                        //Draw a label to show the name in upper letters and newStyle.
                        //If you don't like all capital latter, you can remove ".ToUpper()".
                        EditorGUI.LabelField(selectionRect, newName.ToUpper(), newStyle);
                    }
                }
            }
        }
    }
}