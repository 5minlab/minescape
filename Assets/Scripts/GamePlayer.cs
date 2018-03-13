using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour {
	public static GamePlayer Instance {
		get;
		private set;
	}
	void Awake() {
		Instance = this;
	}

	public GameObject cubePrefab;
	public static readonly float blockSize = 1f;
	Transform gameParent;

	void Start() {
		InitRandom(new Int3(7,7,7),50);
		InitObject();
	}
	Int3 size;
	int totalMineCount;
	int remainMineCount;
	Dictionary<Int3, BlockType> map;
	Dictionary<Int3, Block> objs;

	public void InitRandom(Int3 size, int mineCount) {
		this.size = size;
		this.totalMineCount = mineCount;
		this.remainMineCount = mineCount;
		List<Int3> posList = new List<Int3>();
		map = new Dictionary<Int3, BlockType>();
		BlockHelper.sizeForeach(size,(Int3 pos) => {
			posList.Add(pos);
			map.Add(pos,BlockType.Num0);
		});

		for(int i=0;i<mineCount;i++) {
			int idx = Random.Range(0,posList.Count);
			Int3 mineNow = posList[idx];

			map[mineNow] = BlockType.Bomb;
			for(int j=0;j<BlockHelper.SixDirections.Length;j++) {
				Int3 dir = BlockHelper.SixDirections[j];
				Int3 pos = mineNow + dir;
				if(map.ContainsKey(pos)) {
					if(Block.IsNum(map[pos])) {
						map[pos] = (BlockType)(map[pos]+1);
					}
				}
			}


			posList.RemoveAt(idx);
		}


		// map = new bool[size.x,size.y,size.z];
	}
	public void InitObject() {
		if(gameParent != null) {
			Destroy(gameParent.gameObject);
		}
		
		objs = new Dictionary<Int3,Block>();

		GameObject parentObj = new GameObject("game");
		gameParent = parentObj.transform;
		gameParent.parent = this.transform;

		BlockHelper.sizeForeach(size, (Int3 pos) => {
			GameObject obj = GameObject.Instantiate(cubePrefab, BlockHelper.blockPosToWorldPos(size,pos,blockSize),Quaternion.identity) as GameObject;
			obj.transform.parent = gameParent;
			obj.name = pos.ToString();
			Block block = obj.GetComponent<Block>();
			block.Init(pos,map[pos]);
			objs.Add(pos,block);
		});

		SetRotation();
	}


	float theta = 0f;
	float phi = 0f;
	static readonly float phiMin = -70f;
	static readonly float phiMax = 70f;

	public void Rotate(Vector3 delta) {
		theta += delta.x / 6f;
		phi = Mathf.Clamp(phi + delta.y / 9f, phiMin,phiMax);

		SetRotation();
	}

	void SetRotation() {
		if(!gameParent) {
			return;
		}
		Quaternion rot = Quaternion.AngleAxis(phi,Vector3.right) * Quaternion.AngleAxis(-theta,Vector3.up);
		gameParent.rotation = rot;

	}
	public Quaternion GetRotation() {
		return gameParent.rotation;
	}

	public void OpenBlock(Int3 pos) {
		if(objs[pos].BlockType == BlockType.Bomb) {
			Debug.LogError("Bomb !!");
		} else {
			objs[pos].gameObject.SetActive(false);
			if(objs[pos].BlockType == BlockType.Num0) {
				for(int i=0;i<BlockHelper.SixDirections.Length;i++) {
					Int3 dir = BlockHelper.SixDirections[i];
					Int3 newPos = pos + dir;
					if(objs.ContainsKey(newPos) && objs[newPos].gameObject.activeSelf && !objs[newPos].check) {
						OpenBlock(newPos);
					}
				}
			} else {
				for(int i=0;i<BlockHelper.SixDirections.Length;i++) {
					Int3 dir = BlockHelper.SixDirections[i];
					Int3 newPos = pos + dir;
					if(objs.ContainsKey(newPos) && objs[newPos].gameObject.activeSelf) {
						objs[newPos].SetNum(-dir,Block.GetBombNum(objs[pos].BlockType));
					}
				}
			}
			
		}
		
	}

	public Block GetBlock(Int3 pos) {
		if(objs.ContainsKey(pos)) {
			return objs[pos];
		} else {
			return null;
		}
	}
}
