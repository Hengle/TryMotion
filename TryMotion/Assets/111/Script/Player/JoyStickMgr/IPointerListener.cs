/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-25 17:03:51
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IPointerListener : MonoBehaviour,IPointerDownHandler
{
    public delegate void OnTouchDown(PointerEventData eventData);

    public void OnPointerDown(PointerEventData eventData)
    {
        //OnTouchDown(eventData);
    }
}
