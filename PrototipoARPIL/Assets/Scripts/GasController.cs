using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour {

	[Range(0.1f, 1)]
	public float emptyingSpeed = 0.5f;
	public RectTransform slider;
	float _gasQuantity = 0;

	void Update () {
		if (_gasQuantity > 0) {
			_gasQuantity -= emptyingSpeed;
			float _sliderValue = _gasQuantity * 1.5f;
			if (_sliderValue < 0)
				_sliderValue = 0;
			slider.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, _sliderValue);
		} else if (_gasQuantity <= 0) {
			if (Input.GetKeyUp ("z"))
				_gasQuantity = 100;
		}
	}
}
