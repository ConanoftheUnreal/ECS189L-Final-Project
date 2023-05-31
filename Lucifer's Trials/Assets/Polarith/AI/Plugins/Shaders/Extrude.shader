//
// Copyright (c) 2016-2018 Polarith. All rights reserved.
// Licensed under the Polarith Software and Source Code License Agreement.
// See the LICENSE file in the project root for full license information.
//

Shader "Polarith/Unlit/Extrude"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BaseSize("BaseSize", Float) = 0.2
		_Length("Length", Float) = 1
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
			float _Length;
			int _Transparent;

			//----------------------------------------------------------------------------------------------------------

			struct geo_input
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct frag_input
			{
				half4 vertex : SV_POSITION;
				half4 color : COLOR;
			};

			//----------------------------------------------------------------------------------------------------------

			geo_input vert(appdata_full v)
			{
				geo_input o;
				o.vertex = v.vertex;
				o.normal = mul(unity_ObjectToWorld, v.normal);
				o.color = v.color;
				return o;
			}

			[maxvertexcount(30)]
			void geom(point geo_input p[1], inout TriangleStream<frag_input> triStream)
			{
				frag_input triVertex = (frag_input)0;
				triVertex.color = p[0].color;
				float magnitude = triVertex.color.a;
				if (magnitude < 0.01)
					return;

				half4 vertex = mul(unity_ObjectToWorld, p[0].vertex);
				half3 tangent = cross(half3(0, 1, 0), p[0].normal);
				half3 tangentAlternative = cross(half3(0, 0, 1), p[0].normal);
				if (length(tangent) < length(tangentAlternative))
					tangent = tangentAlternative;
				tangent = normalize(tangent);
				half3 up = normalize(cross(tangent, p[0].normal));

				half3 tanDir = tangent * _Size * 0.5;
				half3 upDir = up * _Size * 0.5;
				half3 scaledNormal = p[0].normal * _Length * magnitude;

				//// Base Triangles ////
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				//// Side 1 ////
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				//// Side 2 ////
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				//// Side 3 ////
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(-tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				//// Side 4 ////
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();

				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir - upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triVertex.vertex = mul(UNITY_MATRIX_VP, vertex + half4(tanDir + upDir + scaledNormal, 0));
				triStream.Append(triVertex);
				triStream.RestartStrip();
			}

			fixed4 frag(frag_input i) : COLOR
			{
				if (_Transparent != 0)
					i.color.a = 1.0;
				return i.color;
			}
			ENDCG
		}
	}
}
