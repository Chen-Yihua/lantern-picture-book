using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_pen : MonoBehaviour
{
    private Texture tex; //���
    public RenderTexture cacheTex; //�w�s�W�@������
    RenderTexture currentTex; //��e�ާ@����
    private float coloredArea = 0.0f; // �O���wø�s���C�⭱�n

    public float brushSize = 0.01f;
    public Color brushCol = Color.red;

    private Material effectMat; //�ΨӳB�z�Ϥ�������
    private Material renderMat; //��l�Ϥ�������

    public Transform penHead;
    public Transform board;
    public GameObject SuccessPenel;

    private Vector2 lastuv;
    private bool isDown;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("Start_game") && !GameObject.Find("Success"))
        {
            int touchCount = Input.touchCount;
            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = Input.GetTouch(i); // ���]�u�B�z�Ĥ@�Ӥ����Ĳ�I
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, LayerMask.GetMask("Board")))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        isDown = true;
                        lastuv = raycastHit.textureCoord2;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        RenderBrushToBoard(raycastHit);
                        lastuv = raycastHit.textureCoord2;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        isDown = false;
                    }
                }
            }
        }

        if (coloredArea >= 0.48f)
        {
            SuccessPenel.SetActive(true);
        }
    }
    private float CountBlackPixels(Texture2D texture)
    {
        float blackPixelCount = 0;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);

                if (pixelColor.r < 0.80f && pixelColor.g > 0.80f && pixelColor.b > 0.80f)
                {
                    blackPixelCount++;
                }
            }
        }
        Debug.Log(blackPixelCount);
        return blackPixelCount;
    }

    private void RenderBrushToBoard(RaycastHit hit)
    {
        Vector2 dir = hit.textureCoord2 - lastuv;

        if (Vector3.SqrMagnitude(dir) > brushSize*brushSize)
        {
            int length = Mathf.CeilToInt(dir.magnitude / brushSize);
            for (int i = 0; i<length; i++)
            {     
                RenderToMatTex(lastuv + dir.normalized * i * brushSize);
            }       
        }
        RenderToMatTex(hit.textureCoord2);     
    }

    private void RenderToMatTex(Vector2 uv)
    {
        float pixelArea = brushSize * brushSize; // ���]�C�ӹ������n�������b�|������
        coloredArea += pixelArea; // �W�[�wø�s���C�⭱�n

        effectMat.SetVector("_BrushPos", new Vector4(uv.x, uv.y, lastuv.x, lastuv.y));
        effectMat.SetColor("_BrushColor", brushCol);
        effectMat.SetFloat("_BrushSize", brushSize);
        Graphics.Blit(cacheTex, currentTex, effectMat);
        renderMat.SetTexture("_MainTex", currentTex);
        Graphics.Blit(currentTex, cacheTex);
    }

    private void Initialized()
    {
        effectMat = new Material(Shader.Find("Brush/markerPen"));
        Material boardMat = board.GetComponent<MeshRenderer>().material;
        tex = boardMat.mainTexture;

        renderMat = boardMat;

        cacheTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(tex, cacheTex);
        renderMat.SetTexture("_MainTex",cacheTex);

        currentTex = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
    }
}
