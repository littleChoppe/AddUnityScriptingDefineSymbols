using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class SpecialDefineUtility
{

    private const string _xialuDefine = "XIAO_LU";

#if XIAO_LU
    [MenuItem("Tools/XIAO LU -> COMMON", false)]
#else
    [MenuItem("Tools/COMMON -> XIAO LU", false)]
#endif

    public static void ToggleSpecialDefine()
    {
        //在 XIAO_LU 宏定义生效时，切换到普通模式下，也就是去掉宏定义
#if XIAO_LU
        Set(_xialuDefine, false);
#else
        //在 XIAO_LU 宏定义不生效时，切换到 XIAO_LU 下，也就是添加宏定义
        Set(_xialuDefine, true);
#endif
    }

    //添加或删除宏定义
    private static void Set(string defineName, bool toAdd)
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("游戏运行时无法切换环境");
            return;
        }

        if (EditorApplication.isCompiling)
        {
            Debug.LogError("游戏编译时无法切换环境");
            return;
        }

       if (HasDefine(defineName) != toAdd)
       {
            string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetCurrentTargetGroup());
            string[] defs = def.Split(';');
           if (toAdd)
           {
               ArrayUtility.Add(ref defs, defineName);
           }
           else
           {
               ArrayUtility.Remove(ref defs, defineName);
           }

           def = string.Join(";", defs);
           PlayerSettings.SetScriptingDefineSymbolsForGroup(GetCurrentTargetGroup(), def);
       }

    }

    //检查是否在目标平台下ScriptingDefineSymbols字段已添加了自定义宏定义
    public static bool HasDefine(string defineName)
    {
        string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetCurrentTargetGroup());
        return ArrayUtility.Contains(def.Split(';'), defineName);
    }

    //获取正在编辑的目标平台
    private static BuildTargetGroup GetCurrentTargetGroup()
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneOSXUniversal:
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneLinuxUniversal:
                return BuildTargetGroup.Standalone;
            case BuildTarget.Android:
                return BuildTargetGroup.Android;
            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;
            case BuildTarget.WebGL:
                return BuildTargetGroup.WebGL;
            default:
                Assert.IsTrue(false, "未处理现在已选定的平台");
                return BuildTargetGroup.Unknown;
        }
    }
}
