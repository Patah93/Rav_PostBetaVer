Shader "Texture and grain texture"{
	Properties {
		_Color ("Color Tint", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("Diffuse Texture", 2D) = "white" {}
		_GrainTex ("Grain Texture", 2D) = "white" {}
		_GrainStrength ("Grain Strength", range(1.0,1.0)) = 1.0
	}
	SubShader {
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash
			
			//user defined variables
			uniform sampler2D _MainTex;
			uniform sampler2D _GrainTex;		
			uniform float4 _MainTex_ST;
			uniform float4 _GrainTex_ST;
			uniform float4 _Color;
			uniform float _GrainStrength;		
			
			//unity defined variables
			uniform float4 _LightColor0;
			
			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};
			
			//vertex Function
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				o.posWorld = mul(_Object2World, v.vertex);
				o.normalDir = normalize( mul( float4( v.normal, 0.0 ), _World2Object ).xyz );
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				
				return o;
			}
			
			//fragment function
			float4 frag(vertexOutput i) : COLOR
			{
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
				float3 lightDirection;
				float atten;
				
				if(_WorldSpaceLightPos0.w == 0.0){ //directional light
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else{
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float distance = length(fragmentToLightSource);
					atten = 1.0/distance;
					lightDirection = normalize(fragmentToLightSource);
				}
				
				//Lighting
				float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
				
				//Texture Maps
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				float4 texG = tex2D(_GrainTex, i.tex.xy * _GrainTex_ST.xy + _GrainTex_ST.zw);
								
				float3 lightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection ;
				float3 grainFinal = texG * _GrainStrength;
				
				return float4(tex.xyz * lightFinal * _Color.xyz * grainFinal, 1.0);
			}
			
			ENDCG
			
		}
		
		Pass {
			Tags {"LightMode" = "ForwardAdd"}
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash
			
			//user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float4 _GrainTex_ST;
			uniform sampler2D _GrainTex;
			uniform float _GrainStrength;
												
			//unity defined variables
			uniform float4 _LightColor0;
			
			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};
			
			//vertex Function
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				
				o.posWorld = mul(_Object2World, v.vertex);
				o.normalDir = normalize( mul( float4( v.normal, 0.0 ), _World2Object ).xyz );
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				
				return o;
			}
			
			//fragment function
			
			float4 frag(vertexOutput i) : COLOR
			{
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
				float3 lightDirection;
				float atten;
				
				if(_WorldSpaceLightPos0.w == 0.0){ //directional light
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else{
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float distance = length(fragmentToLightSource);
					atten = 1.0/distance;
					lightDirection = normalize(fragmentToLightSource);
				}
				
				//Lighting
				float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
				
				//Texture Maps
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				float4 texG = tex2D(_GrainTex, i.tex.xy * _GrainTex_ST.xy + _GrainTex_ST.zw);			
				
				float3 lightFinal = diffuseReflection;
				float3 grainFinal = texG * _GrainStrength;				
			
				
				return float4(lightFinal * _Color.xyz * grainFinal, 1.0);
			}
			
			ENDCG
			
		}
	}
	//Fallback "Specular"
}