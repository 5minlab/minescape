using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseController : MonoBehaviour {

	public bool mouseDown = false;
	public bool rotateCamera = false;
	public bool clickBlock = false;
	public bool rightBlock = false;
	public bool lrBlock = false;
	public Vector3 prevMousePosition = Vector2.zero;
	
	void Update() {
		if(Input.GetMouseButtonDown(1)) {
			if(EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
				return;
			}
			RightClick(Input.mousePosition);
		} else if(Input.GetMouseButton(1)) {
			MoveRight(Input.mousePosition);
		}
		if(Input.GetMouseButtonUp(1)) {
			ReleaseRight();
		}

		if(Input.GetMouseButtonDown(0)) {
			if(EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
				return;
			}
			Click(Input.mousePosition);
			
		} else if(Input.GetMouseButton(0)) {
			Move(Input.mousePosition);
		} 

		if(Input.GetMouseButtonUp(0)) {
			Release();
		}

	}
	Block centerBlock = null;
	List<Block> selectedBlocks = new List<Block>();
	void CheckLR(Vector3 mousePosition) {
		for(int i=0;i<selectedBlocks.Count;i++) {
			selectedBlocks[i].SetSelected(false);
		}
		selectedBlocks.Clear();
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
			// Debug.Log("click : " + hit.collider.gameObject.name);	
			Block block = hit.collider.gameObject.GetComponent<Block>();
			
			// Vector3 dirVec = hit.point - block.transform.position;
			// Int3 dir = Int3.Zero;
			// if(Mathf.Abs(dirVec.x) > Mathf.Abs(dirVec.y) && Mathf.Abs(dirVec.x) > Mathf.Abs(dirVec.z)) {
			// 	dir = new Int3(dirVec.x > 0 ? 1 : -1,0,0);
			// } else if(Mathf.Abs(dirVec.y) > Mathf.Abs(dirVec.x) && Mathf.Abs(dirVec.y) > Mathf.Abs(dirVec.z)) {
			// 	dir = new Int3(0,dirVec.y > 0 ? 1 : -1,0);
			// } else {
			// 	dir = new Int3(0,0,dirVec.z > 0 ? 1 : -1);
			// }
			// Debug.Log("dirVec : " +dirVec);
			// Debug.Log("dir : " +dir);
			Vector3 normal = hit.normal;
			Vector3 localNormal = Quaternion.Inverse(GamePlayer.Instance.GetRotation()) * normal;


			Int3 pos = block.pos + new Int3(localNormal);
			centerBlock = GamePlayer.Instance.GetBlock(pos);
			if(centerBlock != null) {
				for(int i=0;i<BlockHelper.SixDirections.Length;i++) {
					Block blockNow = GamePlayer.Instance.GetBlock(pos + BlockHelper.SixDirections[i]);
					if(blockNow != null && blockNow.gameObject.activeSelf) {
						selectedBlocks.Add(blockNow);
						blockNow.SetSelected(true);
					}
				}
			}
		}
	}
	void RightClick(Vector3 mousePosition) {
		if(clickBlock) {
			lrBlock = true;
			clickBlock = false;
			if(SelectedBlock) {
				SelectedBlock.SetSelected(false);
				SelectedBlock = null;
			}
			
			CheckLR(mousePosition);
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		
		if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
			Block block = hit.collider.gameObject.GetComponent<Block>();
			block.SetSelected(true);
			SelectedBlock = block;
			rightBlock = true;
		}
	}
	void MoveRight(Vector3 mousePosition) {
		if(rightBlock) {
			if(SelectedBlock != null) {
				SelectedBlock.SetSelected(false);
				SelectedBlock = null;
			}
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
				Block block = hit.collider.gameObject.GetComponent<Block>();
				SelectedBlock = block;
				block.SetSelected(true);
			}
		}
	}
	void ReleaseRight() {
		if(SelectedBlock != null) {
			SelectedBlock.SetSelected(false);
			SelectedBlock.ToggleCheck();
			SelectedBlock = null;
		}
		rightBlock = false;
	}
	Block SelectedBlock = null;
	void Click(Vector3 mousePosition) {
		if(rightBlock) {
			lrBlock = true;
			rightBlock = false;
			if(SelectedBlock) {
				SelectedBlock.SetSelected(false);
				SelectedBlock = null;
			}
			CheckLR(mousePosition);
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
			// Debug.Log("click : " + hit.collider.gameObject.name);	
			Block block = hit.collider.gameObject.GetComponent<Block>();
			
			if(!block.check) {
				//GamePlayer.Instance.OpenBlock(block.pos);
				block.SetSelected(true);
				SelectedBlock = block;
			}
			clickBlock = true;
		} else {
			rotateCamera = true;
			prevMousePosition = mousePosition;	
		}
		mouseDown = true;
		
	}
	void Move(Vector3 mousePosition) {
		if(mouseDown && rotateCamera) {
			Vector3 mouseDelta = mousePosition - prevMousePosition;
			GamePlayer.Instance.Rotate(mouseDelta);
		} else if(mouseDown && clickBlock) {
			if(SelectedBlock != null) {
				SelectedBlock.SetSelected(false);
				SelectedBlock = null;
			}
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
				Block block = hit.collider.gameObject.GetComponent<Block>();
				if(!block.check) {
					SelectedBlock = block;
					block.SetSelected(true);
				}
			}
		} else if(lrBlock) {
			CheckLR(mousePosition);
		}
		prevMousePosition = mousePosition;
	}
	void Release() {
		if(clickBlock && SelectedBlock != null) {
			SelectedBlock.SetSelected(false);
			GamePlayer.Instance.OpenBlock(SelectedBlock.pos);
			SelectedBlock = null;
			clickBlock = false;
		}
		if(lrBlock) {
			bool open = false;
			if(centerBlock != null) {
				int count = 0;
				for(int i=0;i<selectedBlocks.Count;i++) {
					if(selectedBlocks[i].check) {
						count ++;
					}
				}
				if((int)centerBlock.BlockType == count) {
					open = true;
				}
			}

			for(int i=0;i<selectedBlocks.Count;i++) {
				if(open && !selectedBlocks[i].check) {
					GamePlayer.Instance.OpenBlock(selectedBlocks[i].pos);
				}
				selectedBlocks[i].SetSelected(false);
			}
			

			selectedBlocks.Clear();
			centerBlock = null;
			lrBlock = false;
		}
		mouseDown = false;
		rotateCamera = false;
	}
}
