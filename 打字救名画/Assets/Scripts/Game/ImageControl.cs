using UnityEngine;

public class ImageControl : MonoBehaviour
{
    [Header("参数")]
    public float gravityScale = 0;          //重力缩放
    public float force;                  //向上的力
    [Header("组件")]
    public Rigidbody2D rigi;                //刚体

    void Start()
    {
        rigi.gravityScale = 0;
        Eventcenter.AddEventListener("上升", () =>
        {
            rigi.velocity = new Vector2(0, 0);
            rigi.AddForce(new Vector2(0, force));
        });

        Eventcenter.AddEventListener("开始", () =>
        {
            rigi.gravityScale = gravityScale;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DieLine")
        {
            Eventcenter.EventTrigger("失败");
        }
    }
}
