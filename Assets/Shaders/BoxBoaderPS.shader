Shader "Custom/BoxBoaderPS" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Base (RGB) Transparency (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Transparent" }
		LOD 200
		pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex, i.uv_MainTex);
				float4 outP = texCol;
				outP.a = outP.a * 0.3;
				//outP.a = step(0.5, outP.a) * 0.3;
				return outP;
			}
				ENDCG
		}

		pass
		{

			Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off
				Cull Back
				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

				sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex, i.uv_MainTex);
				float4 outP = texCol + step(texCol.a, 0.9) * (_Color - texCol)  * _Color.a;
				return outP;
			}
				ENDCG
		}
	}
		FallBack "Diffuse"
}
