using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType {
	Void = -1,
	Num0 = 0,
	Num1 = 1,
	Num2 = 2,
	Num3 = 3,
	Num4 = 4,
	Num5 = 5,
	Num6 = 6,
	Bomb = 99,
}
public class Block : MonoBehaviour {
	public Material defaultMaterial;
	public Material checkedMaterial;
	public MeshRenderer meshRenderer;
	
	public MeshRenderer leftQuad;
	public MeshRenderer rightQuad;
	public MeshRenderer topQuad;
	public MeshRenderer bottomQuad;
	public MeshRenderer forwardQuad;
	public MeshRenderer backwardQuad;
	
	public Material[] numMaterials;
	
	public BlockType BlockType {
		get;
		private set;
	}
	public Int3 pos {
		get; 
		private set;
	}
	public bool check {
		get;
		private set;
	}
	public void Init(Int3 pos, BlockType blockType) {
		this.BlockType = blockType;
		this.pos = pos;
		this.check = false;
	}

	public void ToggleCheck() {
		this.check = !this.check;
		meshRenderer.material = check ? checkedMaterial : defaultMaterial;
	}

	public static bool IsNum(BlockType type) {
		return 0 <= (int)type && (int)type <= 6;
	}
	public static int GetBombNum(BlockType type) {
		return (int)type;
	}
	public void SetNum(Int3 direction, int num) {
		MeshRenderer quad = GetQuad(direction);
		if(quad) {
			Material mat = numMaterials[num];
			if(mat != null) {
				quad.sharedMaterial = mat;
				quad.gameObject.SetActive(true);
			}
		}
	}

	public MeshRenderer GetQuad(Int3 direction) {
		if(direction == new Int3(1,0,0)) {
			return rightQuad;
		} else if(direction == new Int3(-1,0,0)) {
			return leftQuad;
		} else if(direction == new Int3(0,1,0)) {
			return topQuad;
		} else if(direction == new Int3(0,-1,0)) {
			return bottomQuad;
		} else if(direction == new Int3(0,0,1)) {
			return forwardQuad;
		} else if(direction == new Int3(0,0,-1)) {
			return backwardQuad;
		} else {
			return null;
		}
	}
	public bool IsSelected {
		get;
		private set;
	}

	static readonly float selectedScale = 0.8f;
	static readonly float defaultScale = 1f;
	static readonly float scaleChangeSpeed = 2f;
	float currentScale = 1f;

	public void SetSelected(bool selected) {
		IsSelected = selected;
	}

	void Update() {
		if(IsSelected) {
			if(currentScale > selectedScale) {
				currentScale = Mathf.Clamp(currentScale - Time.deltaTime * scaleChangeSpeed,selectedScale,defaultScale);
				meshRenderer.transform.localScale = Vector3.one * currentScale;
			}
		} else {
			if(currentScale < defaultScale) {
				currentScale = Mathf.Clamp(currentScale + Time.deltaTime * scaleChangeSpeed,selectedScale,defaultScale);
				meshRenderer.transform.localScale = Vector3.one * currentScale;
			}
		}
	}

}
