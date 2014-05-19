Shader "Custom/Test" {
	SubShader {
	
		Pass{
			Tags { "RenderType"="Opaque" }
			Cull front
			LOD 200
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			struct Input {
				float4 pos : SV_POSITION;
				float3 color : COLOR0;
			};

			Input vert (appdata_base v){
				Input o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.pos = float4(o.pos.xyz + v.normal * sin(o.pos.x) * _SinTime.w, o.pos.w);
				
				o.color = v.normal * 0.5 + 0.5;
				
				return o;
			
			}
			
			half4 frag(Input i) : COLOR {		
				return half4(i.color,1);		
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
