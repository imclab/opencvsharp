﻿/*
 * (C) 2008-2014 shimat
 * This code is licenced under the LGPL.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenCvSharp.CPlusPlus
{
#if LANG_JP
    /// <summary>
    /// ORB 実装
    /// </summary>
#else
    /// <summary>
    /// ORB implementation
    /// </summary>
#endif
    public class ORB : FeatureDetector
    {
        private bool disposed;

        #region Init & Disposal
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nFeatures"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="nLevels"></param>
        /// <param name="edgeThreshold"></param>
        /// <param name="firstLevel"></param>
        /// <param name="wtaK"></param>
        /// <param name="scoreType"></param>
        /// <param name="patchSize"></param>
        public ORB(int nFeatures = 500, float scaleFactor = 1.2f, int nLevels = 8, int edgeThreshold = 31,
            int firstLevel = 0, int wtaK = 2, ORBScore scoreType = ORBScore.Harris, int patchSize = 31)
        {
            ptr = CppInvoke.features2d_ORB_new(nFeatures, scaleFactor, nLevels, edgeThreshold,
                firstLevel, wtaK, (int)scoreType, patchSize);
        }

#if LANG_JP
    /// <summary>
    /// リソースの解放
    /// </summary>
    /// <param name="disposing">
    /// trueの場合は、このメソッドがユーザコードから直接が呼ばれたことを示す。マネージ・アンマネージ双方のリソースが解放される。
    /// falseの場合は、このメソッドはランタイムからファイナライザによって呼ばれ、もうほかのオブジェクトから参照されていないことを示す。アンマネージリソースのみ解放される。
    ///</param>
#else
        /// <summary>
        /// Releases the resources
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        /// If false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        /// </param>
#endif
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    // releases managed resources
                    if (disposing)
                    {
                    }
                    // releases unmanaged resources
                    if (ptr != IntPtr.Zero)
                        CppInvoke.features2d_ORB_delete(ptr);
                    ptr = IntPtr.Zero;
                    disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// returns the descriptor size in bytes
        /// </summary>
        /// <returns></returns>
        public int DescriptorSize()
        {
            ThrowIfDisposed();
            return CppInvoke.features2d_ORB_descriptorSize(ptr);
        }

        /// <summary>
        /// returns the descriptor type
        /// </summary>
        /// <returns></returns>
        public int DescriptorType()
        {
            ThrowIfDisposed();
            return CppInvoke.features2d_ORB_descriptorType(ptr);
        }

        /// <summary>
        /// Compute the ORB features on an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public KeyPoint[] Run(InputArray image, InputArray mask = null)
        {
            ThrowIfDisposed();
            if (image == null)
                throw new ArgumentNullException("image");
            image.ThrowIfDisposed();

            using (VectorOfKeyPoint keyPointsVec = new VectorOfKeyPoint())
            {
                CppInvoke.features2d_ORB_run1(ptr, image.CvPtr, Cv2.ToPtr(mask), keyPointsVec.CvPtr);
                return keyPointsVec.ToArray();
            }
        }

        /// <summary>
        /// Compute the ORB features and descriptors on an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <param name="keyPoints"></param>
        /// <param name="descriptors"></param>
        /// <param name="useProvidedKeypoints"></param>
        public void Run(InputArray image, InputArray mask, out KeyPoint[] keyPoints,
            OutputArray descriptors, bool useProvidedKeypoints = false)
        {
            ThrowIfDisposed();
            if (image == null)
                throw new ArgumentNullException("image");
            if (descriptors == null)
                throw new ArgumentNullException("descriptors");
            image.ThrowIfDisposed();
            descriptors.ThrowIfNotReady();

            using (VectorOfKeyPoint keyPointsVec = new VectorOfKeyPoint())
            {
                CppInvoke.features2d_ORB_run2(ptr, image.CvPtr, Cv2.ToPtr(mask), keyPointsVec.CvPtr,
                    descriptors.CvPtr, useProvidedKeypoints ? 1 : 0);
                keyPoints = keyPointsVec.ToArray();
            }
            descriptors.Fix();
        }
        /// <summary>
        /// Compute the ORB features and descriptors on an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <param name="keyPoints"></param>
        /// <param name="descriptors"></param>
        /// <param name="useProvidedKeypoints"></param>
        public void Run(InputArray image, InputArray mask, out KeyPoint[] keyPoints,
            out float[] descriptors, bool useProvidedKeypoints = false)
        {
            MatOfFloat descriptorsMat = new MatOfFloat();
            Run(image, mask, out keyPoints, descriptorsMat, useProvidedKeypoints);
            descriptors = descriptorsMat.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public AlgorithmInfo Info
        {
            get
            {
                ThrowIfDisposed();
                IntPtr pInfo = CppInvoke.features2d_ORB_info(ptr);
                return new AlgorithmInfo(pInfo);
            }
        }
        #endregion
    }
}