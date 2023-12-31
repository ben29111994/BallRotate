﻿#if GAIA_PRESENT && UNITY_EDITOR
using UnityEngine;
using GPUInstancer;
using UnityEditor;
using System.Collections.Generic;

namespace Gaia.GX.GurBuTechnologies
{
    /// <summary>
    /// Automatically exported GAIA extensions. This file contains support for Resources and Spawners.
    /// Methods exposed by Gaia as buttons must be prefixed with GX_. General format is GX_SectionName_ButtonName.
    /// Leaving section name out will place the button at the top level eg GX_ButtonName.
    /// </summary>
    public class GPUInstancerGaiaExtension : MonoBehaviour
    {
        #region Generic informational methods

        public static void GX_About()
        {
            EditorUtility.DisplayDialog("About GPU Instancer", "GPU Instancer is an out of the box solution to display extreme numbers of objects on screen with high performance.", "OK");
        }

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return GPUInstancerEditorConstants.PUBLISHER_NAME;
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return GPUInstancerEditorConstants.PACKAGE_NAME;
        }

        #endregion

        #region GX Buttons

        public static void GX_ImportToPrefabManager()
        {
            List<GameObject> prefabList = new List<GameObject>();

            Spawner[] gaiaSpawners = FindObjectsOfType<Spawner>();
            foreach (Spawner sw in gaiaSpawners)
            {
                foreach (SpawnRule sr in sw.m_spawnerRules)
                {
                    if (sr.m_resourceType == GaiaConstants.SpawnerResourceType.GameObject)
                    {
                        foreach (ResourceProtoGameObjectInstance ri in sw.m_resources.m_gameObjectPrototypes[sr.m_resourceIdx].m_instances)
                        {
                            if (ri.m_desktopPrefab != null && !prefabList.Contains(ri.m_desktopPrefab))
                                prefabList.Add(ri.m_desktopPrefab);
                        }
                    }
                }
            }

            GPUInstancerPrefabImporterWindow.ShowWindow(prefabList, "GPU Instancer Gaia Prefab Importer");
        }

        public static void GX_ImportToTreeManager()
        {
            GPUInstancerEditorConstants.ToolbarAddTreeManager();
        }

        public static void GX_ImportToDetailManager()
        {
            GPUInstancerEditorConstants.ToolbarAddDetailManager();
        }

        #endregion GX Buttons
    }
}
#endif