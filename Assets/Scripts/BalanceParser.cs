using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace Swarm
{
    public class BalanceParser : MonoBehaviour
    {
#if UNITY_EDITOR
        // Start is called before the first frame update
        void Start()
        {

        }

        [MenuItem("Swarm/Balance/Parse Local")]
        public static void ParseLocal()
        {
            Debug.Log("Parse balance started!");
            AssignIDs();
            byte[] array = Parse();
            // save array
            string path = "Assets/Resources/swarm_balance.bytes";
            using (FileStream fs = File.Create(path))
            using (BinaryWriter bw = new BinaryWriter(fs))
                bw.Write(array);

            Debug.Log("Parse balance finished!");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void AssignIDs()
        {
            List<Object> objects = new List<Object>();
            objects.Clear();
            AddObjectsFromDirectory("Assets/Data/Enemy", objects, typeof(EnemySO));
            int numObjects = objects.Count;
            for (int i = 0; i < numObjects; i++)
            {
                EnemySO enemy = (EnemySO)objects[i];
                enemy.ID = i;
                EditorUtility.SetDirty(enemy);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static byte[] Parse()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    int version = 1;
                    bw.Write(version);

                    List<Object> objects = new List<Object>();

                    GameBalanceSO gameBalanceSO = (GameBalanceSO)AssetDatabase.LoadAssetAtPath("Assets/Data/GameBalance.asset", typeof(GameBalanceSO));
                    bw.Write(gameBalanceSO.PlayerSpeed);
                    bw.Write(gameBalanceSO.MaxEnemies);

                    objects.Clear();
                    AddObjectsFromDirectory("Assets/Data/Enemy", objects, typeof(EnemySO));
                    int numEnemies = objects.Count;
                    bw.Write(numEnemies);

                    for (int i = 0; i < numEnemies; i++)
                    {
                        EnemySO enemy = (EnemySO)objects[i];

                        bw.Write(enemy.Prefab.name);
                        bw.Write(enemy.HP);
                        bw.Write(enemy.MinSpeed);
                        bw.Write(enemy.MaxSpeed);
                        bw.Write(enemy.MinRotation);
                        bw.Write(enemy.MaxRotation);
                    }
                }
                return stream.ToArray();
            }

        }

        public static void AddObjectsFromDirectory(string path, List<Object> items, System.Type type)
        {
            if (Directory.Exists(path))
            {
                string[] assets = Directory.GetFiles(path);
                foreach (string assetPath in assets)
                    if (assetPath.Contains(".asset") && !assetPath.Contains(".meta"))
                        items.Add(AssetDatabase.LoadAssetAtPath(assetPath, type));

                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                    if (Directory.Exists(directory))
                        AddObjectsFromDirectory(directory, items, type);
            }
        }
#endif
    }
}