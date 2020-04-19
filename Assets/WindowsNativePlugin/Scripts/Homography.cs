using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

namespace Tinker
{

    public static class Homography
    {
        [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_create")]
        private static extern IntPtr _Create();

        [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_calc_homography")]
        private static extern IntPtr _CalcHomography(IntPtr instance, IntPtr src, IntPtr dst, int length);

        [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_get_plugin_name")]
        private static extern IntPtr _GetPluginName(IntPtr instance);

        /// <summary>
        /// ホモグラフィ計算プラグインの初期化
        /// </summary>
        public static IntPtr InitHomographyPlugin()
        {
            return _Create();
        }

        /// <summary>
        /// ホモグラフィマトリクスを返す。
        /// </summary>
        /// <param name="src">元の座標系基準</param>
        /// <param name="dst">変換後の座標系</param>
        /// <returns>ホモグラフィマトリクスのリスト。</returns>
        public static List<float> GetHomography(IntPtr instance, List<Vector3> src, List<Vector3> dst)
        {
            if (src.Count != dst.Count || src.Count < 4)
            {
                Debug.LogError("ホモグラフィは４点以上、そしてsrc と dst の長さは同じでないといけない");
                return null;
            }

            Vector3[] srcArray = src.ToArray();
            Vector3[] dstArray = dst.ToArray();

            // Pin array of source points
            GCHandle pinnedArray = GCHandle.Alloc(srcArray, GCHandleType.Pinned);
            IntPtr ptrSrc = pinnedArray.AddrOfPinnedObject();

            // Pin array of destination points
            GCHandle pinnedDst = GCHandle.Alloc(dstArray, GCHandleType.Pinned);
            IntPtr ptrDst = pinnedDst.AddrOfPinnedObject();

            IntPtr resPtr = IntPtr.Zero;
            IntPtr res = _CalcHomography(instance, ptrSrc, ptrDst, srcArray.Length);

            float[] homography = new float[9];
            Marshal.Copy(res, homography, 0, homography.Length);

            pinnedArray.Free();
            pinnedDst.Free();

            return homography.ToList();
        }
    }
}