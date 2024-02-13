using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPositionCon : MonoBehaviour
{

    [SerializeField]
    private GameObject moveObject;
    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);


        // 自身の座標
        var selfPosition = pos;
        var frontPosition = selfPosition + Vector2.right; // 前方 向きに応じてrightを反転
        var yAdjustMaxDistance = 0.2f; // 補正可能な高さ 
        var rayOrigin = frontPosition + (Vector2.up * yAdjustMaxDistance); // Rayの発射位置
                                                                           // 自身の前方の少し上から真下に向けてRayを飛ばす
        var castResult = Physics2D.Raycast(rayOrigin, Vector2.down, distance: yAdjustMaxDistance, 6);
        // 何とも命中しなかった場合
        if (castResult.collider == null)
            return;
        // 命中したcolliderの法線と自身の上方向の内積を取って一定の傾き以下なら登れない or 下を向いた辺と判定
        var normal = Vector2.Dot(Vector2.up, castResult.normal);
        if (normal <= 0.25f)
            return;

        // 自身のy座標から補正するべき高さ
        var yAdjustDistance = castResult.distance - yAdjustMaxDistance;
        moveObject.transform.position = new Vector2(transform.position.x,transform.position.y + yAdjustDistance);
    }

}
