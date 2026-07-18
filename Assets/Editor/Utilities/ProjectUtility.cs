using System.IO;
using UnityEngine;

namespace Project.Editor.Utilities {
    public static class ProjectUtility {
        public static readonly string project;
        
        static ProjectUtility() {
            project = Path.GetFileName(Path.GetDirectoryName(Application.dataPath));
        }
    }
}