Shader "Custom/BlockNoiseTextShader"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _FaceColor ("Face Color", Color) = (1,1,1,1)
        _NoiseIntensity ("Noise Intensity", Range(0,1)) = 0.5
        _NoiseScale ("Noise Scale", Range(10, 50)) = 30.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _FaceColor;
            float _NoiseIntensity;
            float _NoiseScale;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 文字のアルファを取得し、シャープ化
                fixed4 textTex = tex2D(_MainTex, i.uv);
                float alpha = smoothstep(0.3, 0.7, textTex.a); // 文字のエッジをクッキリ

                // ノイズの計算（ブロック状）
                float noiseX = floor(i.uv.x * _NoiseScale) / _NoiseScale;
                float noiseY = floor(i.uv.y * _NoiseScale) / _NoiseScale;
                float noise = frac(sin(dot(float2(noiseX, noiseY) * _Time.y * 10.0, float2(12.9898, 78.233))) * 43758.5453);
                noise = step(0.5, noise) * _NoiseIntensity;

                // 文字の色を適用しつつノイズを加える
                fixed4 finalColor = _FaceColor;
                finalColor.rgb *= (1.0 - noise); // ノイズで色を変化
                finalColor.a = alpha; // 文字の形状を維持

                return finalColor;
            }
            ENDCG
        }
    }
}
