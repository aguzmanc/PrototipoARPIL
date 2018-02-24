using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour {

	[Range(0.1f, 1)]
	public float emptyingSpeed = 0.3f;
	public RectTransform slider;
	[Range(0, 100)]
	public float _gasQuantity = 100;

	void Update () {
		if (_gasQuantity > 0) {
			_gasQuantity -= emptyingSpeed;
			float _sliderValue = _gasQuantity * 1.5f;
			if (_sliderValue < 0)
				_sliderValue = 0;
			slider.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, _sliderValue);
		} else {
			GetComponent<PlayerTruck>()._slowDown = true;
		}
	}

	private void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag("Gas")) {
			_gasQuantity = 100;
			GetComponent<PlayerTruck>()._slowDown = false;
		}
	}
}
