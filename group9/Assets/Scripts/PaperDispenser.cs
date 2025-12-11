using UnityEngine;
using System.Collections;

public class PaperDispenser : MonoBehaviour
{
    [Header("--- setting ---")]
    public GameObject paperPrefab;
    public Transform spawnPoint;

    // 1. 【修改】把 Texture2D 改成 Sprite
    public Sprite stickerSprite;

    public float ejectForce = 200f;
    public float delayTime = 1.5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Printer Head" && !hasTriggered)
        {
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        hasTriggered = true;
        if (GetComponent<Collider>() != null)
            GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(delayTime);

        SpawnPaper();
    }

    void SpawnPaper()
    {
        if (spawnPoint == null || paperPrefab == null) return;

        GameObject paper = Instantiate(paperPrefab, spawnPoint.position, spawnPoint.rotation);

        Renderer paperRenderer = paper.GetComponent<Renderer>();

        // 关键修改部分开始 -----------------------
        if (stickerSprite != null && paperRenderer != null)
        {
            // 1. 设置大图（整张贴图）
            paperRenderer.material.mainTexture = stickerSprite.texture;

            // 2. 计算这个小 Sprite 在大图里的比例和位置
            // 获取大图的总宽和高
            float texWidth = stickerSprite.texture.width;
            float texHeight = stickerSprite.texture.height;

            // 计算 Tiling (缩放比例)：小图的宽除以大图的宽
            Vector2 newTiling = new Vector2(
                stickerSprite.rect.width / texWidth,
                stickerSprite.rect.height / texHeight
            );

            // 计算 Offset (偏移位置)：小图的起始坐标除以大图宽
            Vector2 newOffset = new Vector2(
                stickerSprite.rect.x / texWidth,
                stickerSprite.rect.y / texHeight
            );

            // 3. 应用给材质球
            paperRenderer.material.mainTextureScale = newTiling;  // 相当于材质面板里的 Tiling
            paperRenderer.material.mainTextureOffset = newOffset; // 相当于材质面板里的 Offset
        }
        // 关键修改部分结束 -----------------------

        Rigidbody rb = paper.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDir = spawnPoint.forward;
            rb.AddForce(forceDir * ejectForce);
            rb.AddTorque(Random.insideUnitSphere * 5f);
        }

        Destroy(gameObject);
    }
}