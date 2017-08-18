using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ShapeReplica.Editor
{
    public class OpenMainSceneEditorMenuItem : MonoBehaviour
    {
        [MenuItem("Assets/Open Main Scene %m", false, 0)]
        private static void OpenMainScene()
        {
            var mainSceneLabel = "MainScene";
            var mainSceneGUID = AssetDatabase.FindAssets(string.Format("l:{0}", mainSceneLabel)).First();
            var assetPath = AssetDatabase.GUIDToAssetPath(mainSceneGUID);
            EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Single);
        }
    }
}