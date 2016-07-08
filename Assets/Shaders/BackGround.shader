Shader "Custom/BackGround" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
	}
		SubShader{
			//Tags{ "Queue" = "BackGround" }
		//ZWrite off
		LOD 200
		pass
		{
			Cull Front
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			float4 _Color;

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				return _Color;
			}
				ENDCG
		}
	}
		FallBack "Diffuse"
}
