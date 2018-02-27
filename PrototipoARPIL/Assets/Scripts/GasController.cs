using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour {

	[Range(0.1f, 1)]
	public float emptyingSpeed = 0.2f;
	[Range(0.7f, 2)]
	public float fillingSpeed = 1.8f;
	public RectTransform slider;
	[HideInInspector]
	public float _gasQuantity = 100;
	[HideInInspector]
	public bool Refilling = false;

	void Update () {
		if (_gasQuantity > 0 && !Refilling) {
			_gasQuantity -= emptyingSpeed;
			if (_gasQuantity < 0)
				_gasQuantity = 0;
			float _sliderValue = _gasQuantity * 1.5f;
			slider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderValue);
		} else if (Refilling) {
			_gasQuantity += fillingSpeed;
			if (_gasQuantity > 100) {
				_gasQuantity = 100;
				Refilling = false;
			}
			float _sliderValue = _gasQuantity * 1.5f;
			slider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderValue);
		} else {
			GetComponent<PlayerTruck>()._slowDown = true;
		}
	}
}
