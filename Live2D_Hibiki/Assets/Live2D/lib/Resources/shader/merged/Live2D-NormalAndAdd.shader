
	Shader "Live2D/NormalAndAdd" {
		Properties{
			_Cull("Culling", Int) = 2
			_SrcColor("SrcColor", Int) = 5
			_DstColor("DstColor", Int) = 10
			_SrcAlpha("SrcAlpha", Int) = 1
			_DstAlpha("DstAlpha", Int) = 10
		}
		
		CGINCLUDE
		#pragma vertex vert 
		#pragma fragment frag
		#include "UnityCG.cginc"

			sampler2D _MainTex;

	#if ! defined( SV_Target )
		#define SV_Target	COLOR
	#endif

	#if ! defined( SV_POSITION )
		#define SV_POSITION	POSITION
	#endif
		
			
		struct v2f {
			float4 position : SV_POSITION;
			float2 texcoord : TEXCOORD0;
			float4 color:COLOR0;
		};
		
		ENDCG
				
		SubShader {
			Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
			LOD 100
			BindChannels{ Bind "Vertex", vertex Bind "texcoord", texcoord Bind "Color", color }

			Pass {
				Blend [_SrcColor] [_DstColor], [_SrcAlpha] [_DstAlpha] ZWrite Off Lighting Off Cull [_Cull]
				CGPROGRAM

							
				v2f vert(appdata_base v ,float4 color:COLOR)
				{
					v2f OUT;
					OUT.position = mul(UNITY_MATRIX_MVP, v.vertex);
					OUT.texcoord = v.texcoord ;
					OUT.color=color;
					return OUT;
				}
				
							
				float4 frag ( v2f IN) : SV_Target
				{
						return tex2D (_MainTex, IN.texcoord) * IN.color ; 
				}
				
				ENDCG
			}
		}
	}