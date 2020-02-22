Shader "Custom/Unlit/BaseShader"
{
    Properties
    {
		_Color("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue" = "Geometry"  "RenderType"="Opaque" }

        Pass
        {
			Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest

            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 pos : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				float4 color : TEXCOORD0;
				LIGHTING_COORDS(1,2)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
				float3 worldNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal,0.0)).xyz);

				fixed NdotL = max(0,dot(normalize(_WorldSpaceLightPos0.xyz), worldNormal)) * .5 + .5;
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				o.color = v.color * NdotL;
				
                return o;
            }

			fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
				return _Color * i.color * (LIGHT_ATTENUATION(i) * .5 + .5);
            }
            ENDCG
        }
    }
	Fallback "Diffuse"
}
