using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour {

	// Delegates are kind of like variables, but they contain functions

	// This delegate will take any function with a return type of void and which takes one int
	// argument.
	delegate void MyDelegate(int num);
	MyDelegate myDelegate;

	// Delegates can also contain multiple functions at once!
	delegate void MultiDelegate();
	MultiDelegate myMultiDelegate;

	void Start () {
		myDelegate = PrintNum;
		myDelegate(50);

		myDelegate = DoubleNum;
		myDelegate(50);

		// Use += to assign multiple functions to a delegate.
		myMultiDelegate += PowerUp;
		myMultiDelegate += TurnRed;

		// This will call all functions attached to myMultiDelegate
		if (myMultiDelegate != null)
		{
			myMultiDelegate();
		}
	}
	
	void PrintNum(int num) {
		print ("Print Num: " + num);
	}

	void DoubleNum(int num) {
		print ("Double Num: " + num * 2);
	}

	void PowerUp() {
		print ("Powering Up!");
	}

	void TurnRed() {
		print ("Turning Red!");
	}
}