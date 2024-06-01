Shader "MadeByProfessorOakie/SimpleSonarShader2D" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RingColor("Ring Color", Color) = (1,1,1,1)
        _RingColorIntensity("Ring Color Intensity", float) = 2
        _RingSpeed("Ring Speed", float) = 1
        _RingWidth("Ring Width", float) = 0.1
        _RingIntensityScale("Ring Range", float) = 1
        _RingTex("Ring Texture", 2D) = "white" {}
    }
    SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _RingTex;
            float4 _MainTex_ST;
            float4 _hitPts[20];
            float _StartTime;
            float _Intensity[20];
            float4 _Color;
            float4 _RingColor;
            float _RingColorIntensity;
            float _RingSpeed;
            float _RingWidth;
            float _RingIntensityScale;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 c = tex2D(_MainTex, IN.uv) * _Color;
                float2 screenPos = IN.vertex.xy / IN.vertex.w;
                screenPos.y = 1 - screenPos.y;

                for (int i = 0; i < 20; i++) {
                    float d = distance(_hitPts[i].xy, screenPos);
                    float intensity = _Intensity[i] * _RingIntensityScale;
                    float val = (1 - (d / intensity));

                    if (d < (_Time.y - _hitPts[i].w) * _RingSpeed && d >(_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth && val > 0) {
                        float posInRing = (d - ((_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth)) / _RingWidth;
                        val *= tex2D(_RingTex, float2(1 - posInRing, 0.5));
                        c.rgb = _RingColor.rgb * val + c.rgb * (1 - val);
                        c.rgb *= _RingColorIntensity;
                    }
                }

                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}