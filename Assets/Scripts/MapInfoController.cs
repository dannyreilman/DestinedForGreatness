using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapInfoController : MonoBehaviour {

    public Image mapImage;
    public Canvas[] playerCards;

    public void SetMapInfo(MapInfo passedInfo)
    {
        mapImage.sprite = passedInfo.MapImage;
        ((RectTransform)playerCards[0].transform).anchoredPosition = new Vector3(passedInfo.friendlyFront.x, passedInfo.friendlyFront.y, .25f);
        playerCards[1].transform.position = new Vector3(passedInfo.friendlyTopWing.x, passedInfo.friendlyTopWing.y, .25f);
        playerCards[2].transform.position = new Vector3(passedInfo.friendlyBottomWing.x, passedInfo.friendlyBottomWing.y, .25f);
        playerCards[3].transform.position = new Vector3(passedInfo.friendlyBack.x, passedInfo.friendlyBack.y, .25f);
    }
}
