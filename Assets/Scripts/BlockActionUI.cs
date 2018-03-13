using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockActionUI : MonoBehaviour {
	
	public Image[] selectImages;


	public void ShowUI(Vector2 position) {
		GetComponent<RectTransform>().anchoredPosition = position;
		gameObject.SetActive(true);		
	}
	public void HideUI() {
		gameObject.SetActive(false);
	}

	public void SetSelect(int idx) {
		for(int i=0;i<selectImages.Length;i++) {
			if(i == idx) {
				selectImages[i].color = new Color(0.9f,0.9f,0.2f,0.75f);
			} else {
				selectImages[i].color = new Color(1f,1f,1f,0.5f);
			}
		}
	}
}
