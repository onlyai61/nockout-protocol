using System.IO;
using Nockout.UIPrototype;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nockout.UIPrototype.Editor
{
    public static class NockoutUIPrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/NockoutUIPrototype/NockoutUIPrototype.unity";

        [MenuItem("NOCKOUT/Create UI Prototype Scene")]
        public static void CreateScene()
        {
            CreateSceneInternal(true);
        }

        public static void CreateSceneBatch()
        {
            CreateSceneInternal(false);
        }

        private static void CreateSceneInternal(bool showDialog)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "NockoutUIPrototype";

            var root = new GameObject("NOCKOUT UI Prototype");
            root.AddComponent<NockoutUIPrototypeController>();

            Directory.CreateDirectory("Assets/NockoutUIPrototype");
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.Refresh();
            if (showDialog)
            {
                EditorUtility.DisplayDialog("NOCKOUT", "UI prototype scene created:\n" + ScenePath, "OK");
            }
        }
    }
}
