
	Shader "Live2D/MultMasked" {
		Properties{
			_Cull("Culling", Int) = 2
			_SrcColor("SrcColor", Int) = 2
			_DstColor("DstColor", Int) = 10
			_SrcAlpha("SrcAlpha", Int) = 0
			_DstAlpha("DstAlpha", Int) = 1
		}
		
		CGINCLUDE
		#pragma vertex vert 
		#pragma fragment frag
		#include "UnityCG.cginc"

			sampler2D _MainTex;
		sampler2D _ClipTex;
		float4x4 _ClipMatrix ; 
		float4   _ChannelFlag; // Color Channel Flag 

	#if ! defined( SV_Target )
		#define SV_Target	COLOR
	#endif

	#if ! defined( SV_POSITION )
		#define SV_POSITION	POSITION
	#endif
		
			
		struct v2f {
			float4 position : POSITION;
			float2 texcoord : TEXCOORD0;
			float4 clipPos : TEXCOORD1;
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
					OUT.clipPos = mul(_ClipMatrix, v.vertex);
			#if UNITY_UV_STARTS_AT_TOP
			        OUT.clipPos.y = 1.0 - OUT.clipPos.y ;// invert
			#endif
					OUT.texcoord=v.texcoord;
					OUT.color=color;
					return OUT;
				}
				
							
				float4 frag ( v2f IN) : SV_Target
				{
						
					float4 tex = tex2D(_MainTex, IN.texcoord) ;
					tex   = tex * IN.color ;
					tex.rgb   = tex.rgb  * tex.a ;
					float4 col_formask =  tex ; 				
					float4 clipMask = (1.0 - tex2D (_ClipTex, IN.clipPos.xy / IN.clipPos.w )) * _ChannelFlag ;
					float maskval = clipMask.r + clipMask.g + clipMask.b + clipMask.a ;
					col_formask = col_formask * maskval ;
					return col_formask ; 
				}
				
				ENDCG
			}
		}
	}