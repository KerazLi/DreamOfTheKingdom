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
        public Vector3 centerPoint;

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
            cardRotations.Clear();
            cardPositions.Clear();
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
                    var pos = new Vector3(xPos, centerPoint.y, 0f);
                    var rotation = Quaternion.identity;
                    cardPositions.Add(pos);
                    cardRotations.Add(rotation);
                    cardRotations.Add(rotation);
                }
            }
        }
    }
}
