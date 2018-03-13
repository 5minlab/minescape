using UnityEngine;

public class BlockHelper {
	public delegate void sizeForeachDelegate(Int3 a);
	public static void sizeForeach(Int3 size, sizeForeachDelegate del) {
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.y; j++) {
				for (int k = 0; k < size.z; k++) {
					del (new Int3 (i, j, k));
				}
			}
		}
	}

	/*
	public static Vector3 blockPosToWorldPos(Int3 size, Int3 blockPos, float scale) {
		//y coordinate is diffrent. y = 0 is bottom.
		return (new Vector3(-size.x, 0f,-size.z) / 2f + blockPos.ToVec3 () + new Vector3(0.5f,0.5f,0.5f)) * scale;
	}
	public static Vector3 blockPosToWorldPos(Int3 size, Vector3 blockPos, float scale) {
		return (new Vector3(-size.x,0f,-size.z) /2f + blockPos + new Vector3(0.5f,0.5f,0.5f)) * scale;
	}
	public static Int3 WorldPosToblockpos(Int3 size, Vector3 worldPos, float scale) {
		return new Int3 (worldPos/scale - new Vector3 (0.5f, 0.5f, 0.5f) + new Vector3(size.x, 0f, size.z) / 2f);
	}*/

	public static Vector3 blockPosToWorldPos(Int3 size, Int3 blockPos, float scale) {
		//y coordinate is diffrent. y = 0 is bottom.
		return (new Vector3(-size.x, -size.y, -size.z) / 2f + blockPos.ToVec3 () + new Vector3(0.5f,0.5f,0.5f)) * scale;
	}
	public static Vector3 blockPosToWorldPos(Int3 size, Vector3 blockPos, float scale) {
		return (new Vector3(-size.x,-size.y,-size.z) /2f + blockPos + new Vector3(0.5f,0.5f,0.5f)) * scale;
	}
	public static Int3 WorldPosToblockpos(Int3 size, Vector3 worldPos, float scale) {
		return new Int3 (worldPos/scale - new Vector3 (0.5f, 0.5f, 0.5f) + new Vector3(size.x, size.y, size.z) / 2f);
	}


	public static Int3[] SixDirections = new Int3[6]{new Int3(1,0,0), new Int3(-1,0,0), new Int3(0,1,0), new Int3(0,-1,0), new Int3(0,0,1), new Int3(0,0,-1)};
}