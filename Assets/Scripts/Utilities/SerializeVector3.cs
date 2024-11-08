using UnityEngine;

namespace Utilities
{
    [System.Serializable]
    public class SerializeVector3 
    {
        // 公开的字段，用于存储三维向量的x、y、z坐标
        public float x, y, z;

        /// <summary>
        /// 构造函数，用于将一个现有的三维向量实例化为SerializeVector3对象
        /// </summary>
        /// <param name="v">要序列化的三维向量</param>
        public SerializeVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        /// <summary>
        /// 将存储的坐标转换为三维向量并返回
        /// </summary>
        /// <returns>包含当前坐标值的三维向量</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 将存储的x和y坐标转换为二维整数向量并返回
        /// 此方法主要用于需要整数向量的场景
        /// </summary>
        /// <returns>包含当前x和y坐标值的二维整数向量</returns>
        public Vector2Int ToVector2Int()
        {
            return new Vector2Int((int)x, (int)y);
        }
    }
}
