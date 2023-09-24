Shader "Brush/markerPen"
{                                  
    Properties{
        _MainTex("Texture", 2D) = "white"{}
        _BrushPos("BrushPos", Vector) = (0,0,0,0)
        _BrushColor("Brush Color", Color) = (1,1,1,1)
        _BrushSize("Brush Size", float) = 0.01
    }

    SubShader{
    Tags{"RenderType" = "Opaque"}

        pass {
            CGPROGRAM
                #pragma vertex vert;
                #pragma fragment frag;
                #include "UnityCg.cginc"

                struct appdata {
                    float4 vertex:POSITION;
                    float2 uv:TEXCOORD0;
                };

                struct v2f {
                    float2 uv:TEXCOORD0;
                    float4 vertex:SV_POSITION;
                };

                sampler2D _MainTex;

                float4 _BrushPos;
                float4 _BrushColor;
                float _BrushSize;
                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                };

                fixed4 frag(v2f i) : SV_Target{
                    fixed4 col = tex2D(_MainTex, i.uv); //Ū���ثe�������z����

                    // �z���ϰ�P�_
                    float2 brushPos = _BrushPos.xy;
                    float2 uvDelta = i.uv - brushPos;
                    if (length(uvDelta) < _BrushSize && col.a > 0) {
                        col = _BrushColor;
                    }
                    clip(col.a - 0.5);

                    return col;
                }
            ENDCG
        }
    }
}                                                  
