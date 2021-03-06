﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasController : MonoBehaviour {

	[Range(0.1f, 1)]
	public float emptyingSpeed = 0.2f;
	[Range(0.7f, 2)]
	public float fillingSpeed = 1.8f;
	public RectTransform CooldownGas;
	float _cooldownGasFactor;
	[HideInInspector]
	public bool Refilling = false;
	float _gasQuantity = 100;

	public System.EventHandler OnNoGas;

	void Start() {
		_cooldownGasFactor = CooldownGas.GetComponent<RectTransform>().rect.width / 100;
	}

	void Update () {
		if (_gasQuantity > 0 && !Refilling) {
			_gasQuantity -= emptyingSpeed;
			if (_gasQuantity < 0) {
				if (OnNoGas != null)
					OnNoGas(this, System.EventArgs.Empty);
				_gasQuantity = 0;
			}
			float _sliderValue = _gasQuantity * _cooldownGasFactor;
			CooldownGas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderValue);
		} else if (Refilling) {
			_gasQuantity += fillingSpeed;
			if (_gasQuantity > 100) {
				_gasQuantity = 100;
				Refilling = false;
			}
			float _sliderValue = _gasQuantity * _cooldownGasFactor;
			CooldownGas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderValue);
		} else {
			GetComponent<PlayerTruck>()._slowDown = true;
		}
	}
}
