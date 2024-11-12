using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class CardLayoutManager : MonoBehaviour
    {
        // 表示卡片是否水平布局的布尔变量
        public bool isHorizonal;
        // 卡片的最大宽度限制
        public float cardMaxWidth=7f;
        // 卡片之间的间距
        public float cardSpacing = 2f;
        // 用于存储卡片位置的列表，方便序列化以便在Unity编辑器中查看
        [SerializeField]private List<Vector3> cardPositions = new List<Vector3>();
        // 用于存储卡片旋转的列表
        private List<Quaternion> cardRotations = new List<Quaternion>();
        [Header("弧形参数")]
        // 两张卡片之间的角度
        public float angleBetweenCards=7f;
        // 卡片排列的半径
        public float radius=17f;
        // 卡片排列的中心点
        public Vector3 centerPoint;

        private void Awake()
        {
            centerPoint=isHorizonal?Vector3.up*-4.5f:Vector3.up*-21.5f;
        }

        public CardTransform GetCardTransform(int index, int totalCards)
        {
            CalculateCardPositions(totalCards,isHorizonal);
            return new CardTransform(cardPositions[index], cardRotations[index]);
        }

        /// <summary>
        /// 根据卡片数量和布局方向计算每个卡片的位置。
        /// </summary>
        /// <param name="numberOfCards">要布局的卡片数量。</param>
        /// <param name="horizontal">是否为水平布局。</param>
        private void CalculateCardPositions(int numberOfCards,bool horizontal)
        {
            // 清空卡片旋转列表，准备重新计算布局
            cardRotations.Clear();
            // 清空卡片位置列表，准备重新计算布局
            cardPositions.Clear();
            
            // 判断布局方向是否为水平
            if (horizontal)
            {
                // 计算当前所有卡片占据的总宽度（不考虑最大宽度限制）
                float currentWidth = cardSpacing * (numberOfCards-1);
                // 计算实际使用的总宽度，考虑最大宽度限制
                float totalWidth=Mathf.Min(currentWidth,cardMaxWidth);
                // 计算实际使用的卡片间距
                float currentSpacing= totalWidth>0?totalWidth/(numberOfCards-1):0;
                
                // 遍历每个卡片，计算其在水平布局下的x坐标位置
                for (int i = 0; i < numberOfCards; i++)
                {
                    // 计算当前位置的x坐标，确保卡片围绕中心点对称布局
                    float xPos=0-(totalWidth/2)+(i*currentSpacing);
                    // 创建卡片位置，y坐标使用中心点的y坐标，z坐标设置为0
                    var pos = new Vector3(xPos, centerPoint.y, 0f);
                    // 创建卡片旋转，水平布局下卡片无旋转
                    var rotation = Quaternion.identity;
                    
                    // 将计算出的位置和旋转添加到列表中
                    cardPositions.Add(pos);
                    cardRotations.Add(rotation);
                }
            }
            else
            {
                // 计算垂直布局时卡片的最大旋转角度
                float cardAngle = (numberOfCards - 1) * angleBetweenCards /2;
                
                // 遍历每个卡片，计算其在垂直布局下的位置和旋转
                for (int i = 0; i < numberOfCards; i++)
                {
                    // 计算当前卡片的位置
                    var pos = FanCardPosition(cardAngle - i * angleBetweenCards);
                    // 计算当前卡片的旋转
                    var rotation = Quaternion.Euler(0, 0, -angleBetweenCards * i);
                    
                    // 将计算出的位置和旋转添加到列表中
                    cardPositions.Add(pos);
                    cardRotations.Add(rotation);
                }
            }
        }

        /// <summary>
        /// 根据角度计算扇形卡片的位置。
        /// </summary>
        /// <param name="angle">扇形卡片相对于中心点的角度。</param>
        /// <returns>返回扇形卡片的位置。</returns>
        private Vector3 FanCardPosition(float angle)
        {
            // 计算扇形卡片的x坐标，使用正弦函数根据角度和半径计算偏移量
            // 计算扇形卡片的y坐标，使用余弦函数根据角度和半径计算偏移量
            // 扇形卡片的z坐标设置为0，保持在同一个平面上
            return new Vector3(centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
                                centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                                0
                                );
        }
    }
}
