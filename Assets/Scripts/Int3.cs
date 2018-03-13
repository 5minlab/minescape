using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Int3 : IEquatable<Int3>
{
	public int x;
	public int y;
	public int z;

	public int i {
		get { return x; }
	}
	public int j {
		get{ return y; }
	}
	public int k {
		get { return z; }
	}
	public Int3(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	/*
	public Int3(float x, float y, float z)
	{
		this.x = Mathf.RoundToInt (x);
		this.y = Mathf.RoundToInt (y);
		this.z = Mathf.RoundToInt (z);
	}*/

	public Int3(Vector3 vec) {
		this.x = Mathf.RoundToInt (vec.x);
		this.y = Mathf.RoundToInt (vec.y);
		this.z = Mathf.RoundToInt (vec.z);
	}
	public Int3(Int3 other) {
		this.x = other.x;
		this.y = other.y;
		this.z = other.z;
	}
	private static Int3 zero = new Int3 (0, 0, 0);
	private static Int3 one = new Int3(1,1,1);
	public static Int3 Zero {
		get { return zero;}
	}
	public static Int3 One {
		get { return one;}
	}

	public static Int3 operator +(Int3 a, Int3 b)
	{
		a.x += b.x;
		a.y += b.y;
		a.z += b.z;
		return a;
	}
	public static Int3 operator -(Int3 a) {
		a.x = -a.x;
		a.y = -a.y;
		a.z = -a.z;
		return a;
	}
	public static Int3 operator -(Int3 a, Int3 b) {
		a.x = a.x - b.x;
		a.y = a.y - b.y;
		a.z = a.z - b.z;
		return a;
	}
	public static Int3 operator *(Int3 a, int c) {
		a.x = a.x * c;
		a.y = a.y * c;
		a.z = a.z * c;
		return a;
	}
	public static Int3 operator *(int c, Int3 a) {
		a.x = a.x * c;
		a.y = a.y * c;
		a.z = a.z * c;
		return a;
	}
	public static Int3 operator /(Int3 a, int c) {
		a.x = a.x / c;
		a.y = a.y / c;
		a.z = a.z / c;
		return a;
	}
	public static Int3 operator *(Int3 a, Int3 b) {
		a.x = a.x * b.x;
		a.y = a.y * b.y;
		a.z = a.z * b.z;
		return a;
	}
	public static bool operator <=(Int3 a, Int3 b) {
		return (a.x <= b.x) && (a.y <= b.y) && (a.z <= b.z);
	}
	public static bool operator >=(Int3 a, Int3 b) {
		return (a.x >= b.x) && (a.y >= b.y) && (a.z >= b.z);
	}

	public override bool Equals(object obj)
	{
		return (obj is Int3) ? this == (Int3)obj : false;
	}

	public bool Equals(Int3 other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{
		return this.x << 22 ^ this.y << 11 ^ this.z;
	}

	public static bool operator ==(Int3 a, Int3 b)
	{
		return a.x == b.x && a.y == b.y && a.z == b.z;
	}

	public static bool operator !=(Int3 a, Int3 b)
	{
		return !(a == b);
	}
	public Vector3 ToVec3() {
		return new Vector3 (x, y, z);
	}
	/*
	public Int2 ToInt2()
	{
		return new Int2(x, z);
	}*/

	public static Int3 Clamp (Int3 value,Int3 min,Int3 max) {
		min.x = Mathf.Clamp (value.x, min.x, max.x);
		min.y = Mathf.Clamp (value.y, min.y, max.y);
		min.z = Mathf.Clamp (value.z, min.z, max.z);
		return min;
	}

	public static Int3 Min(Int3 a, Int3 b) {
		a.x = Mathf.Min (a.x, b.x);
		a.y = Mathf.Min (a.y, b.y);
		a.z = Mathf.Min (a.z, b.z);
		return a;
	}
	public static Int3 Max(Int3 a, Int3 b) {
		a.x = Mathf.Max (a.x, b.x);
		a.y = Mathf.Max (a.y, b.y);
		a.z = Mathf.Max (a.z, b.z);
		return a;
	}
	public static Int3 Abs(Int3 a) {
		a.x = Mathf.Abs (a.x);
		a.y = Mathf.Abs (a.y);
		a.z = Mathf.Abs (a.z);
		return a;
	}
	public static Int3 Sign(Int3 a) {
		int x = 0;
		int y = 0;
		int z = 0;
		if (a.x > 0) {
			x = 1;
		} else if(a.x == 0) {
			x = 0;
		} else {
			x = -1;
		}
		if (a.y > 0) {
			y = 1;
		} else if(a.y == 0) {
			y = 0;
		} else {
			y = -1;
		}
		if (a.z > 0) {
			z = 1;
		} else if(a.z == 0) {
			z = 0;
		} else {
			z = -1;
		}
		a.x = x;
		a.y = y;
		a.z = z;
		return a;
	}
	public static Int3 Rotate(Int3 a, int rot) {
		if (rot == 0) {
			return a;
		} else if (rot == 1) {
			int x = a.z;
			int z = -a.x;
			a.x = x;
			a.z = z;
			return a;
		} else if (rot == 2) {
			a.x = -a.x;
			a.z = -a.z;
			return a;
		} else if (rot == 3) {
			int x = -a.z;
			int z = a.x;
			a.x = x;
			a.z = z;
			return a;
		} else {
			while (rot < 0) {
				rot += 4;
			}
			rot = rot % 4;
			return Rotate (a, rot);
		}
	}
	public static Int3 Scale(Int3 a, Int3 b) {
		return new Int3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}
	public static int Dot(Int3 a, Int3 b) {
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}
	public static int Manhattan(Int3 a, Int3 b) {
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y) + Mathf.Abs (a.z - b.z);
	}
	public override string ToString ()
	{
		return string.Format ("[{0},{1},{2}]", i, j, k);
	}
}