//
// Copyright (c) 2016-2018 Polarith. All rights reserved.
// Licensed under the Polarith Software and Source Code License Agreement.
// See the LICENSE file in the project root for full license information.
//

Shader "Polarith/Unlit/Splat"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Size("Size", Float) = 0.2
		_Billboard("Billboard", Int) = 0
		_Transparent("Transparent", Int) = 0
	}

	SubShader
	{
		Tags { "Queue" = "AlphaTest" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		Cull Off
		Blend One OneMinusSrcAlpha
		AlphaToMask On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"

			//----------------------------------------------------------------------------------------------------------

			sampler2D _MainTex;
			float _Size;
			int _Billboard;
			int _Transparent;

			//----------------------------------------------------------------------------------------------------------

			struct geo_input
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct frag_input
			{
				half4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			//----------------------------------------------------------------------------------------------------------

			geo_input vert(appdata_full v)
			{
				geo_input o;
				o.vertex = v.vertex;
				if(_Billboard == 0)
					o.normal = mul(unity_ObjectToWorld, v.normal);
				else
					o.normal = normalize(WorldSpaceViewDir(v.vertex));
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			[maxvertexcount(3)]
			void geom(point geo_input p[1], inout TriangleStream<frag_input> triStream)
			{
				frag_input triVertex = (frag_input)0;
				triVertex.color = p[0].color;
				if (triVertex.color.a < 0.01)
					return;

				half4 vertex = mul(unity_ObjectToWorld, p[0].vertex);
				half3 tangent = cross(half3(0, 1, 0), p[0].normal);
				half3 tangentAlternative = cross(half3(0, 0, 1), p[0].normal);
				if (length(tangent) < length(tangentAlternative))
					tangent = tangentAlternative;
						
				tangent = normalize(tangent);
				half3 up = normalize(cross(tangent, p[0].normal));

				triVertex.vertex = vertex + half4(tangent * -_Size * 0.66666 - up * _Size * 0.33333, 0);
				triVertex.vertex = mul(UNITY_MATRIX_VP, triVertex.vertex);
				triVertex.texcoord = half2(-0.5, 0);
				triStream.Append(triVertex);

				triVertex.vertex = vertex + half4(tangent * _Size * 0.66666 - up * _Size * 0.33333, 0);
				triVertex.vertex = mul(UNITY_MATRIX_VP, triVertex.vertex);
				triVertex.texcoord = half2(1.5, 0);
				triStream.Append(triVertex);

				triVertex.vertex = vertex + half4(up * _Size * 0.66666, 0);
				triVertex.vertex = mul(UNITY_MATRIX_VP, triVertex.vertex);
				triVertex.texcoord = half2(0.5, 1.5);
				triStream.Append(triVertex);
			}

			fixed4 frag(frag_input i) : COLOR
			{
				if (tex2D(_MainTex, i.texcoord).a < 0.5)
					discard;
				if (_Transparent != 0)
					i.color.a = 1.0;
				return i.color;
			}
			ENDCG
		}
	}
}
