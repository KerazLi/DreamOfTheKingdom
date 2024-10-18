using System;
using UnityEngine;

public class Line : MonoBehaviour
{
    // 声明一个私有变量offsetSpeed，用于控制纹理偏移的速度
    private float offsetSpeed = 0.1f;

    // 公有变量lineRenderer，用于引用LineRenderer组件
    public LineRenderer lineRenderer;

    // Update函数，每帧调用一次，用于更新游戏逻辑
    private void Update()
    {
        if (lineRenderer is null) return;
        
        // 获取当前LineRenderer材质的主纹理偏移量
        var offset = lineRenderer.material.mainTextureOffset;
        
        // 增加纹理在X轴上的偏移量，乘以时间增量以实现平滑移动
        offset.x += offsetSpeed * Time.deltaTime;
        
        // 设置LineRenderer材质的主纹理偏移量为新的值
        lineRenderer.material.mainTextureOffset = offset;
    }
}