using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This is an example class showing how we format our code
/// and what conventions we use
/// </summary>
public class FormattingTemplate : MonoBehaviour
{
	// Comments is only used in when absolutely needed

	// Const on top, then static
	// Sorte most visible to least visible
	public const int ConstantPublicValue = 0;

	private const int ConstantValue = 0;
	private static int aStaticFoo = 0;

	public enum Enum {index1,index2, indexX };
	public Enum myEnum = Enum.index1;

	// Inspector fields are private and explicitly serialized
	// They are also initialized to prevent compiler warnings
	[SerializeField] private float myValue = 0;
	[SerializeField] private Texture myReference = null;
	[SerializeField] private Sprite myImportantReference = null;

	[SerializeField] private Transform mySceneReference = null;

	// It's always a good idea to use Range, Tooltip or other means of helping the designer
	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("How much swagger should the entity have?")]
	private float swagger = 0f;

	// Private state is right below, but still above properties and methods
	private int anotherValue;

	// Auto properties are named like fields. We don't use public fields unless we really need them
	public int aPublicValue { get; set; }

	// Regular properties use a preceding underscore in their backing fields
	private int _superSecretValue;
	public int superSecretValue
	{
		get { return _superSecretValue; }
		set { _superSecretValue = value; }
	}

	// Unity messages come before the rest of the methods
	// They are kept short and delegate work to suitably named subroutines

	// Use Awake to setup the instance and create any references
	void Awake()
	{
		DigHole();
		FillWithCement();
	}

	// Use Start to start any behaviour or action this component is doing
	// (Can be a coroutine)
	IEnumerator Start()
	{
		StartDancing();
		yield return new WaitForSeconds(10f);
		FallAsleep();
	}

	// Methods use PascalCase. They are also as private as possible

	private void StartDancing()
	{
		// Using the fields to avoid warnings
		var v = myValue;
		var r1 = myReference;
		var r2 = mySceneReference;
		var r3 = myImportantReference;
		var s = aStaticFoo;
		var w = swagger;
		// Split long parameter lists with newlines. Add the first one right after the first bracket.
		// Use LogError, not Log (mostly)
		Debug.LogError(string.Format(
			"Values are {0}, {1}, {2}, {3}, {4} and {5}. This script should not be used. It is for demonstrating format.", v, r1, r2, r3, s, w),
			this); // It is often useful to add a reference (usually 'this') to a Log call
	}

	private void FallAsleep()
	{
		throw new NotImplementedException();
	}

	private void DigHole()
	{
		throw new NotImplementedException();
	}

	private void FillWithCement()
	{
		// Preprocessor directives on col 0
#if UNITY_EDITOR
		throw new NotImplementedException();
#endif
	}
}
