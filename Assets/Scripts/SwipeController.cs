using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SwipeController : MonoBehaviour {
	bool rotateCamera = false;
	bool selectBlock = false;
	public RectTransform canvasRect;
	public BlockActionUI blockActionUI;
	RectTransform uiRT;
	void Start() {
		uiRT = blockActionUI.GetComponent<RectTransform>();
	}
	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			if(EventSystem.current && EventSystem.current.IsPointerOverGameObject()) {
				return;
			}
			Click(Input.mousePosition);
		} else if(Input.GetMouseButton(0)) {
			Drag(Input.mousePosition);
		} 

		if(Input.GetMouseButtonUp(0)) {
			Release();
		}
	}

	Block SelectedBlock = null;
	Vector3 prevMousePosition;
	Vector3 initMousePosition;
	int selectIdx = -1;
	List<Block> selectedBlocks = new List<Block>();
	Block centerBlock = null;
	void Click(Vector3 mousePosition) {
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit, float.MaxValue,1 << LayerMask.NameToLayer("Block"))) {
			// Debug.Log("click : " + hit.collider.gameObject.name);	
			Block block = hit.collider.gameObject.GetComponent<Block>();
			block.SetSelected(true);
			SelectedBlock = block;
			selectBlock = true;
			
			Vector3 viewPortPoint = Camera.main.ScreenToViewportPoint(mousePosition);
			Debug.Log(viewPortPoint);
			Vector2 position = new Vector2(canvasRect.sizeDelta.x * viewPortPoint.x, canvasRect.sizeDelta.y * viewPortPoint.y);
			blockActionUI.ShowUI(position);
			initMousePosition = mousePosition;

			Vector3 normal = Quaternion.Inverse(GamePlayer.Instance.GetRotation()) * hit.normal;
			centerBlock = GamePlayer.Instance.GetBlock(block.pos + new Int3(normal));
			if(centerBlock != null) {
				for(int i=0;i<BlockHelper.SixDirections.Length;i++) {
					Block blockNow = GamePlayer.Instance.GetBlock(centerBlock.pos + BlockHelper.SixDirections[i]);
					if(blockNow != null && blockNow.gameObject.activeSelf) {
						selectedBlocks.Add(blockNow);
					}
				}
			}
		} else {
			rotateCamera = true;
			prevMousePosition = mousePosition;	
		}
	}
	static readonly float selectCut = 50f;
	void Drag(Vector3 mousePosition) {
		if(rotateCamera) {
			Vector3 mouseDelta = mousePosition - prevMousePosition;
			GamePlayer.Instance.Rotate(mouseDelta);
		} else if(selectBlock) {
			Vector3 deltaPosition = mousePosition - initMousePosition;
			deltaPosition.z = 0f;
			if(deltaPosition.magnitude > selectCut) {
				float angle = Vector3.SignedAngle(new Vector3(0f,1f,0f),deltaPosition, new Vector3(0f,0f,1f));
				if(angle < -60f) {
					selectIdx = 2;
				} else if(angle < 60f) {
					selectIdx = 1;
				} else {
					selectIdx = 0;
				}
			} else {
				selectIdx = -1;
			}
			if(selectIdx == 2) {
				for(int i=0;i<selectedBlocks.Count;i++) {
					selectedBlocks[i].SetSelected(true);
				}
			} else {
				for(int i=0;i<selectedBlocks.Count;i++) {
					selectedBlocks[i].SetSelected(false);
				}
				SelectedBlock.SetSelected(true);
			}
			blockActionUI.SetSelect(selectIdx);
		}
		prevMousePosition = mousePosition;
	}
	void Release() {
		if(rotateCamera) {
			rotateCamera = false;
		}
		if(selectBlock) {
			if(SelectedBlock != null) {
				SelectedBlock.SetSelected(false);
				if(selectIdx == -1) {
					if(!SelectedBlock.check) {
						GamePlayer.Instance.OpenBlock(SelectedBlock.pos);
					}
				} else if(selectIdx == 0) {
					SelectedBlock.ToggleCheck();
				} else if(selectIdx == 1) {

				} else if(selectIdx == 2) {
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
					if(open) {
						for(int i=0;i<selectedBlocks.Count;i++) {
							if(!selectedBlocks[i].check) {
								GamePlayer.Instance.OpenBlock(selectedBlocks[i].pos);
							}
						}
					}
				}
			}

			selectBlock = false;

			SelectedBlock = null;
			blockActionUI.HideUI();
			for(int i=0;i<selectedBlocks.Count;i++) {
				selectedBlocks[i].SetSelected(false);
			}
			selectedBlocks.Clear();
			centerBlock = null;


		}
	}

}
